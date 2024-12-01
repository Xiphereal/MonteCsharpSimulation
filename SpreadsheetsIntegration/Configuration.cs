using Domain.Strategies;
using Random = Domain.Strategies.Random;

namespace SpreadsheetsIntegration
{
    public record Configuration
    {
        /// <param name="ThroughputSelectionStrategy"> <see cref="Random"/> if none specified. </param>
        /// <param name="DayToStartForecastingFrom"> <see cref="DateTime.Today"/> if none specified. </param>
        /// <param name="NameOfHeaderForDeliveredTasksDates"> 'Delivered' if none specified. </param>
        public Configuration(
            int TasksToBeCompleted,
            int Runs,
            IThroughputSelectionStrategy? ThroughputSelectionStrategy,
            DateTime? DayToStartForecastingFrom = null,
            string? NameOfHeaderForDeliveredTasksDates = null)
        {
            this.TasksToBeCompleted = TasksToBeCompleted;
            this.Runs = Runs;
            this.ThroughputSelectionStrategy = ThroughputSelectionStrategy ?? new Random();
            this.NameOfHeaderForDeliveredTasksDates =
                NameOfHeaderForDeliveredTasksDates ?? "Delivered";
            this.DayToStartForecastingFrom = DayToStartForecastingFrom ?? DateTime.Today;
        }

        public int TasksToBeCompleted { get; }
        public int Runs { get; }
        public DateTime DayToStartForecastingFrom { get; }
        public IThroughputSelectionStrategy ThroughputSelectionStrategy { get; }
        public string NameOfHeaderForDeliveredTasksDates { get; }
    }
}