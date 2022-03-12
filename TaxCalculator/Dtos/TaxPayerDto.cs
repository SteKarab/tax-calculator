using System;

namespace TaxCalculator.Dtos
{
    public class TaxPayerDto
    {
        public string FullName { get; set; }
    
        public DateTime DateOfBirth { get; set; }
    
        public decimal? GrossIncome { get; set; }
    
        public string SSN { get; set; }
    
        public decimal? CharitySpent { get; set; }
    
        public string? Country { get; set; }
    
        public string? Version { get; set; }
    }
}
