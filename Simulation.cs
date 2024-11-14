
namespace MonteCsharpSimulation
{
    public class Simulation
    {
        private readonly IEnumerable<DateTime> tasksCompletionDates;

        private Simulation(IEnumerable<DateTime> from)
        {
            this.tasksCompletionDates = from;
        }

        public static Simulation From(Period period)
        {
            return new Simulation(from: period.TasksCompletionDates);
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