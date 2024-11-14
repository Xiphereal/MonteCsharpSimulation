
namespace MonteCsharpSimulation
{
    public class Simulation
    {
        private readonly IEnumerable<DateTime> tasksCompletionDates;

        private Simulation(IEnumerable<DateTime> from)
        {
            this.tasksCompletionDates = from;
        }

        public static Simulation From(
            IEnumerable<DateTime> tasksCompletionDates)
        {
            return new Simulation(from: tasksCompletionDates);
        }

        public IReadOnlyList<Completion> For(int numberOfTasks)
        {
            return
            [
                new Completion()
                {
                    When = new DateTime(year: 2014, month: 2, day: 1),
                    Occurrences = 1,
                }
            ];
        }
    }
}