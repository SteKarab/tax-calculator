﻿namespace TaxCalculator.Dtos
{
    public class TaxesDto
    {
        public string SSN { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal CharitySpent { get; set; } 
        public decimal IncomeTax { get; set; }
        public decimal SocialTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal NetIncome { get; set; }

    }
}
