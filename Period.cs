
namespace MonteCsharpSimulation
{
    public record Period
    {
        public Period(
            DateTime From,
            DateTime To,
            IEnumerable<DateTime> TasksCompletionDates)
        {
            if (From > To)
                throw new ArgumentException();

            if (TasksCompletionDates.Any(x => x.Date < From || x.Date > To))
                throw new ArgumentException();

            this.From = From;
            this.To = To;
            this.TasksCompletionDates = TasksCompletionDates;
        }

        public DateTime From { get; }
        public DateTime To { get; }
        public IEnumerable<DateTime> TasksCompletionDates { get; }

        public bool IsEmpty => !TasksCompletionDates.Any();

        public IEnumerable<ThroughputPerDay> ThroughputPerDay()
        {
            return
                ThroughputPerDayForDatesWithAnyCompletedTask()
                    .Union(ZeroThroughputPerDayFor(DatesWithNoCompletedTask()))
                    .OrderBy(x => x.Date);
        }

        private IEnumerable<DateTime> DatesWithNoCompletedTask()
        {
            IEnumerable<DateTime> allDates =
                EnumerateAllInBetween(From, To);

            return allDates.Except(TasksCompletionDates);
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

        private IEnumerable<ThroughputPerDay> ThroughputPerDayForDatesWithAnyCompletedTask()
        {
            return TasksCompletionDates
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