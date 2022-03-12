namespace TaxCalculator.Domain.DependencyInjection
{
    public interface IFluentTaxCalculatorBuilder
    {
        IFluentTaxCalculatorBuilder WithCharityWriteOff();

        IFluentTaxCalculatorBuilder WithTaxCheck();

        IFluentTaxCalculatorBuilder WithIncomeTax();
    
        IFluentTaxCalculatorBuilder WithSocialTax();
    }
}
