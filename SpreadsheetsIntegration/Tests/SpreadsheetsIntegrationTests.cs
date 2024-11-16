using FluentAssertions;

namespace SpreadsheetsIntegration.Tests
{
    public class SpreadsheetsIntegrationTests
    {
        [Test]
        public void CreatesResultInCSV()
        {
            MonteCarloSimulation
                .Simulate(
                    fromSpreadsheetPath: "./Files/ListOfTasksCompletionDates.csv",
                    toSpreadsheetPath: "Result.csv");

            File.Exists("Result.csv").Should().BeTrue();
        }
    }
}