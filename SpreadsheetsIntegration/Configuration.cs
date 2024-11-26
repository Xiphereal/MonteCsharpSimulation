using Domain.Strategies;
using Random = Domain.Strategies.Random;

namespace SpreadsheetsIntegration
{
    public record Configuration
    {
        public Configuration(
            int Runs,
            IThroughputSelectionStrategy? ThroughputSelectionStrategy)
        {
            this.Runs = Runs;
            this.ThroughputSelectionStrategy = ThroughputSelectionStrategy ?? new Random();
        }

        public int Runs { get; }
        public IThroughputSelectionStrategy ThroughputSelectionStrategy { get; }
    }
}