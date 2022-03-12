using TaxCalculatorNET6.Models;

namespace TaxCalculatorNET6.Services.Interfaces;

public interface ITaxParamsService
{
    Task<TaxParams> GetParamsFor(string country, string version);
}