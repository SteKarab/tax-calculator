using TaxCalculator.Domain.TaxCalculations.Interfaces;
using TaxCalculator.Dtos;
using TaxCalculator.Models;

namespace TaxCalculator.Domain.TaxCalculations
{
    public abstract class TaxCalculationBase : ITaxCalculation
    {
        public int Rank { get; }
    
        public string Name { get; protected set; }
    
        public string Description { get; protected set; }

        public TaxCalculationBase(int rank)
        {
            Rank = rank;
        }
    
        public abstract void Execute(ITaxCalculationState state, TaxPayerDto taxPayerDto, TaxParams taxParams);
    }
}
