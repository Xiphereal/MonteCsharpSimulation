
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
    }
}