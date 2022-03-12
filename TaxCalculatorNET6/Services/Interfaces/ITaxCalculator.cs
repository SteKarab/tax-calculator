using TaxCalculatorNET6.Dtos;

namespace TaxCalculatorNET6.Services.Interfaces;

public interface ITaxCalculator
{
    Task<TaxesDto> CalculateTaxes(TaxPayerDto taxPayer);
}