using TaxCalculatorNET6.Domain.TaxCalculations.Interfaces;

namespace TaxCalculatorNET6.Domain.TaxCalculations;

public class TaxCalculationStateFactory: ITaxCalculationStateFactory
{
    public ITaxCalculationState GetNewState()
    {
        return new TaxCalculationState();
    }
}