using TaxCalculator.Data;
using TaxCalculator.Exceptions;
using TaxCalculator.Models;
using TaxCalculator.Services.Interfaces;

namespace TaxCalculator.Services;

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