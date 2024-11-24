namespace Domain
{
    public record Period
    {
        private readonly DateTime to;
        private readonly DateTime from;
        private readonly IEnumerable<DateTime> tasksCompletionDates;

        public Period(
            DateTime from,
            DateTime to,
            IEnumerable<DateTime> tasksCompletionDates)
        {
            if (from > to)
                throw new ArgumentException();

            this.from = from;
            this.to = to;
            this.tasksCompletionDates = tasksCompletionDates
                .Select(x => x.Date);
        }

        public bool IsEmpty => !tasksCompletionDates.Any();

        public IEnumerable<ThroughputPerDay> ThroughputPerDay()
        {
            return
                ThroughputPerDayForDatesWithAnyCompletedTask()
                    .Union(ZeroThroughputPerDayFor(DatesWithNoCompletedTask()))
                    .OrderBy(x => x.Date);
        }

        private IEnumerable<DateTime> DatesWithNoCompletedTask() =>
            EnumerateAllInBetween(from, to).Except(tasksCompletionDates);

        private static IEnumerable<ThroughputPerDay> ZeroThroughputPerDayFor(
            IEnumerable<DateTime> dates)
        {
            return dates
                .GroupBy(x => x.Date)
                .Select(
                    x => new ThroughputPerDay(
                        Date: x.Key,
                        Throughput: 0));
        }

        private IEnumerable<ThroughputPerDay> 
            ThroughputPerDayForDatesWithAnyCompletedTask()
        {
            return tasksCompletionDates
                .Where(x => IsWithin(from, to, x))
                .GroupBy(x => x.Date)
                .Select(
                    x => new ThroughputPerDay(
                        Date: x.Key,
                        Throughput: x.Count()));
        }

        private bool IsWithin(DateTime from, DateTime to, DateTime x)
        {
            return from <= x && x <= to;
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