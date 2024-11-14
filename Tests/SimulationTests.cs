using FluentAssertions;
using FluentAssertions.Extensions;

namespace MonteCsharpSimulation.Tests
{
    public class SimulationTests
    {
        private readonly DateTime today = 1.February(2014);

        // Given the throughput per date and 1 task, a random day throughput is chosen as completion.
        // Given the throughput per date and 2 tasks, their completion is the sum of any two random days throughput.
        // A number of iterations can be specified. There are several possible completion dates.

        [Test]
        public void NoCompletedTasks_PredictsNoCompletionDate()
        {
            Simulation
                .From(new Period(
                    From: today.Subtract(2.Days()),
                    To: today.Subtract(1.Days()),
                    TasksCompletionDates: []))
                .For(numberOfTasks: 1)
                .Should().BeEmpty();
        }

        [Test]
        public void PeriodOfSingleDay_WithATaskCompleted_PredictsTodayAsCompletionDate()
        {
            Simulation
                .From(new Period(
                    From: today.Subtract(1.Days()),
                    To: today.Subtract(1.Days()),
                    TasksCompletionDates: [today.Subtract(1.Days())]))
                .For(numberOfTasks: 1)
                .Should().BeEquivalentTo(
                [
                    new Completion(When: today, Occurrences: 1)
                ]);
        }

        [Test]
        public void MyTestMethod()
        {
            Simulation
                .From(new Period(
                    From: today.Subtract(2.Days()),
                    To: today.Subtract(1.Days()),
                    TasksCompletionDates: [today.Subtract(1.Days())]))
                .For(numberOfTasks: 1)
                .Should().BeEquivalentTo(
                [
                    new Completion(When: today, Occurrences: 1)
                ]);
        }
    }
}
