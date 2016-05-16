using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Models.Enums;
using CurrencyExplorer.Utilities;
using Microsoft.Extensions.Configuration;

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
            UserSettings userSettings = UserSettingsHolder.LoadSettings(uid); ;

            if (userSettings == null)
            {
                // There is no UserSettings for specified cookie.
                userSettings = LoadDefaultSettings();
            }

            return userSettings;
        }

        private UserSettings LoadDefaultSettings()
        {
            UserSettings defaultUserSettings = new UserSettings();

            defaultUserSettings.Language = (CurrencyExplorerLanguage)Enum.Parse(typeof(CurrencyExplorerLanguage), Configuration["ExplorerSettings:Defaults:Language"]);

            // Process default period.

            ChartTimePeriod timePeriod = new ChartTimePeriod();

            string configPeriodInDays = Configuration["ExplorerSettings:Defaults:ChartPeriodInDays"];
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

            var allCodes = CachingProcessor.RequestAllCurrencyCodes();

            defaultCodes.AddRange(allCodes.Where(x => codesList.Contains(x.Alias)));

            // Fille currenciesData for todays info.

            IDictionary<CurrencyCodeEntry, CurrencyDataEntry> currenciesData = null;

            DateTime iterator = DateTime.Now;
            while (currenciesData == null)
            {
                currenciesData = CachingProcessor.RequestSingleData(iterator, defaultCodes);
                iterator = iterator.Subtract(TimeSpan.FromDays(1));
            }

            defaultUserSettings.Currencies = currenciesData.Select((pair) => pair.Value).ToList();

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
