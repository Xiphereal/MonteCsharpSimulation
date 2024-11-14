
namespace MonteCsharpSimulation
{
    public class Simulation
    {
        public static IReadOnlyList<Completion> From(
            IEnumerable<DateTime> tasksCompletionDates)
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