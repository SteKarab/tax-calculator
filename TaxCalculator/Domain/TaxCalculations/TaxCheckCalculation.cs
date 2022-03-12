using TaxCalculator.Domain.TaxCalculations.Interfaces;
using TaxCalculator.Dtos;
using TaxCalculator.Models;

namespace TaxCalculator.Domain.TaxCalculations
{
    public class TaxCheckCalculation: TaxCalculationBase
    {
        public TaxCheckCalculation(int rank) : base(rank)
        {
            Name = "Tax Check calculation";
            Description = "Standard tax check calculation, based on a tax floor, designed to relieve low-income earners.";
        }
    
        public override void Execute(ITaxCalculationState state, TaxPayerDto taxPayerDto, TaxParams taxParams)
        {
            if (state.TaxableIncome <= taxParams.IncomeTaxFloor) state.Complete();
        }
    }
}
