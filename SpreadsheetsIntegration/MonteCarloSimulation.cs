using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace SpreadsheetsIntegration
{
    public class MonteCarloSimulation
    {
        public static void Simulate(
            string fromSpreadsheetPath,
            string toSpreadsheetPath)
        {
            IEnumerable<TaskRecord> tasks = Read(fromSpreadsheetPath);

            Write(toSpreadsheetPath, tasks);
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

        private static void Write(
            string toSpreadsheetPath,
            IEnumerable<TaskRecord> tasks)
        {
            using var writer = new StreamWriter(toSpreadsheetPath);
            using var result = new CsvWriter(writer, CultureInfo.InvariantCulture);
            result.Context.RegisterClassMap<TaskRecordMap>();

            result.WriteRecords(tasks);
        }

        public class TaskRecord
        {
            public string Task { get; set; }
            public DateTime Started { get; set; }
            public DateTime Delivered { get; set; }
        }

        public class TaskRecordMap : ClassMap<TaskRecord>
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
    }
}
