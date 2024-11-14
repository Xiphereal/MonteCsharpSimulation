namespace MonteCsharpSimulation
{
    public class Historic
    {
        public static IEnumerable<ThroughputPerDay> ThroughputPerDay(Period period)
        {
            return
                ThroughputPerDayFor(period.TasksCompletionDates)
                    .Union(ZeroThroughputPerDayFor(
                        DatesWithNoCompletedTask(period)))
                    .OrderBy(x => x.Date);
        }

        private static IEnumerable<DateTime> DatesWithNoCompletedTask(Period period)
        {
            IEnumerable<DateTime> allDates =
                EnumerateAllInBetween(period.From, period.To);

            return allDates.Except(period.TasksCompletionDates);
        }

        private static IEnumerable<ThroughputPerDay> ZeroThroughputPerDayFor(
            IEnumerable<DateTime> dates)
        {
            return dates
                .GroupBy(x => x.Date)
                .Select(x => new ThroughputPerDay(
                    Date: x.Key,
                    Throughput: 0));
        }

        private static IEnumerable<ThroughputPerDay> ThroughputPerDayFor(
            IEnumerable<DateTime> dates)
        {
            return dates
                .GroupBy(x => x.Date)
                .Select(x => new ThroughputPerDay(
                    Date: x.Key,
                    Throughput: x.Count()));
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
