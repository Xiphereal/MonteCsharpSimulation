

namespace MonteCsharpSimulation
{
    public class Historic
    {
        public static IEnumerable<ThroughputPerDay> ThroughputPerDay(
            IEnumerable<DateTime> tasksCompletionDates)
        {
            return
            [
                new ThroughputPerDay()
                {
                    Date = new DateTime(year: 2014, month: 2, day: 1),
                    Throughput = 1,
                }
            ];
        }
    }
}
