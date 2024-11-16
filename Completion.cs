
namespace MonteCsharpSimulation
{
    public record Completion
    {
        public DateTime When { get; set; }
        public int Occurrences { get; set; }

        public Completion(DateTime When, int Occurrences)
        {
            this.When = When;
            this.Occurrences = Occurrences;
        }
    };
}