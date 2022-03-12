using TaxCalculatorNET6.Data;
using TaxCalculatorNET6.Exceptions;
using TaxCalculatorNET6.Models;
using TaxCalculatorNET6.Services.Interfaces;

namespace TaxCalculatorNET6.Services;

public class TaxParamsService: ITaxParamsService
{
    private TaxPayerContext _context;
    
    public TaxParamsService(TaxPayerContext context)
    {
        _context = context;
    }

    public async Task<TaxParams> GetParamsFor(string? country, string? version)
    {
        var result = await _context.TaxParams.FindAsync(country ?? "Imagiaria", version ?? "1");

        if (result == null)
        {
            throw new TaxParamsNotFoundException();
        }

        return result;
    }
}