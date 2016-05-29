using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Models.Enums;

namespace CurrencyExplorer.Models
{
    public class UserSettingsHolder : IUserSettingsHolder
    {
        public UserSettingsHolder(IExplorerRepository _explorerRepository)
        {
            ExplorerRepository = _explorerRepository;
        }

        /// <summary>
        /// Saves user settings.
        /// </summary>
        /// <param name="uid">The uid of user.</param>
        /// <param name="userSettings">The user settings to be saved.</param>
        public void SaveSettings(long uid, UserSettingsRequest userSettings)
        {
            UserSettingsEntry userSettingsEntry = new UserSettingsEntry();

            userSettingsEntry.ChartBeginTime = userSettings.BeginDate;
            userSettingsEntry.ChartEndTime = userSettings.EndDate;
            userSettingsEntry.CookieUid = uid;

            ExplorerRepository.RemoveCorrespondanceEntries(userSettingsEntry);
            //userSettingsEntry.CurrencyCodes = ExplorerRepository.GetCodeEntries().Where(x => userSettings.CurrencyValues.Contains(x.Value)).ToList();

            UserLanguageEntry newUserLanguageEntry = new UserLanguageEntry();
            newUserLanguageEntry.Language = userSettings.Language.ToString();
            ExplorerRepository.AddUserLanguage(newUserLanguageEntry);

            userSettingsEntry.Language = newUserLanguageEntry;

            ExplorerRepository.SaveUserSettings(userSettingsEntry);

            var correspCurrencyCodes =
                ExplorerRepository.GetCodeEntries().Where(x => userSettings.CurrencyValues.Contains(x.Value)).ToList();

            foreach (CurrencyCodeEntry currencyCode in correspCurrencyCodes)
            {
                CorrespondanceEntry correspondanceEntry = new CorrespondanceEntry();
                correspondanceEntry.UserSettings = userSettingsEntry;
                correspondanceEntry.CurrencyCode = currencyCode;

                ExplorerRepository.AddCorrespondenceEntry(correspondanceEntry);
            }
        }

        /// <summary>
        /// Loads user settings.
        /// </summary>
        /// <param name="uid">The uid of user.</param>
        /// <returns>User settings for specified user uid.</returns>
        public UserSettings LoadSettings(long uid)
        {
            UserSettings userSettings = null;

            var langs = ExplorerRepository.GetUserLanguages().ToList();
            UserSettingsEntry userSettingsEntry = ExplorerRepository.LoadUserSettings(uid);

            if (userSettingsEntry != null)
            {
                userSettings = new UserSettings();

                userSettings.Language = (CurrencyExplorerLanguage)Enum.Parse(typeof(CurrencyExplorerLanguage), userSettingsEntry.Language.Language);
                userSettings.TimePeriod = new ChartTimePeriod(userSettingsEntry.ChartBeginTime, userSettingsEntry.ChartEndTime);

                userSettings.Currencies =
                    ExplorerRepository.GetCorrespondanceEntries(userSettingsEntry).Select(x => new CurrencyDataEntry() { DbCurrencyCodeEntry = x.CurrencyCode });
            }

            return userSettings;
        }

        public IExplorerRepository ExplorerRepository { get; set; }
    }
}
