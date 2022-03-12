using System;
using TaxCalculator.Domain.TaxCalculations.Interfaces;
using TaxCalculator.Dtos;
using TaxCalculator.Models;

namespace TaxCalculator.Domain.TaxCalculations
{
    public class CharityWriteOffTaxCalculation: TaxCalculationBase
    {
        public CharityWriteOffTaxCalculation(int rank) : base(rank)
        {
            Name = "Charity Write-off Calculation";
            Description = "Pre-income tax and pre-social tax calculation to provide tax relief to tax-payers.";
        }
    
        public override void Execute(ITaxCalculationState state, TaxPayerDto taxPayerDto, TaxParams taxParams)
        {
            state.CharityWriteOff = Math.Min(Math.Round(taxPayerDto.GrossIncome.Value * taxParams.AllowedCharityPercentage / 100), taxPayerDto.CharitySpent.Value);

            state.TaxableIncome = taxPayerDto.GrossIncome.Value - state.CharityWriteOff;
            state.SocialTaxableIncome = state.TaxableIncome;
        }
    }
}
