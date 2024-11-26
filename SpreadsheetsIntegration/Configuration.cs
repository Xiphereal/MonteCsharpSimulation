using Domain.Strategies;
using Random = Domain.Strategies.Random;

namespace SpreadsheetsIntegration
{
    public record Configuration
    {
        public Configuration(
            int Runs,
            IThroughputSelectionStrategy? ThroughputSelectionStrategy,
            string nameOfHeaderForDeliveredTasksDates = "Delivered")
        {
            this.Runs = Runs;
            this.ThroughputSelectionStrategy = ThroughputSelectionStrategy ?? new Random();
            this.NameOfHeaderForDeliveredTasksDates = nameOfHeaderForDeliveredTasksDates;
        }


        public int Runs { get; }
        public IThroughputSelectionStrategy ThroughputSelectionStrategy { get; }
        public string NameOfHeaderForDeliveredTasksDates { get; }
    }
}