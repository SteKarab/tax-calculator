using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TaxCalculator.Models;

public class Taxes
{
    [MinLength(5)]
    [MaxLength(10)]
    [Key]
    public string SSN { get; set; }
    
    [Precision(30, 2)]
    public decimal GrossIncome { get; set; }
    
    [Precision(30, 2)]
    public decimal CharitySpent { get; set; }
    
    [Precision(30, 2)]
    public decimal IncomeTax { get; set; }
    
    [Precision(30, 2)]
    public decimal SocialTax { get; set; }
    
    [Precision(30, 2)]
    public decimal TotalTax { get; set; }
    
    [Precision(30, 2)]
    public decimal NetIncome { get; set; }
}