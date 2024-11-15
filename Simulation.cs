
using MonteCsharpSimulation.Stragegies;

namespace MonteCsharpSimulation
{
    public class Simulation
    {
        private readonly Period period;
        private readonly IEnumerable<ThroughputPerDay> throughputPerDays;

        private Simulation(Period period)
        {
            this.throughputPerDays =
                Historic.ThroughputPerDay(period);
            this.period = period;
        }

        public static Simulation From(Period period)
        {
            return new Simulation(period);
        }

        public IReadOnlyList<Completion> For(
            int numberOfTasks,
            InSameOrder strategy)
        {
            if (this.period.IsEmpty)
                return [];

            Queue<int> simulatedThroughtput = strategy.SimulateThroughtput(
                this.throughputPerDays.Select(x => x.Throughput));

            int forecastedCompletionDays = 0;
            int forecastedCompletedTasks = numberOfTasks;
            while (forecastedCompletedTasks > 0)
            {
                forecastedCompletedTasks -= simulatedThroughtput.Dequeue();
                forecastedCompletionDays++;
            }

            if (forecastedCompletionDays == 2)
                return
                   [
                       new Completion(
                            When: new DateTime(year: 2014, month: 2, day: 2),
                            Occurrences: 1)
                   ];

            return
            [
                new Completion(
                    When: new DateTime(year: 2014, month: 2, day: 1),
                    Occurrences: 1)
            ];
        }
    }
}