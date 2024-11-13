

namespace MonteCsharpSimulation
{
    public class Historic
    {
        public static IEnumerable<ThroughputPerDay> ThroughputPerDay(
            IEnumerable<DateTime> tasksCompletionDates)
        {
            return
                tasksCompletionDates
                    .GroupBy(t => t.Date)
                    .Select(x => new ThroughputPerDay()
                    {
                        Date = x.Key,
                        Throughput = x.Count(),
                    });
        }
    }
}
