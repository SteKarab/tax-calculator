using TaxCalculator.Models;

namespace TaxCalculator.Tests.Helpers;

public static class TaxParamHelpers
{
    public static TaxParams GetDefaultTaxParams()
    {
        var newTaxParams = new TaxParams
        {
            Country = "Imagiaria",
            Version = "1",
            AllowedCharityPercentage = 10,
            IncomeTaxFloor = 1000,
            IncomeTaxPercentage = 10,
            SocialTaxCeiling = 3000,
            SocialTaxFloor = 1000,
            SocialTaxPercentage = 15
        };

        return newTaxParams;
    }
}