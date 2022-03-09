using Microsoft.EntityFrameworkCore;
using TaxCalculator.Models;

namespace TaxCalculator.Data;

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
    }
}