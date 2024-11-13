using FluentAssertions;
using FluentAssertions.Extensions;

namespace MonteCsharpSimulation.Tests
{
    public class SADfasdf
    {
        // Given the throughput per date and 1 task, a random day throughput is chosen as completion.
        // Given the throughput per date and 2 tasks, their completion is the sum of any two random days throughput.
        // A number of iterations can be specified. There are several possible completion dates.

        [Test]
        public void MyTestMethod()
        {
            Simulation
                .From(historic: [1.February(2014)])
                .Should().BeEquivalentTo(
                [
                    new Completion()
                    {
                        When = 1.February(2014),
                        Occurrences = 1,
                    }
                ]);
        }

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
                .ThroughputPerDay([1.February(2014), 2.March(2014)])
                .Should().BeEquivalentTo(
                [
                    new ThroughputPerDay()
                    {
                        Date = 1.February(2014),
                        Throughput = 1,
                    },
                    new ThroughputPerDay()
                    {
                        Date = 2.March(2014),
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
                    2.March(2014),
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
                        Date = 2.March(2014),
                        Throughput = 1,
                    }
                ]);
        }
    }
}
