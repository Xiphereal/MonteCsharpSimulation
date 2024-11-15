using FluentAssertions;
using FluentAssertions.Extensions;
using MonteCsharpSimulation.Stragegies;

namespace MonteCsharpSimulation.Tests
{
    public class SimulationTests
    {
        private static readonly DateTime today = 1.February(2014);
        private static readonly DateTime yesterday = today.Subtract(1.Days());
        private static readonly DateTime tomorrow = today.AddDays(1);

        // Given the throughput per date and 1 task, a random day throughput is chosen as completion.
        // Given the throughput per date and 2 tasks, their completion is the sum of any two random days throughput.
        // A number of iterations can be specified. There are several possible completion dates.

        [Test]
        public void NoCompletedTasks_PredictsNoCompletionDate()
        {
            Simulation
                .From(new Period(
                    From: today,
                    To: today,
                    TasksCompletionDates: []))
                .For(numberOfTasks: 1, new InSameOrder())
                .Should().BeEmpty();
        }

        [Test]
        public void PeriodOfSingleDay_WithATaskCompleted_PredictsTodayAsCompletionDate()
        {
            Simulation
                .From(new Period(
                    From: yesterday,
                    To: yesterday,
                    TasksCompletionDates: [yesterday]))
                .For(numberOfTasks: 1, new InSameOrder())
                .Should().BeEquivalentTo(
                [
                    new Completion(When: today, Occurrences: 1)
                ]);
        }

        [Test]
        public void ThroughputFromPeriodIsUsed_ForForecastingTheCompletion()
        {
            Simulation
                .From(new Period(
                    From: yesterday,
                    To: today,
                    TasksCompletionDates: [today]))
                .For(numberOfTasks: 1, throughputSelectionStrategy: new InSameOrder())
                .Should().BeEquivalentTo(
                [
                    new Completion(When: tomorrow, Occurrences: 1)
                ]);
        }
    }
}