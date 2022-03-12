using System;
using TaxCalculator.Domain.TaxCalculations.Interfaces;
using TaxCalculator.Dtos;
using TaxCalculator.Models;

namespace TaxCalculator.Domain.TaxCalculations
{
    public class SocialTaxCalculation: TaxCalculationBase
    {
        public SocialTaxCalculation(int rank) : base(rank)
        {
            Name = "Social Tax calculation";
            Description = "Standard social tax calculation, based on a percentage, and a tax floor and ceiling";
        }
    
        public override void Execute(ITaxCalculationState state, TaxPayerDto taxPayerDto, TaxParams taxParams)
        {
            state.SocialTaxableIncome -= taxParams.SocialTaxFloor;
            var defaultSocialRange = taxParams.SocialTaxCeiling - taxParams.SocialTaxFloor;
            var taxableSocialAmount = Math.Min(defaultSocialRange, state.SocialTaxableIncome);
            state.SocialTax = Math.Round(taxableSocialAmount * taxParams.SocialTaxPercentage / 100, 2);
        }
    }
}
