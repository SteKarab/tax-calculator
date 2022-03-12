namespace TaxCalculatorNET6.Domain.TaxCalculations.Interfaces;

public interface ITaxCalculationState
{
    decimal CharityWriteOff { get; set; } 
    
    decimal TaxableIncome { get; set; }
    
    decimal IncomeTax { get; set; }
    
    decimal SocialTaxableIncome { get; set; }
    
    decimal SocialTax { get; set; }

    decimal TotalTax => IncomeTax + SocialTax;
    
    bool IsComplete { get; }

    void Complete();
}