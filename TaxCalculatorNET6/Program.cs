using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TaxCalculatorNET6.Data;
using TaxCalculatorNET6.Domain.DependencyInjection;
using TaxCalculatorNET6.StartupExtensions;
using TaxCalculatorNET6.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<TaxPayerDtoValidator>(ServiceLifetime.Singleton);
builder.Services.AddFluentValidation();
builder.Services.AddDbContext<TaxPayerContext>(options =>
{
    options.UseInMemoryDatabase("InMemoryDb");
});

builder.Services.AddTaxCalculator()
    .WithCharityWriteOff()
    .WithTaxCheck()
    .WithIncomeTax()
    .WithSocialTax();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.SeedTaxParams();

app.UseAuthorization();

app.MapControllers();

app.Run();