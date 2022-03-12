using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TaxCalculator.Data;
using TaxCalculator.Models;

namespace TaxCalculator.StartupExtensions
{
    public static class SeedExtensions
    {
        public static IApplicationBuilder SeedTaxParams(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            
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
                
            var configContext = serviceScope.ServiceProvider.GetService<TaxPayerContext>();

            Console.WriteLine($"Adding default tax params: Country: {newTaxParams.Country}, Version: {newTaxParams.Version}");
                
            var taxParams = configContext.TaxParams.Find(newTaxParams.Country, newTaxParams.Version);

            if (taxParams != null) configContext.TaxParams.Remove(taxParams);

            configContext.SaveChanges();

            configContext.Add(newTaxParams);

            configContext.SaveChanges();

            return app;
        }
    }
}

