using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICorrespondenceRepository
    {
        void AddCorrespondenceEntry(CorrespondanceEntry correspondanceEntry);
        IQueryable<CorrespondanceEntry> GetCorrespondanceEntries();
        IQueryable<CorrespondanceEntry> GetCorrespondanceEntries(UserSettingsEntry userSettingsEntry);
    }
}
