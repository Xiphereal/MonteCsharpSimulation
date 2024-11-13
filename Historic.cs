

namespace MonteCsharpSimulation
{
    public class Historic
    {
        public static IEnumerable<ThroughputPerDay> ThroughputPerDay(
            IEnumerable<DateTime> tasksCompletionDates)
        {
            return
                tasksCompletionDates.Select(date => new ThroughputPerDay()
                {
                    Date = date,
                    Throughput = 1,
                });
        }
    }
}
