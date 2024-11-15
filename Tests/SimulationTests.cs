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
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today)
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
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today)
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
                    TasksCompletionDates: [yesterday]))
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today)
                .Should().BeEquivalentTo(
                [
                    new Completion(When: today, Occurrences: 1)
                ]);

            Simulation
                .From(new Period(
                    From: yesterday,
                    To: today,
                    TasksCompletionDates: [today]))
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today)
                .Should().BeEquivalentTo(
                [
                    new Completion(When: tomorrow, Occurrences: 1)
                ]);
        }

        [Test]
        public void ForecastCanStartInAnyDate()
        {
            Simulation
                .From(new Period(
                    From: today,
                    To: today,
                    TasksCompletionDates: [today]))
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today.AddDays(40))
                .Should().BeEquivalentTo(
                [
                    new Completion(When: today.AddDays(40), Occurrences: 1)
                ]);
        }

        [Test]
        public void SeveralTasksCompletionCanBeForecasted()
        {
            Simulation
                .From(new Period(
                    From: yesterday,
                    To: today,
                    TasksCompletionDates: [yesterday, today]))
                .For(
                    numberOfTasks: 2,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today)
                .Should().BeEquivalentTo(
                [
                    new Completion(When: tomorrow, Occurrences: 1)
                ]);
        }

        [Test]
        public void ForecastTakesLongerThanPeriod_ThroughputIsNotExhausted()
        {
            Simulation
                .From(new Period(
                    From: today,
                    To: today,
                    TasksCompletionDates: [today]))
                .For(
                    numberOfTasks: 2,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today)
                .Should().BeEquivalentTo(
                [
                    new Completion(When: tomorrow, Occurrences: 1)
                ]);
        }

        [Test]
        public void asdfasdf()
        {
            Simulation
                .From(new Period(
                    From: yesterday,
                    To: today,
                    TasksCompletionDates: [today]))
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new SeededRandom(seed: 1),
                    dayToStartForecastingFrom: today,
                    runs: 10)
                .Should().BeEquivalentTo(
                [
                    new Completion(When: today.AddDays(3), Occurrences: 10)
                ]);
        }
    }
}