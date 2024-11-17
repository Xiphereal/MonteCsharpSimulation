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

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(200)]
        public void ResultContainsHeadersPlusForecastedCompletions(int runs)
        {
            MonteCarloSimulation(
                fromSpreadsheetPath: PathOfSourceSpreadsheet,
                toSpreadsheetPath: PathOfResultCsv,
                runs: runs);

            File.ReadAllLines(PathOfResultCsv)
                .Should().HaveCountGreaterThan(
                    1,
                    because: "the headers row is always present");
            
            var headersRow = File.ReadAllLines(PathOfResultCsv).First();
            headersRow.Should().ContainAll("When", "Occurrences");
        }

        [Test]
        public void ResultIsForecastedFromSourceSpreadsheet()
        {
            MonteCarloSimulation(
                fromSpreadsheetPath: PathOfSourceSpreadsheet,
                toSpreadsheetPath: PathOfResultCsv);

            File.ReadLines(PathOfResultCsv)
                .Should().BeEquivalentTo(
                [
                    "When,Occurrences",
                    "11/18/2014 00:00:00,1"
                ]);
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

        private static void MonteCarloSimulation(string fromSpreadsheetPath,
            string toSpreadsheetPath,
            int runs = 1)
        {
            SpreadsheetsIntegration.MonteCarloSimulation
                .Simulate(
                    fromSpreadsheetPath: fromSpreadsheetPath,
                    toSpreadsheetPath: toSpreadsheetPath,
                    runs);
        }
    }
}