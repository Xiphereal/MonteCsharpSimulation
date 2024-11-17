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

        public void SimulateFor(Period period)
        {
            this.period = period;
        }

        public int NextValue()
        {
            List<int> throughputPerDays = this.period
                .ThroughputPerDay()
                .Select(x => x.Throughput)
                .ToList();

            return throughputPerDays
                .ElementAt(this.random.Next(0, throughputPerDays.Count));
        }

        public int NextValueNew(Period period)
        {
            SimulateFor(period);
            return NextValue();
        }
    }
}
