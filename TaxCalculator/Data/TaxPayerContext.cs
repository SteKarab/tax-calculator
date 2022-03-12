using Microsoft.EntityFrameworkCore;
using TaxCalculator.Models;

namespace TaxCalculator.Data
{
    public class TaxPayerContext : DbContext
    {
        public DbSet<Taxes> PayerTaxes { get; set; }
        public DbSet<TaxParams> TaxParams { get; set; }

        public TaxPayerContext(DbContextOptions options): base(options)
        {
        }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxParams>()
                .HasKey(x => new {x.Country, x.Version});
            modelBuilder.Entity<TaxParams>().Property(x => x.IncomeTaxPercentage).HasPrecision(30, 2);
            modelBuilder.Entity<TaxParams>().Property(x => x.IncomeTaxFloor).HasPrecision(30, 2);
            modelBuilder.Entity<TaxParams>().Property(x => x.SocialTaxPercentage).HasPrecision(30, 2);
            modelBuilder.Entity<TaxParams>().Property(x => x.SocialTaxFloor).HasPrecision(30, 2);
            modelBuilder.Entity<TaxParams>().Property(x => x.SocialTaxCeiling).HasPrecision(30, 2);
            modelBuilder.Entity<TaxParams>().Property(x => x.AllowedCharityPercentage).HasPrecision(30, 2);
        
            modelBuilder.Entity<Taxes>().Property(x => x.GrossIncome).HasPrecision(30, 2);
            modelBuilder.Entity<Taxes>().Property(x => x.CharitySpent).HasPrecision(30, 2);
            modelBuilder.Entity<Taxes>().Property(x => x.IncomeTax).HasPrecision(30, 2);
            modelBuilder.Entity<Taxes>().Property(x => x.SocialTax).HasPrecision(30, 2);
            modelBuilder.Entity<Taxes>().Property(x => x.TotalTax).HasPrecision(30, 2);
            modelBuilder.Entity<Taxes>().Property(x => x.NetIncome).HasPrecision(30, 2);
        }
    }
}
