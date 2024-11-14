﻿using FluentAssertions;
using FluentAssertions.Extensions;

namespace MonteCsharpSimulation.Tests
{
    public class ThoughputPerDayTests
    {
        [Test]
        public void ThroughputForSingleDate()
        {
            Historic
                .ThroughputPerDay([1.February(2014)])
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay()
                    {
                        Date = 1.February(2014),
                        Throughput = 1,
                    }
                ]);
        }

        [Test]
        public void ThroughputForSeveralDifferentDates()
        {
            Historic
                .ThroughputPerDay([1.February(2014), 2.February(2014)])
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay()
                    {
                        Date = 1.February(2014),
                        Throughput = 1,
                    },
                    new ThroughputPerDay()
                    {
                        Date = 2.February(2014),
                        Throughput = 1,
                    }
                ]);
        }

        [Test]
        public void ThroughputForDate_IsItsNumberOfOcurrences()
        {
            Historic
                .ThroughputPerDay([1.February(2014), 1.February(2014)])
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay()
                    {
                        Date = 1.February(2014),
                        Throughput = 2,
                    }
                ]);
        }

        [Test]
        public void DatesWithDifferentThroughput()
        {
            Historic
                .ThroughputPerDay(
                [
                    1.February(2014),
                    1.February(2014),
                    2.February(2014),
                ])
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay()
                    {
                        Date = 1.February(2014),
                        Throughput = 2,
                    },
                    new ThroughputPerDay()
                    {
                        Date = 2.February(2014),
                        Throughput = 1,
                    }
                ]);
        }

        [Test]
        public void DatesAreSortedAscendantly()
        {
            Historic
                .ThroughputPerDay([2.February(2014), 1.February(2014)])
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay()
                    {
                        Date = 1.February(2014),
                        Throughput = 1,
                    },
                    new ThroughputPerDay()
                    {
                        Date = 2.February(2014),
                        Throughput = 1,
                    }
                ], options => options.WithStrictOrdering());
        }

        [Test]
        public void DaysWhereNoTaskHasBeenCompleted_AreTakenIntoAccount()
        {
            Historic
                .ThroughputPerDay(
                [
                    1.February(2014),
                    3.February(2014),
                ])
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay()
                    {
                        Date = 1.February(2014),
                        Throughput = 1,
                    },
                    new ThroughputPerDay()
                    {
                        Date = 2.February(2014),
                        Throughput = 0,
                    },
                    new ThroughputPerDay()
                    {
                        Date = 3.February(2014),
                        Throughput = 1,
                    }
                ]);
        }
    }
}