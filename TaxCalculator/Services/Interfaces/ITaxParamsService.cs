using TaxCalculator.Models;

namespace TaxCalculator.Services.Interfaces;

public interface ITaxParamsService
{
    Task<TaxParams> GetParamsFor(string country, string version);
}