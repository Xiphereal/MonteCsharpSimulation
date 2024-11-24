namespace Domain.Strategies
{
    public interface IThroughputSelectionStrategy
    {
        int NextValue(Period period);
    }
}
