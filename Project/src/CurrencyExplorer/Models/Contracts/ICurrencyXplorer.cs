using System.Collections.Generic;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Enums;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyXplorer
    {
        IEnumerable<CurrencyCode> ChartCurrencyCodes { get; set; }
        IDictionary<CurrencyCode, IEnumerable<ChartCurrencyDataPoint>> ChartDataPoints { get; set; }
        ChartTimePeriod ChartTimePeriod { get; set; }
        IDictionary<CurrencyCode, CurrencyData> TodaysCurrencies { get; set; }

        void AddUserSettings(string ip, UserSettings userSettings);
        string ExportChartData(ExportFormat exportFormat);
        void RequestChartData();
        UserSettings RequestDefaultUserSettings();
        void RequestTodaysCurrencies();
        UserSettings RequestUserSettings(string ip);
        void SetCurrencyImporter(CurrencyImporterType importerType);
    }
}