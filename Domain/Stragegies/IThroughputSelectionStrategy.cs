namespace Domain.Stragegies
{
    public interface IThroughputSelectionStrategy
    {
        int NextValue(Period period);
    }
}
