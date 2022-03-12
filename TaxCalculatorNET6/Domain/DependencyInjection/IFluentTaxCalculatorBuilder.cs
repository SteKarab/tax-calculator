namespace TaxCalculatorNET6.Domain.DependencyInjection;

public interface IFluentTaxCalculatorBuilder
{
    IFluentTaxCalculatorBuilder WithCharityWriteOff();

    IFluentTaxCalculatorBuilder WithTaxCheck();

    IFluentTaxCalculatorBuilder WithIncomeTax();
    
    IFluentTaxCalculatorBuilder WithSocialTax();
}