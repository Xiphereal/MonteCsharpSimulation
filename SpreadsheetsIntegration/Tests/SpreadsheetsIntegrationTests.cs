using Domain.Strategies;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace SpreadsheetsIntegration.Tests
{
    public class SpreadsheetsIntegrationTests
    {
        private const string PathOfResultCsv = "Result.csv";

        private const string PathOfSourceSpreadsheet =
            "./Files/ListOfTasksCompletionDates.csv";

        private const string PathOfSourceSpreadsheetWithNonDeliveredYetTasks =
            "./Files/ListOfTasksWithSomeNotDeliveredYet.csv";

        private const string PathOfSourceSpreadsheetWithTasksWithoutTime =
            "./Files/SomeTasksDeliveredHaveNoTime.csv";

        [Test]
        public void Acceptance()
        {
            MonteCarloSimulation(
                fromSpreadsheetPath: PathOfSourceSpreadsheet,
                toSpreadsheetPath: PathOfResultCsv,
                from: 14.November(2024),
                to: 19.November(2024),
                dayToStartForecastingFrom: 3.January(year: 2025),
                runs: 10);

            File.ReadLines(PathOfResultCsv)
                .Should().BeEquivalentTo(
                    "When;Occurrences",
                    "01/03/2025 00:00:00;5",
                    "01/05/2025 00:00:00;1",
                    "01/07/2025 00:00:00;4");
        }

        [Test]
        public void CreatesResultInCsv()
        {
            MonteCarloSimulation(
                fromSpreadsheetPath: PathOfSourceSpreadsheet,
                toSpreadsheetPath: PathOfResultCsv);

            File.Exists(PathOfResultCsv).Should().BeTrue();
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
                    "When;Occurrences",
                    "11/18/2014 00:00:00;1");
        }

        [Test]
        public void TasksNotDeliveredYet_AreReadButNotTakenIntoAccount()
        {
            MonteCarloSimulation(
                fromSpreadsheetPath: PathOfSourceSpreadsheetWithNonDeliveredYetTasks,
                toSpreadsheetPath: PathOfResultCsv);

            File.ReadLines(PathOfResultCsv).Should().NotBeEmpty();
        }

        [Test]
        public void TasksWhoseDeliveredHaveNoTime_CanBeRead()
        {
            MonteCarloSimulation(
                fromSpreadsheetPath: PathOfSourceSpreadsheetWithTasksWithoutTime,
                toSpreadsheetPath: PathOfResultCsv);

            File.ReadLines(PathOfResultCsv).Should().NotBeEmpty();
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


        [TearDown]
        public void TearDown()
        {
            if (File.Exists(PathOfResultCsv))
                File.Delete(PathOfResultCsv);
        }

        private static void MonteCarloSimulation(
            string fromSpreadsheetPath,
            string toSpreadsheetPath,
            DateTime? from = null,
            DateTime? to = null,
            DateTime? dayToStartForecastingFrom = null,
            int runs = 1)
        {
            SpreadsheetsIntegration.MonteCarloSimulation
                .Simulate(
                    fromSpreadsheetPath: fromSpreadsheetPath,
                    toSpreadsheetPath: toSpreadsheetPath,
                    from: from ?? 15.November(2024),
                    to: to ?? 18.November(2024),
                    throughputSelectionStrategy: new InSameOrder(),
                    runs,
                    dayToStartForecastingFrom ?? 17.November(year: 2014));
        }
    }
}