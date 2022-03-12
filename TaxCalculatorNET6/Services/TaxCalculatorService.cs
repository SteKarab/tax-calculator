using Mapster;
using TaxCalculatorNET6.Data;
using TaxCalculatorNET6.Domain.TaxCalculations.Interfaces;
using TaxCalculatorNET6.Dtos;
using TaxCalculatorNET6.Exceptions;
using TaxCalculatorNET6.Models;
using TaxCalculatorNET6.Services.Interfaces;
using TaxCalculatorNET6.Services.Interfaces;

namespace TaxCalculatorNET6.Services;

public class TaxCalculatorService: ITaxCalculator
{
    private TaxPayerContext _context;
    private ITaxParamsService _taxParamsService;
    private ITaxCalculationState _calculationState;
    private IOrderedEnumerable<ITaxCalculation> _taxCalculations;

    public TaxCalculatorService(TaxPayerContext context, ITaxParamsService taxParamsService, ITaxCalculationStateFactory stateFactory, IEnumerable<ITaxCalculation> taxCalculations)
    {
        _context = context;
        _taxParamsService = taxParamsService;
        _calculationState = stateFactory.GetNewState();
        _taxCalculations = taxCalculations.OrderBy(x => x.Rank);
    }
    
    public async Task<TaxesDto> CalculateTaxes(TaxPayerDto taxPayer)
    {
        if (!taxPayer.GrossIncome.HasValue || !taxPayer.CharitySpent.HasValue || taxPayer.GrossIncome < 0 || taxPayer.CharitySpent < 0) throw new InvalidTaxInformationException();
        
        var taxParams = await _taxParamsService.GetParamsFor(taxPayer.Country, taxPayer.Version);
        var existingTaxes = await _context.PayerTaxes.FindAsync(taxPayer.SSN);

        if (existingTaxes != null)
        {
            return existingTaxes.Adapt<TaxesDto>();
        }

        var taxes = new TaxesDto
        {
            CharitySpent = taxPayer.CharitySpent ?? 0,
            GrossIncome = taxPayer.GrossIncome ?? 0,
            SSN = taxPayer.SSN
        };

        foreach (var taxCalculation in _taxCalculations)
        {
            taxCalculation.Execute(_calculationState, taxPayer, taxParams);
            if (_calculationState.IsComplete) break;
        }

        taxes.IncomeTax = _calculationState.IncomeTax;
        taxes.SocialTax = _calculationState.SocialTax;
        taxes.TotalTax = _calculationState.TotalTax;
        taxes.NetIncome = taxes.GrossIncome - taxes.TotalTax;

        await _context.AddAsync(taxes.Adapt<Taxes>());
        await _context.SaveChangesAsync();

        return taxes;
    }
}