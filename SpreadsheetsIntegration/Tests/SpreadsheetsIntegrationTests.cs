using FluentAssertions;

namespace SpreadsheetsIntegration.Tests
{
    public class SpreadsheetsIntegrationTests
    {
        private const string PathOfResultCsv = "Result.csv";
        private const string PathOfSourceSpreadsheet = "./Files/ListOfTasksCompletionDates.csv";

        [Test]
        public void CreatesResultInCsv()
        {
            MonteCarloSimulation(
                fromSpreadsheetPath: PathOfSourceSpreadsheet,
                toSpreadsheetPath: PathOfResultCsv);

            File.Exists(PathOfResultCsv).Should().BeTrue();
        }

        [Test]
        public void ResultContainsSomething()
        {
            MonteCarloSimulation(
                fromSpreadsheetPath: PathOfSourceSpreadsheet, 
                toSpreadsheetPath: PathOfResultCsv);

            File.ReadAllLines(PathOfResultCsv).Should().HaveCount(3);
        }

        [Test]
        public void ThrowsIfNoSourceIsFound()
        {
            var sutInvocation = () =>
                MonteCarloSimulation(
                    fromSpreadsheetPath: "Path/WhereThereIsNo/Spreadsheet",
                    toSpreadsheetPath: PathOfResultCsv);

            sutInvocation.Should().Throw<FileNotFoundException>();
        }

        [Test]
        public void DeletesPreviousResult()
        {
            File.Create(PathOfResultCsv).Dispose();
            File.Exists(PathOfResultCsv).Should().BeTrue();

            MonteCarloSimulation(
                fromSpreadsheetPath: PathOfSourceSpreadsheet,
                toSpreadsheetPath: PathOfResultCsv);
            File.Exists(PathOfResultCsv).Should().BeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(PathOfResultCsv))
                File.Delete(PathOfResultCsv);
        }

        private static void MonteCarloSimulation(
            string fromSpreadsheetPath,
            string toSpreadsheetPath)
        {
            SpreadsheetsIntegration.MonteCarloSimulation
                .Simulate(
                    fromSpreadsheetPath: fromSpreadsheetPath,
                    toSpreadsheetPath: toSpreadsheetPath);
        }
    }
}