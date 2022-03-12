using TaxCalculatorNET6.Domain.TaxCalculations;
using TaxCalculatorNET6.Domain.TaxCalculations.Interfaces;

namespace TaxCalculatorNET6.Domain.DependencyInjection;

public class FluentTaxCalculatorBuilder: IFluentTaxCalculatorBuilder
{
    private IServiceCollection _serviceCollection;
    private int _rankToGive;

    public FluentTaxCalculatorBuilder(IServiceCollection services)
    {
        _serviceCollection = services;
        _rankToGive = 1;
    }
    
    public IFluentTaxCalculatorBuilder WithCharityWriteOff()
    {
        _serviceCollection.AddScoped<ITaxCalculation>(_ => new CharityWriteOffTaxCalculation(_rankToGive));
        _rankToGive++;
        return this;
    }

    public IFluentTaxCalculatorBuilder WithTaxCheck()
    {
        _serviceCollection.AddScoped<ITaxCalculation>(_ => new TaxCheckCalculation(_rankToGive));
        _rankToGive++;
        return this;
    }

    public IFluentTaxCalculatorBuilder WithIncomeTax()
    {
        _serviceCollection.AddScoped<ITaxCalculation>(_ => new IncomeTaxCalculation(_rankToGive));
        _rankToGive++;
        return this;
    }

    public IFluentTaxCalculatorBuilder WithSocialTax()
    {
        _serviceCollection.AddScoped<ITaxCalculation>(_ => new SocialTaxCalculation(_rankToGive));
        _rankToGive++;
        return this;
    }
}