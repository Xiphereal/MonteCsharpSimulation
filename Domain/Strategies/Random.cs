namespace Domain.Strategies
{
    public class Random : IThroughputSelectionStrategy
    {
        public int NextValue(Period period)
        {
            var throughputPerDays = period.ThroughputPerDay().ToArray();
            var randomIndex = 
                new System.Random().Next(throughputPerDays.Length);
            
            return throughputPerDays.ElementAt(randomIndex).Throughput;
        }
    }
}