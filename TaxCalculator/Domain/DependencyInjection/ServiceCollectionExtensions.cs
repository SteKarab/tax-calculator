using Microsoft.Extensions.DependencyInjection;
using TaxCalculator.Domain.TaxCalculations;
using TaxCalculator.Domain.TaxCalculations.Interfaces;
using TaxCalculator.Services;
using TaxCalculator.Services.Interfaces;

namespace TaxCalculator.Domain.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IFluentTaxCalculatorBuilder AddTaxCalculator(this IServiceCollection services)
        {
            services.AddScoped<ITaxCalculationStateFactory, TaxCalculationStateFactory>();
            services.AddScoped<ITaxParamsService, TaxParamsService>();
            services.AddScoped<ITaxCalculator, TaxCalculatorService>();

            return new FluentTaxCalculatorBuilder(services);
        }
    }
}
