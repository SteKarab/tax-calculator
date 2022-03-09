using Mapster;
using TaxCalculator.Data;
using TaxCalculator.Dtos;
using TaxCalculator.Models;
using TaxCalculator.Services.Interfaces;

namespace TaxCalculator.Services;

public class TaxCalculatorService: ITaxCalculator
{
    private TaxPayerContext _context;
    private ITaxParamsService _taxParamsService;

    public TaxCalculatorService(TaxPayerContext context, ITaxParamsService taxParamsService)
    {
        _context = context;
        _taxParamsService = taxParamsService;
    }
    
    public async Task<TaxesDto> CalculateTaxes(TaxPayerDto taxPayer)
    {
        var taxParams = await _taxParamsService.GetParamsFor(taxPayer.Country, taxPayer.Version);
        var existingTaxes = await _context.PayerTaxes.FindAsync(taxPayer.SSN);

        if (existingTaxes != null)
        {
            return existingTaxes.Adapt<TaxesDto>();
        }

        var taxes = new TaxesDto
        {
            CharitySpent = taxPayer.CharitySpent,
            GrossIncome = taxPayer.GrossIncome,
            NetIncome = taxPayer.GrossIncome,
            SSN = taxPayer.SSN
        };

        (var taxableIncome, var socialTaxableIncome) = CalculateBaselines(taxPayer, taxParams);

        if (taxableIncome <= taxParams.IncomeTaxFloor) return taxes;

        taxes.IncomeTax = CalculateIncomeTax(taxableIncome, taxParams);
        taxes.SocialTax = CalculateSocialTax(socialTaxableIncome, taxParams);
        taxes.TotalTax = taxes.SocialTax + taxes.IncomeTax;
        taxes.NetIncome = taxes.GrossIncome - taxes.TotalTax;

        await _context.AddAsync(taxes.Adapt<Taxes>());
        await _context.SaveChangesAsync();

        return taxes;
    }

    private (decimal taxableIncome, decimal socialTaxableIncome) CalculateBaselines(TaxPayerDto taxPayer, TaxParams taxParams)
    {
        var charity = Math.Min(Math.Round(taxPayer.GrossIncome * taxParams.AllowedCharityPercentage / 100), taxPayer.CharitySpent);

        var taxableIncome = taxPayer.GrossIncome - charity;
        var socialTaxableIncome = taxableIncome;

        return (taxableIncome, socialTaxableIncome);
    }

    private decimal CalculateIncomeTax(decimal taxableIncome, TaxParams taxParams)
    {
        taxableIncome -= taxParams.IncomeTaxFloor;
        var incomeTax = Math.Round(taxableIncome * taxParams.IncomeTaxPercentage / 100, 2);
        return incomeTax;
    }

    private decimal CalculateSocialTax(decimal socialTaxableIncome, TaxParams taxParams)
    {
        socialTaxableIncome -= taxParams.SocialTaxFloor;
        var defaultSocialRange = taxParams.SocialTaxCeiling - taxParams.SocialTaxFloor;
        var taxableSocialAmount = Math.Min(defaultSocialRange, socialTaxableIncome);
        var socialTax = Math.Round(taxableSocialAmount * taxParams.SocialTaxPercentage / 100, 2);

        return socialTax;
    }
}