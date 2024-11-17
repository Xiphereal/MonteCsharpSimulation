
namespace Domain.Stragegies
{
    public class InSameOrder : IThroughputSelectionStrategy
    {
        private Queue<int> simulatedThroughtput = new();

        public int NextValue(Period period)
        {
            if (!simulatedThroughtput.Any())
                SimulateThroughput(period);

            return simulatedThroughtput.Dequeue();
        }

        private void SimulateThroughput(Period period)
        {
            simulatedThroughtput = new Queue<int>(
                period.ThroughputPerDay().Select(x => x.Throughput));
        }
    }
}