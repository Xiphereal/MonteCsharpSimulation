﻿using FluentAssertions;
using FluentAssertions.Extensions;

namespace Domain.Tests
{
    public class ThroughputPerDayTests
    {
        private static readonly DateTime today = 1.February(2014);
        private static readonly DateTime yesterday = today.Subtract(1.Days());
        private static readonly DateTime tomorrow = today.AddDays(1);

        [Test]
        public void ThroughputForSingleDate()
        {
            new Period(
                    from: today,
                    to: today,
                    TasksCompletionDates: [today])
                .ThroughputPerDay()
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: today, Throughput: 1)
                ]);

            new Period(
                    from: today,
                    to: today,
                    TasksCompletionDates: [])
               .ThroughputPerDay()
               .Should().BeEquivalentTo(
               [
                   new ThroughputPerDay(Date: today, Throughput: 0)
               ]);
        }

        [Test]
        public void ThroughputForSeveralDifferentDates()
        {
            new Period(
                    from: yesterday,
                    to: today,
                    TasksCompletionDates: [yesterday, today])
                .ThroughputPerDay()
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: yesterday, Throughput: 1),
                    new ThroughputPerDay(Date: today, Throughput:1),
                ]);
        }

        [Test]
        public void ThroughputForDate_IsItsNumberOfOccurrences()
        {
            new Period(
                    from: today,
                    to: today,
                    TasksCompletionDates: [today, today])
                .ThroughputPerDay()
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: today, Throughput: 2),
                ]);
        }

        [Test]
        public void DatesWithDifferentThroughput()
        {
            new Period(
                    from: yesterday,
                    to: today,
                    TasksCompletionDates:
                    [
                        yesterday,
                        yesterday,
                        today,
                    ])
                .ThroughputPerDay()
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: yesterday, Throughput: 2),
                    new ThroughputPerDay(Date: today, Throughput: 1),
                ]);
        }

        [Test]
        public void DatesAreSortedAscending()
        {
            new Period(
                    from: yesterday,
                    to: today,
                    [today, yesterday])
                .ThroughputPerDay()
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: yesterday, Throughput: 1),
                    new ThroughputPerDay(Date: today, Throughput: 1),
                ], options => options.WithStrictOrdering());
        }

        [Test]
        public void DaysWhereNoTaskHasBeenCompleted_AreTakenIntoAccount()
        {
            new Period(
                    from: yesterday,
                    to: tomorrow,
                    TasksCompletionDates: [yesterday, tomorrow])
                .ThroughputPerDay()
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay(Date: yesterday, Throughput: 1),
                    new ThroughputPerDay(Date: today, Throughput: 0),
                    new ThroughputPerDay(Date: tomorrow, Throughput: 1),
                ]);
        }
    }
}
