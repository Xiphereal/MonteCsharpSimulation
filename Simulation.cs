
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
            IThroughputSelectionStrategy throughputSelectionStrategy,
            DateTime dayToStartForecastingFrom,
            int runs = 1)
        {
            if (this.period.IsEmpty)
                return [];

            throughputSelectionStrategy.SimulateFor(period);

            var forecast = new Forecast();

            for (int i = 0; i < runs; i++)
            {
                int forecastedCompletionDays =
                    ForecastCompletionDays(
                        numberOfTasks,
                        throughputSelectionStrategy);

                forecast.Add(new Completion(
                    when: dayToStartForecastingFrom
                        .AddDays(forecastedCompletionDays - 1),
                    occurrences: 1));
            }

            return forecast;
        }

        private static int ForecastCompletionDays(
            int numberOfTasks,
            IThroughputSelectionStrategy throughputSelectionStrategy)
        {
            int forecastedCompletionDays = 0;
            int forecastedCompletedTasks = numberOfTasks;
            while (forecastedCompletedTasks > 0)
            {
                forecastedCompletedTasks -=
                    throughputSelectionStrategy.NextValue();

                forecastedCompletionDays++;
            }

            return forecastedCompletionDays;
        }
    }
}