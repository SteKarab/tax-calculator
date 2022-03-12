using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TaxCalculatorNET6.Models;

public class TaxParams
{
    [Required]
    [MaxLength(100)]
    public string Country { get; set; }
    [Required]
    [MaxLength(20)]
    public string Version { get; set; }
    [Precision(30, 2)]
    public decimal IncomeTaxPercentage { get; set; }
    [Precision(30, 2)]
    public decimal IncomeTaxFloor { get; set; }
    [Precision(30, 2)]
    public decimal SocialTaxPercentage { get; set; }
    [Precision(30, 2)]
    public decimal SocialTaxFloor { get; set; }
    [Precision(30, 2)]
    public decimal SocialTaxCeiling { get; set; }
    [Precision(30, 2)]
    public decimal AllowedCharityPercentage { get; set; }
}