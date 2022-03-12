namespace TaxCalculatorNET6.Domain.TaxCalculations.Interfaces;

public interface ITaxCalculationStateFactory
{
    ITaxCalculationState GetNewState();
}