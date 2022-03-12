using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Models
{
    public class TaxParams
    {
        [Required]
        [MaxLength(100)]
        public string Country { get; set; }
        [Required]
        [MaxLength(20)]
        public string Version { get; set; }
        public decimal IncomeTaxPercentage { get; set; }
        public decimal IncomeTaxFloor { get; set; }
        public decimal SocialTaxPercentage { get; set; }
        public decimal SocialTaxFloor { get; set; }
        public decimal SocialTaxCeiling { get; set; }
        public decimal AllowedCharityPercentage { get; set; }
    }
}

