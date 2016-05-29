using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Utilities;

namespace CurrencyExplorer.Models.Exporters
{
    public class CsvCurrencyExporter : AbstractCurrencyExporter<String>
    {
        public override void Export()
        {
            string result = $"Date{GetCsvSeparator()}";
            string fileName = Constants.TempTableFileName;

            var header = InputData.Select(x => new { Alias = x.Key.Alias, Name = x.Key.Name }).ToArray();
            int currenciesCount = header.Length - 1;

            foreach (var title in header)
            {
                result += $"{title.Alias}{GetCsvSeparator()}";
            }

            result += GetNewLineSeparator();

            Dictionary<string, Dictionary<string, string>> csvLines = new Dictionary<string, Dictionary<string, string>>();

            foreach (var pair in InputData)
            {
                string alias = pair.Key.Alias;
                foreach (var dataPoint in pair.Value)
                {
                    string date = Utils.GetClientFormattedDateString(dataPoint.DataObject.ActualDate, ".");
                    string value = dataPoint.DataObject.Value.ToString();

                    if (csvLines.ContainsKey(date) == false)
                    {
                        csvLines.Add(date, new Dictionary<string, string>());
                    }

                    if (csvLines[date].ContainsKey(alias) == false)
                    {
                        csvLines[date].Add(alias, value);
                    }
                }

            }

            foreach (var csvLine in csvLines)
            {
                result += String.Format("{0}{1}", csvLine.Key, GetCsvSeparator());

                foreach (var pair in csvLine.Value)
                {
                    result += String.Format("{0}{1}", pair.Value, GetCsvSeparator());
                }
                result += GetNewLineSeparator();
            }

            try
            {
                File.WriteAllText(fileName, result);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }

            Result = fileName;
        }

        private string GetNewLineSeparator()
        {
            return Environment.NewLine;
        }

        private string GetCsvSeparator()
        {
            return ",";
        }
    }
}
