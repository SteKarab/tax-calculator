using TaxCalculatorNET6.Domain.TaxCalculations.Interfaces;

namespace TaxCalculatorNET6.Domain.TaxCalculations;

public class TaxCalculationState: ITaxCalculationState
{
    public decimal CharityWriteOff { get; set; } 
    
    public decimal TaxableIncome { get; set; }
    
    public decimal IncomeTax { get; set; }
    
    public decimal SocialTaxableIncome { get; set; }
    
    public decimal SocialTax { get; set; }

    public decimal TotalTax => IncomeTax + SocialTax;
    
    public bool IsComplete { get; private set; }

    public void Complete() => IsComplete = true;
}