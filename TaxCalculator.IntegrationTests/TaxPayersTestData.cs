using System.Collections;
using NUnit.Framework;

namespace TaxCalculator.IntegrationTests
{
    public static class TaxPayersTestData
    {
        private static string George = @"
{
    ""fullName"": ""George Georgiev"",
    ""dateOfBirth"": ""2022-03-09T17:45:58.975Z"",
    ""grossIncome"": 980,
    ""ssn"": ""00001"",
    ""charitySpent"": 0
}";
        
        private static string Irina = @"
{
    ""fullName"": ""Irina Georgieva"",
    ""dateOfBirth"": ""2022-03-09T17:45:58.975Z"",
    ""grossIncome"": 3400,
    ""ssn"": ""00002"",
    ""charitySpent"": 0
}";

        private static string Mick = @"
{
    ""fullName"": ""Mick Georgiev"",
    ""dateOfBirth"": ""2022-03-09T17:45:58.975Z"",
    ""grossIncome"": 2500,
    ""ssn"": ""00003"",
    ""charitySpent"": 150
}";

        private static string Bill = @"
{
    ""fullName"": ""Bill Georgiev"",
    ""dateOfBirth"": ""2022-03-09T17:45:58.975Z"",
    ""grossIncome"": 3600,
    ""ssn"": ""00004"",
    ""charitySpent"": 520
}";
    
        public static IEnumerable Examples()
        {
            yield return new TestCaseData(George, 980m);
            yield return new TestCaseData(Irina, 2860m);
            yield return new TestCaseData(Mick, 2162.5m);
            yield return new TestCaseData(Bill, 3076m);
        }
    }
}
