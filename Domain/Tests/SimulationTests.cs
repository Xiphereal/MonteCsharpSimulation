using Domain.Stragegies;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace Domain.Tests
{
    public class SimulationTests
    {
        private static readonly DateTime today = 1.February(2014);
        private static readonly DateTime yesterday = today.Subtract(1.Days());
        private static readonly DateTime tomorrow = today.AddDays(1);

        [Test]
        public void Acceptance()
        {
            Simulation
                .From(new Period(
                    from: 1.January(2010),
                    to: 14.January(2010),
                    tasksCompletionDates:
                    [
                        1.January(2010),
                        5.January(2010),
                        5.January(2010),
                        8.January(2010),
                        12.January(2010),
                        12.January(2010),
                        12.January(2010),
                        14.January(2010),
                    ]))
                .For(
                    numberOfTasks: 8,
                    throughputSelectionStrategy: new SeededRandom(seed: 1),
                    dayToStartForecastingFrom: 4.March(2010),
                    runs: 5)
                .Should().BeEquivalentTo(
                [
                    new Completion(when: 10.March(2010), occurrences: 1),
                    new Completion(when: 11.March(2010), occurrences: 1),
                    new Completion(when: 18.March(2010), occurrences: 1),
                    new Completion(when: 20.March(2010), occurrences: 1),
                    new Completion(when: 24.March(2010), occurrences: 1),
                ]);
        }

        [Test]
        public void NoCompletedTasks_PredictsNoCompletionDate()
        {
            Simulation
                .From(new Period(
                    from: today,
                    to: today,
                    tasksCompletionDates: []))
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
                    from: yesterday,
                    to: yesterday,
                    tasksCompletionDates: [yesterday]))
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
                    from: yesterday,
                    to: today,
                    tasksCompletionDates: [yesterday]))
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
                    from: yesterday,
                    to: today,
                    tasksCompletionDates: [today]))
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
                    from: today,
                    to: today,
                    tasksCompletionDates: [today]))
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
                    from: yesterday,
                    to: today,
                    tasksCompletionDates: [yesterday, today]))
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
                    from: today,
                    to: today,
                    tasksCompletionDates: [today]))
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
                    from: yesterday,
                    to: today,
                    tasksCompletionDates: [today]))
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
                    from: yesterday,
                    to: today,
                    tasksCompletionDates: [today]))
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
                    from: yesterday,
                    to: today,
                    tasksCompletionDates: [yesterday]))
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

        [Test]
        [Repeat(100)]
        public void AlwaysGenerateOneCompletionPerRun()
        {
            var runs = new Random().Next(1, 10000);

            Simulation
                .From(new Period(
                    from: yesterday,
                    to: today,
                    tasksCompletionDates: [yesterday, today]))
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: today,
                    runs: runs
                ).Sum(x => x.Occurrences).Should().Be(runs);
        }
    }
}