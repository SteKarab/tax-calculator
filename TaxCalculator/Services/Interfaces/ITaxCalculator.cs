using TaxCalculator.Dtos;

namespace TaxCalculator.Services.Interfaces;

public interface ITaxCalculator
{
    Task<TaxesDto> CalculateTaxes(TaxPayerDto taxPayer);
}