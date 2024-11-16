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
                    new Completion(when: today, occurrences: 1)
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
                    new Completion(when: today, occurrences: 1)
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
                    new Completion(when: tomorrow, occurrences: 1)
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
                    new Completion(when: today.AddDays(40), occurrences: 1)
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
                    new Completion(when: tomorrow, occurrences: 1)
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
                    new Completion(when: tomorrow, occurrences: 1)
                ]);
        }

        [Test]
        public void CompletionDatesMayBeForecasted_MoreThanOnce()
        {
            Simulation
                .From(new Period(
                    From: yesterday,
                    To: today,
                    TasksCompletionDates: [today]))
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today,
                    runs: 2)
                .Should().BeEquivalentTo(
                [
                    new Completion(when: tomorrow, occurrences: 2),
                ]);

            Simulation
                .From(new Period(
                    From: yesterday,
                    To: today,
                    TasksCompletionDates: [today]))
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today,
                    runs: 3)
                .Should().BeEquivalentTo(
                [
                    new Completion(when: tomorrow, occurrences: 3),
                ]);
        }

        [Test]
        public void ForecastMayHaveDifferentCompletionDates()
        {
            Simulation
                .From(new Period(
                    From: yesterday,
                    To: today,
                    TasksCompletionDates: [yesterday]))
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today,
                    runs: 2)
                .Should().BeEquivalentTo(
                [
                    new Completion(when: today, occurrences: 1),
                    new Completion(when: tomorrow, occurrences: 1),
                ]);
        }
    }
}