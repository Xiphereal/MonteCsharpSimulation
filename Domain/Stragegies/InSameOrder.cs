
namespace Domain.Stragegies
{
    public class InSameOrder : IThroughputSelectionStrategy
    {
        private Queue<int> simulatedThroughtput = new();

        public int NextValueNew(Period period)
        {
            if (!simulatedThroughtput.Any())
                SimulateThroughput(period);

            return simulatedThroughtput.Dequeue();
        }

        private void SimulateThroughput(Period period1)
        {
            simulatedThroughtput = new Queue<int>(
                period1.ThroughputPerDay().Select(x => x.Throughput));
        }
    }
}