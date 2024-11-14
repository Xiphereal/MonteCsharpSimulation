
namespace MonteCsharpSimulation
{
    public record Period(
        DateTime From,
        DateTime To,
        IEnumerable<DateTime> TasksCompletionDates);
}