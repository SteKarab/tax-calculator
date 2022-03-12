using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using TaxCalculator.Data;
using TaxCalculator.Domain.TaxCalculations;
using TaxCalculator.Domain.TaxCalculations.Interfaces;
using TaxCalculator.Dtos;
using TaxCalculator.Exceptions;
using TaxCalculator.Services;
using TaxCalculator.Services.Interfaces;
using TaxCalculator.Tests.Helpers;

namespace TaxCalculator.Tests
{
    public class TaxCalculatorServiceTests
    {
        private ITaxParamsService _taxParamsService;
        private ITaxCalculator _taxCalculator;
        private ITaxCalculationStateFactory _taxCalculationStateFactory;
        private IEnumerable<ITaxCalculation> _taxCalculations;
        private TaxPayerContext _context;

        [SetUp]
        public void Setup()
        {
            _taxParamsService = Substitute.For<ITaxParamsService>();
            _taxParamsService
                .GetParamsFor(Arg.Any<string>(), Arg.Any<string>())
                .Returns(TaxParamHelpers.GetDefaultTaxParams());
            
            var taxPayerContextOptions = new DbContextOptionsBuilder<TaxPayerContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new TaxPayerContext(taxPayerContextOptions);

            _taxCalculationStateFactory = Substitute.For<ITaxCalculationStateFactory>();
            _taxCalculationStateFactory.GetNewState().Returns(new TaxCalculationState());

            _taxCalculations = new List<ITaxCalculation>
            {
                new CharityWriteOffTaxCalculation(1),
                new TaxCheckCalculation(2),
                new IncomeTaxCalculation(3),
                new SocialTaxCalculation(4)
            };
            
            _taxCalculator = new TaxCalculatorService(_context, _taxParamsService, _taxCalculationStateFactory, _taxCalculations);
        }

        [TearDown]
        public void TearDown()
        {
            _context.PayerTaxes.RemoveRange(_context.PayerTaxes);       
        }

        [TestCase(1000)]
        [TestCase(980)] // George Example
        [TestCase(900)]
        [TestCase(500)]
        [TestCase(0)]
        public async Task CalculateTaxes_ReturnsNoTaxesWhenGrossIncomeIs(decimal grossIncome)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"t{grossIncome}",
                GrossIncome = grossIncome,
                CharitySpent = 0
            };

            var result = await _taxCalculator.CalculateTaxes(taxPayerDto);
            
