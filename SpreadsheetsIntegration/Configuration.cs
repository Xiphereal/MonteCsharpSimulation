using Domain.Strategies;
using Random = Domain.Strategies.Random;

namespace SpreadsheetsIntegration
{
    public record Configuration
    {
        public Configuration(
            int Runs,
            IThroughputSelectionStrategy? ThroughputSelectionStrategy,
            string? NameOfHeaderForDeliveredTasksDates = null)
        {
            this.Runs = Runs;
            this.ThroughputSelectionStrategy = ThroughputSelectionStrategy ?? new Random();
            this.NameOfHeaderForDeliveredTasksDates =
                NameOfHeaderForDeliveredTasksDates ?? "Delivered";
        }


        public int Runs { get; }
        public IThroughputSelectionStrategy ThroughputSelectionStrategy { get; }
        public string NameOfHeaderForDeliveredTasksDates { get; }
    }
}