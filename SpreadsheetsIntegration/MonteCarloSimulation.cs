using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Domain;
using Domain.Strategies;

namespace SpreadsheetsIntegration
{
    public static class MonteCarloSimulation
    {
        private static readonly CsvConfiguration CsvConfiguration =
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = Delimiter,
            };

        private const string Delimiter = ";";

        public static void Simulate(
            string fromSpreadsheetPath,
            string toSpreadsheetPath,
            DateTime from,
            DateTime to,
            IThroughputSelectionStrategy throughputSelectionStrategy,
            int runs,
            DateTime dayToStartForecastingFrom)
        {
            IEnumerable<TaskRecord> tasks = Read(fromSpreadsheetPath);

            var completions = Simulation
                .From(
                    new Period(
                        from,
                        to,
                        tasksCompletionDates: tasks
                            .Where(x => x.Delivered.HasValue)
                            .Select(x => x.Delivered!.Value)))
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy,
                    dayToStartForecastingFrom,
                    runs);

            Write(
                toSpreadsheetPath,
                completions
                    .Select(
                        x => new CompletionRecord
                        {
                            When = x.When,
                            Occurrences = x.Occurrences
                        }));
        }

        private static IReadOnlyList<TaskRecord> Read(string fromSpreadsheetPath)
        {
            if (!File.Exists(fromSpreadsheetPath))
                throw new FileNotFoundException();

            using var reader = new StreamReader(fromSpreadsheetPath);
            using var csv = new CsvReader(reader, CsvConfiguration);
            csv.Context.RegisterClassMap<TaskRecordMap>();

            return csv.GetRecords<TaskRecord>().ToArray();
        }

        private static void Write(
            string toSpreadsheetPath,
            IEnumerable<CompletionRecord> completions)
        {
            if (File.Exists(toSpreadsheetPath))
                File.Delete(toSpreadsheetPath);

            using var writer = new StreamWriter(toSpreadsheetPath);
            using var result = new CsvWriter(writer, CsvConfiguration);
            result.Context.RegisterClassMap<TaskRecordMap>();

            result.WriteRecords(completions);
        }

        private class TaskRecord
        {
            public DateTime? Delivered { get; set; }
        }

        private class TaskRecordMap : ClassMap<TaskRecord>
        {
            public TaskRecordMap()
            {
                Map(m => m.Delivered)
                    .Name("Delivered")
                    .TypeConverterOption
                    .Format(
                        "dd/MM/yyyy HH:mm",
                        "dd/MM/yyyy H:mm");
            }
        }

        private class CompletionRecord
        {
            public DateTime When { get; set; }
            public int Occurrences { get; set; }
        }
    }
}