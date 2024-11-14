using FluentAssertions;
using FluentAssertions.Extensions;

namespace MonteCsharpSimulation.Tests
{
    public class SimulationTests
    {
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
    }
}
