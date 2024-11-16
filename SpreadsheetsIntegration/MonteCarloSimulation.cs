namespace SpreadsheetsIntegration
{
    public class MonteCarloSimulation
    {
        public static void Simulate(
            string fromSpreadsheetPath,
            string toSpreadsheetPath)
        {
            File.Create(toSpreadsheetPath);
        }
    }
}
