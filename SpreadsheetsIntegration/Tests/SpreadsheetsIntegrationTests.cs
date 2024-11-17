using FluentAssertions;

namespace SpreadsheetsIntegration.Tests
{
    public class SpreadsheetsIntegrationTests
    {
        private const string PathOfResultCsv = "Result.csv";
        private const string PathOfSourceSpreadsheet = "./Files/ListOfTasksCompletionDates.csv";

        [Test]
        public void CreatesResultInCSV()
        {
            MonteCarloSimulation
                .Simulate(
                    fromSpreadsheetPath: PathOfSourceSpreadsheet,
                    toSpreadsheetPath: PathOfResultCsv);

            File.Exists(PathOfResultCsv).Should().BeTrue();
        }

        [Test]
        public void ThrowsIfNoSourceIsFound()
        {
            var sutInvocation = () =>
                MonteCarloSimulation
                    .Simulate(
                        fromSpreadsheetPath: "Path/WhereThereIsNo/Spreadsheet",
                        toSpreadsheetPath: PathOfResultCsv);

            sutInvocation.Should().Throw<FileNotFoundException>();
        }

        [Test]
        public void ResultContainsSomething()
        {
            MonteCarloSimulation
                .Simulate(
                    fromSpreadsheetPath: PathOfSourceSpreadsheet,
                    toSpreadsheetPath: PathOfResultCsv);

            File.ReadAllText(PathOfResultCsv).Should().NotBeEmpty();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(PathOfResultCsv))
                File.Delete(PathOfResultCsv);
        }
    }
}