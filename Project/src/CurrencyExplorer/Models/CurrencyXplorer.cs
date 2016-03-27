using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.CurrencyImporters;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Enums;

namespace CurrencyExplorer.Models
{
    public class CurrencyXplorer
    {
        private DataProcessor _dataProcessor;
        private DataHolder _dataHolder;
        private DataPresenter _dataPresenter;

        private ICachingProcessor _iCachingProcessor;

        private ICurrencyProvider _iCurrencyProvider;

        private ICurrencyImporter _iCurrencyImporter;

        public CurrencyXplorer()
        {
            // Entry point for dependency injection.

            _iCurrencyImporter = new JsonCurrencyImporter();

            _iCurrencyProvider = new NationalBankCurrencyProvider(_iCurrencyImporter);

            _iCachingProcessor = new ApiDatabaseCachingProcessor(_iCurrencyProvider);

            _dataPresenter = new DataPresenter();
            _dataHolder = new DataHolder();
            _dataProcessor = new DataProcessor(_iCachingProcessor);
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
        public UserSettings RequestUserSettings(string cookie)
        {
            // Get from DB.

            Debug.WriteLine("User settings requested");


            return null;
            //throw new NotImplementedException();
        }

        public UserSettings RequestDefaultUserSettings()
        {
            // TODO: use data holder.

            UserSettings defaultUserSettings = new UserSettings()
            {
                Language = CurrencyExplorerLanguage.English,
                TimePeriod = new ChartTimePeriod()
                {
                    Begin = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
                    End = DateTime.Now
                },
            };

            ICollection<CurrencyCode> defaultCodes = new List<CurrencyCode>();

            var allCodes = _iCachingProcessor.RequestAllCurrencyCodes();

            defaultCodes.Add(allCodes.ElementAt(0));
            defaultCodes.Add(allCodes.ElementAt(1));

            IDictionary<CurrencyCode, CurrencyData> recvData = null;

            DateTime iterator = DateTime.Now;

            while (recvData == null)
            {
                recvData = _iCachingProcessor.RequestSingleData(iterator, defaultCodes);
                iterator = iterator.Subtract(TimeSpan.FromDays(1));
            }

            defaultUserSettings.Currencies = recvData.Select((pair) => pair.Value).ToList();

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
        public IDictionary<CurrencyCode, ICollection<ChartCurrencyDataPoint>> ChartDataPoints { get; set; }
        public IDictionary<CurrencyCode, CurrencyData> TodaysCurrencies { get; set; }
        public ICollection<CurrencyCode> ChartCurrencyCodes { get; set; }
    }
}
