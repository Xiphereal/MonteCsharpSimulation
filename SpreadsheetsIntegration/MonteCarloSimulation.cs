using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Domain;
using Domain.Strategies;
using Random = Domain.Strategies.Random;

namespace SpreadsheetsIntegration
{
    public static class MonteCarloSimulation
    {
        private const string Delimiter = ";";

        private static readonly CsvConfiguration CsvConfiguration =
            new(CultureInfo.InvariantCulture)
            {
                Delimiter = Delimiter,
            };

        public static void Simulate(
            string fromSpreadsheetPath,
            string toSpreadsheetPath,
            DateTime from,
            DateTime to,
            Configuration config)
        {
            IEnumerable<TaskRecord> tasks = Read(
                fromSpreadsheetPath,
                config.NameOfHeaderForDeliveredTasksDates);

            var completions = Simulation
                .From(
                    new Period(
                        from,
                        to,
                        tasksCompletionDates: tasks
                            .Where(x => x.Delivered.HasValue)
                            .Select(x => x.Delivered!.Value)))
                .For(
                numberOfTasks: config.TasksToBeCompleted,
                    config.ThroughputSelectionStrategy,
                    config.DayToStartForecastingFrom,
                    config.Runs);

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

        private static IReadOnlyList<TaskRecord> Read(
            string fromSpreadsheetPath,
            string nameOfHeaderForDeliveredTasksDates)
        {
            if (!File.Exists(fromSpreadsheetPath))
                throw new FileNotFoundException();

            using var reader = new StreamReader(fromSpreadsheetPath);
            using var csv = new CsvReader(reader, CsvConfiguration);
            csv.Context.RegisterClassMap(
                CreateTaskRecordMapWith(nameOfHeaderForDeliveredTasksDates));

            return csv.GetRecords<TaskRecord>().ToArray();
        }

        private static TaskRecordMap CreateTaskRecordMapWith(string headerName)
        {
            var map = new TaskRecordMap();

            var property = typeof(TaskRecord).GetProperty(nameof(TaskRecord.Delivered));
            map.Map(typeof(TaskRecord), property!).Name(headerName);

            return map;
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