using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Domain;
using Domain.Stragegies;

namespace SpreadsheetsIntegration
{
    public class MonteCarloSimulation
    {
        public static void Simulate(
            string fromSpreadsheetPath,
            string toSpreadsheetPath,
            int runs)
        {
            IEnumerable<TaskRecord> tasks = Read(fromSpreadsheetPath);

            var completions = Simulation
                .From(new Period(
                    from: DateTime.Now.AddDays(-1),
                    to: DateTime.Now.AddDays(1),
                    tasksCompletionDates: [DateTime.Now]))
                .For(
                    numberOfTasks: 1,
                    throughputSelectionStrategy: new InSameOrder(),
                    dayToStartForecastingFrom: DateTime.Now,
                    runs);

            Write(
                toSpreadsheetPath,
                completions
                    .Select(x => new CompletionRecord
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
            using var csv = new CsvReader(
                reader,
                new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                });
            csv.Context.RegisterClassMap<TaskRecordMap>();

            return csv.GetRecords<TaskRecord>().ToArray();
        }

        private static void Write(string toSpreadsheetPath,
            IEnumerable<CompletionRecord> completions)
        {
            if (File.Exists(toSpreadsheetPath))
                File.Delete(toSpreadsheetPath);
            
            using var writer = new StreamWriter(toSpreadsheetPath);
            using var result = new CsvWriter(writer, CultureInfo.InvariantCulture);
            result.Context.RegisterClassMap<TaskRecordMap>();

            result.WriteRecords(completions);
        }

        private class TaskRecord
        {
            public string Task { get; set; }
            public DateTime Started { get; set; }
            public DateTime Delivered { get; set; }
        }

        private class TaskRecordMap : ClassMap<TaskRecord>
        {
            public TaskRecordMap()
            {
                Map(m => m.Task).Name("Task");
                Map(m => m.Started).Name("Started")
                    .TypeConverterOption.Format("dd/MM/yy HH:mm");
                Map(m => m.Delivered).Name("Delivered")
                    .TypeConverterOption.Format("dd/MM/yy HH:mm");
            }
        }

        private class CompletionRecord
        {
            public DateTime When { get; set; }
            public int Occurrences { get; set; }
        }
    }
}
