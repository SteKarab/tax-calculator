using TaxCalculator.Dtos;
using TaxCalculator.Models;

namespace TaxCalculator.Domain.TaxCalculations.Interfaces
{
    public interface ITaxCalculation
    {
        int Rank { get; }
    
        string Name { get; }
    
        string Description { get; }
    
        void Execute(ITaxCalculationState state, TaxPayerDto taxPayerDto, TaxParams taxParams);
    }
}

