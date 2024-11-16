
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

            var result = new List<Completion>();

            for (int i = 0; i < runs; i++)
            {
                int forecastedCompletionDays =
                    ForecastCompletionDays(
                        numberOfTasks,
                        throughputSelectionStrategy);

                Completion completion = new Completion(
                    When: dayToStartForecastingFrom
                        .AddDays(forecastedCompletionDays - 1),
                    Occurrences: 1);

                AddTo(completion, result);
            }

            return result;
        }

        private static void AddTo(
            Completion completion,
            List<Completion> result)
        {
            if (result.Contains(completion))
            {
                result.Find(x => x == completion).Occurrences++;
            }
            else
            {
                result.Add(completion);
            }
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