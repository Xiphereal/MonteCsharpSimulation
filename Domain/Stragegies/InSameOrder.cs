
using Domain;

namespace Domain.Stragegies
{
    public class InSameOrder : IThroughputSelectionStrategy
    {
        private Period period;
        private Queue<int> simulatedThroughtput = new();

        public void SimulateFor(Period period)
        {
            this.period = period;
        }

        public int NextValue()
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
    }
}