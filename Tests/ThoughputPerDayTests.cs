using FluentAssertions;
using FluentAssertions.Extensions;

namespace MonteCsharpSimulation.Tests
{
    public class ThoughputPerDayTests
    {
        private static readonly DateTime today = 1.February(2014);
        private static readonly DateTime yesterday = today.Subtract(1.Days());
        private static readonly DateTime tomorrow = today.AddDays(1);

        [Test]
        public void ThroughputForSingleDate()
        {
            Historic
                .ThroughputPerDay(
                    new Period(
                        From: today,
                        To: today,
                        TasksCompletionDates: [today]))
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: today, Throughput: 1)
                ]);

            Historic
               .ThroughputPerDay(new Period(
                    From: today,
                    To: today,
                    TasksCompletionDates: []))
               .Should().BeEquivalentTo(
               [
                   new ThroughputPerDay(Date: today, Throughput: 0)
               ]);
        }

        [Test]
        public void ThroughputForSeveralDifferentDates()
        {
            Historic
                .ThroughputPerDay(
                    new Period(
                        From: yesterday,
                        To: today,
                        TasksCompletionDates: [yesterday, today]))
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: yesterday, Throughput: 1),
                    new ThroughputPerDay(Date: today, Throughput:1),
                ]);
        }

        [Test]
        public void ThroughputForDate_IsItsNumberOfOcurrences()
        {
            Historic
                .ThroughputPerDay(
                    new Period(
                        From: today,
                        To: today,
                        TasksCompletionDates: [today, today]))
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: today, Throughput: 2),
                ]);
        }

        [Test]
        public void DatesWithDifferentThroughput()
        {
            Historic
                .ThroughputPerDay(
                    new Period(
                        From: yesterday,
                        To: today,
                        TasksCompletionDates:
                        [
                            yesterday,
                            yesterday,
                            today,
                        ]))
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: yesterday, Throughput: 2),
                    new ThroughputPerDay(Date: today, Throughput: 1),
                ]);
        }

        [Test]
        public void DatesAreSortedAscendantly()
        {
            Historic
                .ThroughputPerDay(
                    new Period(
                        From: yesterday,
                        To: today,
                    [today, yesterday]))
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: yesterday, Throughput: 1),
                    new ThroughputPerDay(Date: today, Throughput: 1),
                ], options => options.WithStrictOrdering());
        }

        [Test]
        public void DaysWhereNoTaskHasBeenCompleted_AreTakenIntoAccount()
        {
            Historic
                .ThroughputPerDay(
                    new Period(
                        From: yesterday,
                        To: tomorrow,
                        TasksCompletionDates: [yesterday, tomorrow]))
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: yesterday, Throughput: 1),
                    new ThroughputPerDay(Date: today, Throughput: 0),
                    new ThroughputPerDay(Date: tomorrow, Throughput: 1),
                ]);
        }
    }
}
