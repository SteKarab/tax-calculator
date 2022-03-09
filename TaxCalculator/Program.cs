using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TaxCalculator.Data;
using TaxCalculator.Services;
using TaxCalculator.Services.Interfaces;
using TaxCalculator.StartupExtensions;
using TaxCalculator.Validation;

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
builder.Services.AddScoped<ITaxCalculator, TaxCalculatorService>();
builder.Services.AddScoped<ITaxParamsService, TaxParamsService>();

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