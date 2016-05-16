using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.CurrencyImporters;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Models.Enums;
using CurrencyExplorer.Models.Repositories;
using CurrencyExplorer.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace CurrencyExplorer.Models
{
    public class CurrencyXplorer
    {
        private DataProcessor _dataProcessor;
        private DataHolder _dataHolder;
        private DataPresenter _dataPresenter;

        private ICachingProcessor _iCachingProcessor;

        private ICurrencyProvider _iCurrencyProvider;
        private IExplorerRepository _iCurrencyRepository;
        private ICurrencyImporter _iCurrencyImporter;

        private IUserSettingsHolder _iUserSettingsHolder;

        private CurrencyDataContext _currencyDataContext;

        private ICollection<CurrencyCodeEntry> _allCurrencyCodes;

        public IConfiguration Configuration { get; set; }

        public CurrencyXplorer()
        {
            _allCurrencyCodes = null;
        }

        public CurrencyXplorer(IApplicationEnvironment appEnv) : this()
        {
            Configuration = Utils.CreateConfiguration(appEnv, "config.json");

            // Entry point for dependency injection.

            _iCurrencyImporter = new JsonCurrencyImporter();
            //_iCurrencyImporter = new LocalJsonCurrencyImporter(); ;

            _iCurrencyProvider = new NationalBankCurrencyProvider(_iCurrencyImporter);

            _currencyDataContext = new CurrencyDataContext(Configuration["Data:DefaultConnection:ConnectionString"]);
            _iCurrencyRepository = new MsSqlExplorerRepository(_currencyDataContext);

            _iCachingProcessor = new ApiDatabaseCachingProcessor(_iCurrencyProvider, _iCurrencyRepository);

            _iUserSettingsHolder = new UserSettingsHolder(_iCurrencyRepository);

            _dataPresenter = new DataPresenter();
            _dataHolder = new DataHolder(_iCachingProcessor, _iUserSettingsHolder, Configuration);
            _dataProcessor = new DataProcessor(_iCachingProcessor);

            _allCurrencyCodes = GetAllCurrencyCodes();
        }

        public ICollection<CurrencyCodeEntry> GetAllCurrencyCodes()
        {
            ICollection<CurrencyCodeEntry> result = null;
            if (_allCurrencyCodes != null && _allCurrencyCodes.Any())
            {
                result = _allCurrencyCodes;
            }
            else
            {
                result = _iCachingProcessor.RequestAllCurrencyCodes();
            }
            return result;
        }

        /// <summary>
        /// Requests data to be displayed in the chart.
        /// Requires ChartTimePeriod, ChartCurrencyCodes.
        /// The result is stored in ChartDataPoints.
        /// </summary>
        public void RequestChartData()
        {
            ChartDataPoints = _dataProcessor.GetChartData(ChartTimePeriod, ConvertCurrencyVluesToCodes(ChartCurrencyCodeStrings));
        }

        private ICollection<CurrencyCodeEntry> ConvertCurrencyVluesToCodes(ICollection<string> input)
        {
            var allCodes = GetAllCurrencyCodes();

            return allCodes.Where(e => input.Contains(e.Value)).ToList();
        }

        /// <summary>
        /// Requests todays currencies.
        /// Currencies are specified in ChartCurrencyCodes.
        /// </summary>
        /// <returns>Currency data object.</returns>
        public void RequestTodaysCurrencies()
        {
            //TodaysCurrencies
            TodaysCurrencies = _dataProcessor.GetDailyCurrencies(DateTime.Now, ConvertCurrencyVluesToCodes(ChartCurrencyCodeStrings));
        }

        /// <summary>
        /// Adds user settings into the database.
        /// </summary>
        /// <param name="uid">The uid cookie of browser</param>
        /// <param name="userSettings">User settings.</param>
        public void SaveUserSettings(long uid, UserSettingsRequest userSettings)
        {
            _dataHolder.SaveSettings(uid, userSettings);
            
        }

        /// <summary>
        /// Requests user settings by specified IP.
        /// </summary>
        /// <returns>The user settings.</returns>
        public UserSettings RequestUserSettings(long uid)
        {
            UserSettings defaultUserSettings = _dataHolder.LoadSettings(uid);

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
        public IDictionary<CurrencyCodeEntry, ICollection<ChartCurrencyDataPoint<CurrencyDataEntry>>> ChartDataPoints { get; set; }
        public IDictionary<CurrencyCodeEntry, CurrencyDataEntry> TodaysCurrencies { get; set; }
        /// <summary>
        /// Like USD, UAH.
        /// </summary>
        public ICollection<string> ChartCurrencyCodeStrings { get; set; }
    }
}
