using TaxCalculatorNET6.Dtos;
using TaxCalculatorNET6.Models;

namespace TaxCalculatorNET6.Domain.TaxCalculations.Interfaces;

public interface ITaxCalculation
{
    int Rank { get; }
    
    string Name { get; }
    
    string Description { get; }
    
    void Execute(ITaxCalculationState state, TaxPayerDto taxPayerDto, TaxParams taxParams);
}