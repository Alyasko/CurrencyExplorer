using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models
{
    public class UserSettingsHolder : IUserSettingsHolder
    {
        public UserSettingsHolder(IExplorerRepository _explorerRepository)
        {
            ExplorerRepository = _explorerRepository;
        }

        public void SaveSettings(long uid, UserSettingsRequest userSettings)
        {
            UserSettingsEntry userSettingsEntry = new UserSettingsEntry();

            userSettingsEntry.ChartBeginTime = userSettings.BeginDate;
            userSettingsEntry.ChartEndTime = userSettings.EndDate;
            userSettingsEntry.CookieUid = uid;
            userSettingsEntry.CurrencyCodes = ExplorerRepository.GetCodeEntries().Where(x => userSettings.CurrencyValues.Contains(x.Value)).ToList();

            UserLanguageEntry newUserLanguageEntry = new UserLanguageEntry();
            newUserLanguageEntry.Language = userSettings.Language;

            ExplorerRepository.AddUserLanguage(newUserLanguageEntry);

            userSettingsEntry.Language = newUserLanguageEntry;

            ExplorerRepository.SaveUserSettings(userSettingsEntry);
        }

        public UserSettings LoadSettings(long uid)
        {
            return null;
        }

        public IExplorerRepository ExplorerRepository { get; set; }
    }
}
