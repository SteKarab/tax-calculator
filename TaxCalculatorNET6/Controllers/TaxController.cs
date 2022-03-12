using Microsoft.AspNetCore.Mvc;
using TaxCalculatorNET6.Dtos;
using TaxCalculatorNET6.Exceptions;
using TaxCalculatorNET6.Services.Interfaces;

namespace TaxCalculatorNET6.Controllers;

[ApiController]
[Route("tax")]
public class TaxController : ControllerBase
{
    private ILogger<TaxController> _logger;
    private ITaxCalculator _taxCalculator;
    
    public TaxController(ILogger<TaxController> logger, ITaxCalculator taxCalculator)
    {
        _logger = logger;
        _taxCalculator = taxCalculator;
    }

    [HttpPost(Name = "calculate")]
    public async Task<IActionResult> Get([FromBody] TaxPayerDto taxPayer)
    {
        TaxesDto result;

        try
        {
            result = await _taxCalculator.CalculateTaxes(taxPayer);
        }
        catch (TaxParamsNotFoundException)
        {
            ModelState.AddModelError("TaxParams", "The proper tax params could not be found!");
            return BadRequest(ModelState);
        }
        catch (InvalidTaxInformationException)
        {
            ModelState.AddModelError("TaxInformation", "Invalid tax information has been supplied and has somehow passed validation. You can contact our development team if issue persists.");
            return BadRequest(ModelState);
        }

        return Ok(result);
    }
}