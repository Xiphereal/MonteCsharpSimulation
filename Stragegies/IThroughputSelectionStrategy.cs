namespace MonteCsharpSimulation.Stragegies
{
    public interface IThroughputSelectionStrategy
    {
        void SimulateFor(Period period);
        int NextValue();
    }
}
