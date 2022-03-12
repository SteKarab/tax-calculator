using System;
using TaxCalculator.Domain.TaxCalculations.Interfaces;
using TaxCalculator.Dtos;
using TaxCalculator.Models;

namespace TaxCalculator.Domain.TaxCalculations
{
    public class IncomeTaxCalculation: TaxCalculationBase
    {
        public IncomeTaxCalculation(int rank) : base(rank)
        {
            Name = "Income Tax calculation";
            Description = "Standard income tax calculation, based on a percentage and a tax floor";
        }
    
        public override void Execute(ITaxCalculationState state, TaxPayerDto taxPayerDto, TaxParams taxParams)
        {
            state.TaxableIncome -= taxParams.IncomeTaxFloor;
            state.IncomeTax = Math.Round(state.TaxableIncome * taxParams.IncomeTaxPercentage / 100, 2);
        }
    }
}
