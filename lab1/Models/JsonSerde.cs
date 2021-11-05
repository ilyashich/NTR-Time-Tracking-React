using System.Text.Json;
using System;
using System.Collections.Generic;

namespace TimeReporter.Models
{
    public class JsonSerde
    {
        private const string StandardPath = "../TimeReporter/data/data.json";

        public static void CreateNewMonthReport(string surname, DateTime date)
        {
            string jsonDatafile = "../TimeReporter/data/" + surname + "-" + date.ToString("yyyy-MM") + ".json";
            Report report = new Report()
            {
                Frozen = false,
                Entries = new List<Entry>(),
                Accepted = new List<AcceptedTime>()
            };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(report, options);
            System.IO.File.WriteAllText(jsonDatafile,jsonString);
        }

        public static Data GetData()
        {
            string jsonString = System.IO.File.ReadAllText(StandardPath);
            Data data = JsonSerializer.Deserialize<Data>(jsonString);

            return data;
        }

        public static void SaveDataChanges(Data data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(data, options);
            System.IO.File.WriteAllText(StandardPath, jsonString);
        }

        public static Report GetReport(string surname, DateTime date)
        {
            string jsonDatafile = "../TimeReporter/data/" + surname + "-" + date.ToString("yyyy-MM") + ".json";
            if (!System.IO.File.Exists(jsonDatafile)) return null;
            string jsonString = System.IO.File.ReadAllText(jsonDatafile);
            Report report = JsonSerializer.Deserialize<Report>(jsonString);
            return report;

        }
        
        public static Report GetReport(string surname, string month, int year)
        {
            string jsonDatafile = "../TimeReporter/data/" + surname + "-" + year + "-" + month + ".json";
            if (!System.IO.File.Exists(jsonDatafile)) return null;
            string jsonString = System.IO.File.ReadAllText(jsonDatafile);
            Report report = JsonSerializer.Deserialize<Report>(jsonString);
            return report;

        }

        public static void SaveReportChanges(Report report,string surname, DateTime date)
        {
            string jsonDatafile = "../TimeReporter/data/" + surname + "-" + date.ToString("yyyy-MM") + ".json";
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(report, options);
            System.IO.File.WriteAllText(jsonDatafile, jsonString);
        }
        
        public static void SaveReportChanges(Report report,string surname, string month, int year)
        {
            string jsonDatafile = "../TimeReporter/data/" + surname + "-" + year + "-" + month + ".json";
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(report, options);
            System.IO.File.WriteAllText(jsonDatafile, jsonString);
        }
    }
}