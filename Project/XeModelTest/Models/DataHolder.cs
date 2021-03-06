﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Models.Enums;
using CurrencyExplorer.Utilities;
using XeModelTest;

namespace CurrencyExplorer.Models
{
    public class DataHolder
    {
        public DataHolder()
        {
            
        }

        public DataHolder(ICachingProcessor cachingProcessor, IUserSettingsHolder userSettingsHolder, IConfiguration configuration)
        {
            CachingProcessor = cachingProcessor;
            UserSettingsHolder = userSettingsHolder;
            Configuration = configuration;
        }

        public UserSettings LoadSettings(long uid)
        {
            UserSettings userSettings = UserSettingsHolder.LoadSettings(uid);

            if (userSettings == null)
            {
                // There is no UserSettings for specified cookie.
                userSettings = LoadDefaultSettings();
            }

            // Fill currenciesData for todays info.

            IDictionary<CurrencyCodeEntry, CurrencyDataEntry> currenciesData = null;

            DateTime iterator = DateTime.Now;
            while (currenciesData == null)
            {
                currenciesData = CachingProcessor.RequestSingleData(iterator, userSettings.Currencies.Select(x => x.DbCurrencyCodeEntry).ToList());
                iterator = iterator.Subtract(TimeSpan.FromDays(1));
            }

            userSettings.Currencies = currenciesData.Select((pair) => pair.Value).ToList();

            return userSettings;
        }

        private UserSettings LoadDefaultSettings()
        {
            UserSettings defaultUserSettings = new UserSettings();

            defaultUserSettings.Language = (CurrencyExplorerLanguage)Enum.Parse(typeof(CurrencyExplorerLanguage), "");

            // Process default period.

            ChartTimePeriod timePeriod = new ChartTimePeriod();

            string configPeriodInDays = "";//Configuration["ExplorerSettings:Defaults:ChartPeriodInDays"];
            double periodInDays = 10;
            if (double.TryParse(configPeriodInDays, out periodInDays) == false)
            {
                Debug.WriteLine($"Incorrect ChartPeriodInDays value from configuration file. Default valye is set.");
            }

            timePeriod.Begin = DateTime.Now.Subtract(TimeSpan.FromDays(periodInDays));
            timePeriod.End = DateTime.Now;
            defaultUserSettings.TimePeriod = timePeriod;

            // Get currencies list to be displayed.

            var codesList = Utils.GetListFromConfiguration(Configuration, "ExplorerSettings:Defaults:CurrenciesList");

            List<CurrencyCodeEntry> defaultCodes = new List<CurrencyCodeEntry>();

            var allCodes = CachingProcessor.RequestAllCurrencyCodes().Distinct().ToList();

            defaultCodes.AddRange(allCodes.Where(x => codesList.Contains(x.Alias)));

            defaultUserSettings.Currencies = defaultCodes.Select(x => new CurrencyDataEntry() {DbCurrencyCodeEntry = x});

            return defaultUserSettings;
        }


        public void SaveSettings(long uid, UserSettingsRequest userSettings)
        {
            UserSettingsHolder.SaveSettings(uid, userSettings);
        }

        public IConfiguration Configuration { get; set; }
        public IUserSettingsHolder UserSettingsHolder { get; set; }
        public ICachingProcessor CachingProcessor { get; set; }
    }
}