            Assert.AreEqual(0, result.TotalTax);
        }
        
        [TestCase(-1000)]
        [TestCase(-980)] // George Example
        [TestCase(-900)]
        [TestCase(-500)]
        public async Task CalculateTaxes_ThrowsIfNegativeGrossIncomePassed(decimal grossIncome)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"t{grossIncome}",
                GrossIncome = grossIncome,
                CharitySpent = 0
            };

            Assert.ThrowsAsync<InvalidTaxInformationException>(() => _taxCalculator.CalculateTaxes(taxPayerDto));
        }
        
        [TestCase(-1000)]
        [TestCase(-980)] // George Example
        [TestCase(-900)]
        [TestCase(-500)]
        public async Task CalculateTaxes_ThrowsIfNegativeCharitySpentPassed(decimal charitySpent)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"t{charitySpent}",
                GrossIncome = 0,
                CharitySpent = charitySpent
            };

            Assert.ThrowsAsync<InvalidTaxInformationException>(() => _taxCalculator.CalculateTaxes(taxPayerDto));
        }
        
        [TestCase(1000)]
        [TestCase(980)] // George Example
        [TestCase(900)]
        [TestCase(500)]
        [TestCase(0)]
        public async Task CalculateTaxes_Throws_IfCharitySpentNotPassed(decimal grossIncome)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"t{grossIncome}",
                GrossIncome = grossIncome,
            };

            Assert.ThrowsAsync<InvalidTaxInformationException>(() => _taxCalculator.CalculateTaxes(taxPayerDto));
        }
        
        [TestCase(1000)]
        [TestCase(980)] // George Example
        [TestCase(900)]
        [TestCase(500)]
        [TestCase(0)]
        public async Task CalculateTaxes_Throws_IfGrossIncomeNotPassed(decimal grossIncome)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"t{grossIncome}",
                CharitySpent = 0,
            };

            Assert.ThrowsAsync<InvalidTaxInformationException>(() => _taxCalculator.CalculateTaxes(taxPayerDto));
        }

        [TestCase(0.1, 1001)]
        [TestCase(50, 1500)]
        [TestCase(100, 2000)]
        [TestCase(150, 2500)]
        [TestCase(200, 3000)]
        [TestCase(240, 3400)] // Irina Example
        [TestCase(250, 3500)]
        [TestCase(400, 5000)]
        [TestCase(900, 10000)]
        [TestCase(9900, 100000)]
        [TestCase(99900, 1000000)]
        public async Task CalculateTaxes_ReturnsProperIncomeTax_WhenGrossIncomeIs(decimal expectedIncomeTax, decimal grossIncome)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"g{expectedIncomeTax}",
                GrossIncome = grossIncome,
                CharitySpent = 0
            };

            var result = await _taxCalculator.CalculateTaxes(taxPayerDto);
            
            Assert.AreEqual(expectedIncomeTax, result.IncomeTax);
        }
        
        [TestCase(0, 50, 1001)]
        [TestCase(40, 100, 1500)]
        [TestCase(80, 200, 2000)]
        [TestCase(125, 500, 2500)]
        [TestCase(135, 150, 2500)] // Mick Example
        [TestCase(170, 800, 3000)]
        [TestCase(215, 1000, 3500)]
        [TestCase(224, 520, 3600)] // Bill Example
        [TestCase(350, 1500, 5000)]
        [TestCase(800, 2500, 10000)]
        [TestCase(9400, 5000, 100000)]
        [TestCase(96900, 30000, 1000000)]
        public async Task CalculateTaxes_ReturnsProperIncomeTax_WhenCharityAndGrossIncomeAre(decimal expectedIncomeTax, decimal charity, decimal grossIncome)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"f{expectedIncomeTax}",
                GrossIncome = grossIncome,
                CharitySpent = charity
            };

            var result = await _taxCalculator.CalculateTaxes(taxPayerDto);
            
            Assert.AreEqual(expectedIncomeTax, result.IncomeTax);
        }
        
        [TestCase(0.15, 1001)]
        [TestCase(75, 1500)]
        [TestCase(150, 2000)]
        [TestCase(225, 2500)]
        [TestCase(300, 3000)]
        [TestCase(300, 3400)] // Irina Example
        [TestCase(300, 3500)]
        [TestCase(300, 5000)]
        [TestCase(300, 10000)]
        [TestCase(300, 100000)]
        [TestCase(300, 1000000)]
        public async Task CalculateTaxes_ReturnsProperSocialTax_WhenGrossIncomeIs(decimal expectedSocialTax, decimal grossIncome)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"a{grossIncome}",
                GrossIncome = grossIncome,
                CharitySpent = 0
            };

            var result = await _taxCalculator.CalculateTaxes(taxPayerDto);
            
            Assert.AreEqual(expectedSocialTax, result.SocialTax);
        }
        
        [TestCase(0, 50, 1001)]
        [TestCase(60, 100, 1500)]
        [TestCase(120, 200, 2000)]
        [TestCase(187.5, 500, 2500)]
        [TestCase(202.5, 150, 2500)] // Mick Example
        [TestCase(255, 800, 3000)]
        [TestCase(300, 1000, 3500)]
        [TestCase(300, 520, 3600)] // Bill Example
        [TestCase(300, 1500, 5000)]
        [TestCase(300, 2500, 10000)]
        [TestCase(300, 5000, 100000)]
        [TestCase(300, 30000, 1000000)]
        public async Task CalculateTaxes_ReturnsProperSocialTax_WhenCharityAndGrossIncomeAre(decimal expectedSocialTax, decimal charity, decimal grossIncome)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"s{charity}",
                GrossIncome = grossIncome,
                CharitySpent = charity
            };

            var result = await _taxCalculator.CalculateTaxes(taxPayerDto);
            
            Assert.AreEqual(expectedSocialTax, result.SocialTax);
        }
        
        [TestCase(0, 50, 1001)]
        [TestCase(100, 100, 1500)]
        [TestCase(200, 200, 2000)]
        [TestCase(312.5, 500, 2500)]
        [TestCase(337.5, 150, 2500)] // Mick Example
        [TestCase(425, 800, 3000)]
        [TestCase(540, 0, 3400)] // Irina Example
        [TestCase(515, 1000, 3500)]
        [TestCase(524, 520, 3600)] // Bill Example
        [TestCase(650, 1500, 5000)]
        [TestCase(1100, 2500, 10000)]
        [TestCase(9700, 5000, 100000)]
        [TestCase(97200, 30000, 1000000)]
        public async Task CalculateTaxes_ReturnsProperTotalTax_WhenCharityAndGrossIncomeAre(decimal expectedTotalTax, decimal charity, decimal grossIncome)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"h{expectedTotalTax}",
                GrossIncome = grossIncome,
                CharitySpent = charity
            };

            var result = await _taxCalculator.CalculateTaxes(taxPayerDto);
            
            Assert.AreEqual(expectedTotalTax, result.TotalTax);
        }
        
        [TestCase(1001, 50, 1001)]
        [TestCase(1400, 100, 1500)]
        [TestCase(1800, 200, 2000)]
        [TestCase(2187.5, 500, 2500)]
        [TestCase(2162.5, 150, 2500)] // Mick Example
        [TestCase(2575, 800, 3000)]
        [TestCase(2860, 0, 3400)] // Irina Example
        [TestCase(2985, 1000, 3500)]
        [TestCase(3076, 520, 3600)] // Bill Example
        [TestCase(4350, 1500, 5000)]
        [TestCase(8900, 2500, 10000)]
        [TestCase(90300, 5000, 100000)]
        [TestCase(902800, 30000, 1000000)]
        public async Task CalculateTaxes_ReturnsProperNetIncome_WhenCharityAndGrossIncomeAre(decimal expectedNetIncome, decimal charity, decimal grossIncome)
        {
            var taxPayerDto = new TaxPayerDto
            {
                SSN = $"j{expectedNetIncome}",
                GrossIncome = grossIncome,
                CharitySpent = charity
            };

            var result = await _taxCalculator.CalculateTaxes(taxPayerDto);
            
            Assert.AreEqual(expectedNetIncome, result.NetIncome);
        }
    }
}

