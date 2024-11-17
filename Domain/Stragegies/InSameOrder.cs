
using Domain;

namespace Domain.Stragegies
{
    public class InSameOrder : IThroughputSelectionStrategy
    {
        private Period period;
        private Queue<int> simulatedThroughtput = new();

        private void SimulateFor(Period period)
        {
            this.period = period;
        }

        private int NextValue()
        {
            if (!simulatedThroughtput.Any())
                SimulateThroughput();

            return simulatedThroughtput.Dequeue();
        }

        private void SimulateThroughput()
        {
            simulatedThroughtput = new Queue<int>(
                period.ThroughputPerDay().Select(x => x.Throughput));
        }

        public int NextValueNew(Period period)
        {
            SimulateFor(period);
            return NextValue();
        }
    }
}