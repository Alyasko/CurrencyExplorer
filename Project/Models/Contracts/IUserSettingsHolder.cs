using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models.Contracts
{
    public interface IUserSettingsHolder
    {
        void SaveSettings(long uid, UserSettingsRequest userSettings);
        UserSettings LoadSettings(long uid);
    }
}
