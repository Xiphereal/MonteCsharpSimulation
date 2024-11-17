using Domain;

namespace Domain.Stragegies
{
    public class SeededRandom : IThroughputSelectionStrategy
    {
        private Period period;
        private readonly Random random;

        public SeededRandom(int seed)
        {
            this.random = new Random(Seed: seed);
        }

        private void SimulateFor(Period period)
        {
            this.period = period;
        }

        private int NextValue()
        {
            List<int> throughputPerDays = this.period
                .ThroughputPerDay()
                .Select(x => x.Throughput)
                .ToList();

            return throughputPerDays
                .ElementAt(this.random.Next(0, throughputPerDays.Count));
        }

        public int NextValue(Period period)
        {
            this.period = period;
            return NextValue();
        }
    }
}
