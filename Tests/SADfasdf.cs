﻿using FluentAssertions;
using FluentAssertions.Extensions;

namespace MonteCsharpSimulation.Tests
{
    public class SADfasdf
    {
        // Given completion dates, their frequency is computed.
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
    }
}
