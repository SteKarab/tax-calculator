namespace TaxCalculator.Domain.TaxCalculations.Interfaces
{
    public interface ITaxCalculationStateFactory
    {
        ITaxCalculationState GetNewState();
    }
}

