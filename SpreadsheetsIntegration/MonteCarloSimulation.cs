namespace SpreadsheetsIntegration
{
    public class MonteCarloSimulation
    {
        public static void Simulate(
            string fromSpreadsheetPath,
            string toSpreadsheetPath)
        {
            if (!File.Exists(fromSpreadsheetPath))
                throw new FileNotFoundException();
            using var disposedAtExitingScope = File.Create(toSpreadsheetPath);
        }
    }
}
