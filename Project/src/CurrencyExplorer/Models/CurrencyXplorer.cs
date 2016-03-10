using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Enums;

namespace CurrencyExplorer.Models
{
    public class CurrencyXplorer
    {
        private DataProcessor _dataProcessor;
        private DataHolder _dataHolder;
        private DataPresenter _dataPresenter;

        public CurrencyXplorer(ICachingProcessor cachingProcessor)
        {
            _dataPresenter = new DataPresenter();
            _dataHolder = new DataHolder();
            _dataProcessor = new DataProcessor();
        }

        /// <summary>
        /// Requests data to be displayed in the chart.
        /// Requires ChartTimePeriod, ChartCurrencyCodes.
        /// The result is stored in ChartDataPoints.
        /// </summary>
        public void RequestChartData()
        {
            ChartDataPoints = _dataProcessor.GetChartData(ChartTimePeriod, ChartCurrencyCodes);
        }

        /// <summary>
        /// Requests todays currencies.
        /// Currencies are specified in ChartCurrencyCodes.
        /// </summary>
        /// <returns>Currency data object.</returns>
        public void RequestTodaysCurrencies()
        {
            //TodaysCurrencies
            TodaysCurrencies = _dataProcessor.GetDailyCurrencies(DateTime.Now, ChartCurrencyCodes);
        }

        /// <summary>
        /// Adds user settings into the database.
        /// </summary>
        /// <param name="ip">The IP that user is browsing from.</param>
        /// <param name="userSettings">User settings.</param>
        public void AddUserSettings(string ip, UserSettings userSettings)
        {

        }

        /// <summary>
        /// Requests user settings by specified IP.
        /// </summary>
        /// <returns>The user settings.</returns>
        public UserSettings RequestUserSettings(string ip)
        {
            // Get from DB.

            Debug.WriteLine("User settings requested");


            return null;
            //throw new NotImplementedException();
        }

        public UserSettings RequestDefaultUserSettings()
        {
            UserSettings defaultUserSettings = new UserSettings()
            {
                Language = CurrencyExplorerLanguage.English,
                TimePeriod = new ChartTimePeriod()
                {
                    Begin = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
                    End = DateTime.Now
                },
                Currencies =
                    new List<CurrencyData>()
                    {
                        new CurrencyData()
                        {
                            Code = new CurrencyCode() {Value = "r030"},
                            Value = 27.5,
                            ShortName = "USD",
                            Name = "US Dollars",
                            Actual = DateTime.Now
                        },
                        new CurrencyData()
                        {
                            Code = new CurrencyCode() {Value = "r040"},
                            Value = 35.2,
                            ShortName = "EUR",
                            Name = "Europe Euro",
                            Actual = DateTime.Now
                        }
                    }
            };

            return defaultUserSettings;
        }

        /// <summary>
        /// Exports data from chart in specified format.
        /// </summary>
        /// <param name="exportFormat">The format in which chart is to be exported.</param>
        /// <returns>The path to the generated file.</returns>
        public string ExportChartData(ExportFormat exportFormat)
        {
            throw new NotImplementedException();
        }

        public void SetCurrencyImporter(CurrencyImporterType importerType)
        {
            
        }

        public ChartTimePeriod ChartTimePeriod { get; set; }
        public IDictionary<CurrencyCode, IEnumerable<ChartCurrencyDataPoint>> ChartDataPoints { get; set; }
        public IDictionary<CurrencyCode, CurrencyData> TodaysCurrencies { get; set; }
        public IEnumerable<CurrencyCode> ChartCurrencyCodes { get; set; }
    }
}
