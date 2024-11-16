using Domain;

namespace Domain.Stragegies
{
    public interface IThroughputSelectionStrategy
    {
        void SimulateFor(Period period);
        int NextValue();
    }
}
