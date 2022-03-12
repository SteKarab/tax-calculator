using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using DotNet.Testcontainers.Containers.WaitStrategies;
using Newtonsoft.Json;
using NUnit.Framework;
using TaxCalculator.Dtos;

namespace TaxCalculator.IntegrationTests
{
    public class TaxEndpointTests
    {
        private readonly string _baseAddress = "http://localhost:5100"; 
    
        private HttpClient _httpClient;
    

        [OneTimeSetUp]
        public async Task Init()
        {
            _httpClient = new HttpClient(new HttpClientHandler());
            _httpClient.BaseAddress = new Uri(_baseAddress);

            var taxCalculatorContainerBuilder = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("tax_calculator")
                .WithPortBinding(5100,5100)
                .WithExposedPort(5100)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5100))
                .WithCleanUp(true);

            var taxCalculatorContainer = taxCalculatorContainerBuilder.Build();

            await taxCalculatorContainer.StartAsync();
        }

        [TestCaseSource(typeof(TaxPayersTestData), nameof(TaxPayersTestData.Examples))]
        public async Task ExampleTests(string json, decimal expectedNetIncome)
        {
            var result = await _httpClient.PostAsync("/tax", new StringContent(json, Encoding.UTF8,"application/json"));

            var content = await result.Content.ReadAsStringAsync();

            var parsed = JsonConvert.DeserializeObject<TaxesDto>(content);

            Assert.AreEqual(expectedNetIncome, parsed.NetIncome);
        }

        private bool HealthCheck()
        {
            var result = Task.Run(() => _httpClient.GetAsync("/health"));

            return result.Result.IsSuccessStatusCode;
        }
    }
}

