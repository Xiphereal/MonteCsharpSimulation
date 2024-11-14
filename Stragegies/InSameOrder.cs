namespace MonteCsharpSimulation.Stragegies
{
    public class InSameOrder
    {
        public Queue<int> SimulateThroughtput(IEnumerable<int> value) =>
            new Queue<int>(value);
    }
}