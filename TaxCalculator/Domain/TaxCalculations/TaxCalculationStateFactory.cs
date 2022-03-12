using TaxCalculator.Domain.TaxCalculations.Interfaces;

namespace TaxCalculator.Domain.TaxCalculations
{
    public class TaxCalculationStateFactory: ITaxCalculationStateFactory
    {
        public ITaxCalculationState GetNewState()
        {
            return new TaxCalculationState();
        }
    }
}
