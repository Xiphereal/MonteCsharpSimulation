
using MonteCsharpSimulation.Stragegies;

namespace MonteCsharpSimulation
{
    public class Simulation
    {
        private readonly Period period;

        private Simulation(Period period)
        {
            this.period = period;
        }

        public static Simulation From(Period period)
        {
            return new Simulation(period);
        }

        public IReadOnlyList<Completion> For(
            int numberOfTasks,
            InSameOrder throughputSelectionStrategy,
            DateTime dayToStartForecastingFrom,
            int runs = 1)
        {
            if (this.period.IsEmpty)
                return [];

            throughputSelectionStrategy.SimulateFor(period);

            int forecastedCompletionDays = 0;
            int forecastedCompletedTasks = numberOfTasks;
            while (forecastedCompletedTasks > 0)
            {
                forecastedCompletedTasks -=
                    throughputSelectionStrategy.NextValue();

                forecastedCompletionDays++;
            }

            return
            [
                new Completion(
                    When: dayToStartForecastingFrom
                        .AddDays(forecastedCompletionDays - 1),
                    Occurrences: runs)
            ];
        }
    }
}