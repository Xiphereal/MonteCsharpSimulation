using FluentAssertions;
using FluentAssertions.Extensions;

namespace MonteCsharpSimulation.Tests
{
    public class SimulationTests
    {
        // Given the throughput per date and 1 task, a random day throughput is chosen as completion.
        // Given the throughput per date and 2 tasks, their completion is the sum of any two random days throughput.
        // A number of iterations can be specified. There are several possible completion dates.

        [Test]
        public void MyTestMethod()
        {
            Simulation
                .From(tasksCompletionDates: [1.February(2014)])
                .For(numberOfTasks: 1)
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
