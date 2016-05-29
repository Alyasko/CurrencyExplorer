using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Models.Enums;

namespace CurrencyExplorer.Models.Contracts
{
    public interface IUserSettingsRepository
    {
        void SaveUserSettings(UserSettingsEntry userSettings);
        UserSettingsEntry LoadUserSettings(long uid);

        void AddUserLanguage(UserLanguageEntry userLanguageEntry);
        IQueryable<UserLanguageEntry> GetUserLanguages();
    }
}
