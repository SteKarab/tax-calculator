using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Models
{
    public class Taxes
    {
        [MinLength(5)]
        [MaxLength(10)]
        [Key]
        public string SSN { get; set; }
    
        public decimal GrossIncome { get; set; }
    
        public decimal CharitySpent { get; set; }
    
        public decimal IncomeTax { get; set; }
    
        public decimal SocialTax { get; set; }
    
        public decimal TotalTax { get; set; }
    
        public decimal NetIncome { get; set; }
    }
}
