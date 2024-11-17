namespace Domain.Stragegies
{
    public interface IThroughputSelectionStrategy
    {
        int NextValueNew(Period period);
    }
}
