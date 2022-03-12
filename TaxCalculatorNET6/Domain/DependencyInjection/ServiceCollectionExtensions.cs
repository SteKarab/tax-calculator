
using TaxCalculatorNET6.Domain.TaxCalculations;
using TaxCalculatorNET6.Domain.TaxCalculations.Interfaces;
using TaxCalculatorNET6.Services;
using TaxCalculatorNET6.Services.Interfaces;

namespace TaxCalculatorNET6.Domain.DependencyInjection;

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