

namespace MonteCsharpSimulation
{
    public class Historic
    {
        public static IEnumerable<ThroughputPerDay> ThroughputPerDay(
            IEnumerable<DateTime> tasksCompletionDates)
        {
            return
                ThroughputPerDayFor(tasksCompletionDates)
                    .Union(ZeroThroughputPerDayFor(
                        DatesWithNoCompletedTask(tasksCompletionDates)))
                    .OrderBy(x => x.Date);
        }

        private static IEnumerable<DateTime> DatesWithNoCompletedTask(
            IEnumerable<DateTime> timePeriodWithLapsus)
        {
            IEnumerable<DateTime> allDates =
                EnumerateAllInBetween(
                    timePeriodWithLapsus.Order().First(),
                    timePeriodWithLapsus.Order().Last());

            return allDates.Except(timePeriodWithLapsus);
        }

        private static IEnumerable<ThroughputPerDay> ZeroThroughputPerDayFor(
            IEnumerable<DateTime> dates)
        {
            return dates
                .GroupBy(x => x.Date)
                .Select(x => new ThroughputPerDay()
                {
                    Date = x.Key,
                    Throughput = 0,
                });
        }

        private static IEnumerable<ThroughputPerDay> ThroughputPerDayFor(
            IEnumerable<DateTime> dates)
        {
            return dates
                .GroupBy(x => x.Date)
                .Select(x => new ThroughputPerDay()
                {
                    Date = x.Key,
                    Throughput = x.Count(),
                });
        }

        private static IEnumerable<DateTime> EnumerateAllInBetween(
            DateTime startDate,
            DateTime endDate)
        {
            return Enumerable
                .Range(0, (endDate - startDate).Days + 1)
                .Select(offset => startDate.AddDays(offset));
        }
    }
}
