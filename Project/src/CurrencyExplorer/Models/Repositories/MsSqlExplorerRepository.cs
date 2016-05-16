using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using Microsoft.Data.Entity;

namespace CurrencyExplorer.Models.Repositories
{
    public class MsSqlExplorerRepository : IExplorerRepository
    {
        private CurrencyDataContext _currencyDataContext;

        public MsSqlExplorerRepository(CurrencyDataContext currencyDataContext)
        {
            _currencyDataContext = currencyDataContext;
        }

        public IQueryable<CurrencyDataEntry> GetDataEntries()
        {
            return _currencyDataContext.CurrencyDataEntries;
        }

        public IQueryable<CurrencyDataEntry> GetDataEntries(ChartTimePeriod timePeriod)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<CurrencyCodeEntry> GetCodeEntries()
        {
            return _currencyDataContext.CurrencyCodesEntries;
        }

        public void AddDataEntry(CurrencyDataEntry currencyData)
        {
            var data = _currencyDataContext.CurrencyDataEntries.Where(x => x.Equals(currencyData));

            if (!data.Any())
            {
                _currencyDataContext.CurrencyDataEntries.Add(currencyData);

                _currencyDataContext.SaveChanges();
            }
        }

        public void AddDataEntries(ICollection<CurrencyDataEntry> entries)
        {
            bool isEntryAdded = false;

            var allEntries = _currencyDataContext.CurrencyDataEntries.ToList();

            foreach (CurrencyDataEntry entry in entries)
            {
                if (!allEntries.Any(data => data.Equals(entry)))
                {
                    _currencyDataContext.CurrencyDataEntries.Add(entry);
                    isEntryAdded = true;
                }
            }

            if (isEntryAdded)
            {
                _currencyDataContext.SaveChanges();
            }
        }

        public void AddCodeEntries(ICollection<CurrencyCodeEntry> codeEntries)
        {
            bool isEntryAdded = false;

            foreach (CurrencyCodeEntry codeEntry in codeEntries)
            {
                if (!_currencyDataContext.CurrencyCodesEntries.Any(code => code.Equals(codeEntry)))
                {
                    _currencyDataContext.CurrencyCodesEntries.Add(codeEntry);
                    isEntryAdded = true;
                }
            }

            if (isEntryAdded)
            {
                _currencyDataContext.SaveChanges();
            }
        }

        public void AddCodeEntry(CurrencyCodeEntry codeEntryEntry)
        {
            if (!_currencyDataContext.CurrencyCodesEntries.Any(code => code.Equals(codeEntryEntry)))
            {
                _currencyDataContext.CurrencyCodesEntries.Add(codeEntryEntry);

                _currencyDataContext.SaveChanges();
            }
        }

        public void RemoveDataEntries(CurrencyCodeEntry entryToRemove)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveDataEntries(ChartTimePeriod timePeriod)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveDataEntries(CurrencyCodeEntry entryToRemove, ChartTimePeriod timePeriod)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteQuery(string query)
        {
            throw new System.NotImplementedException();
        }

        public void SaveUserSettings(UserSettingsEntry userSettings)
        {
            var data = _currencyDataContext.UserSettingsEntries.Where(x => x.Equals(userSettings));

            if (!data.Any())
            {
                _currencyDataContext.UserSettingsEntries.Add(userSettings);

                _currencyDataContext.SaveChanges();
            }
            else
            {
                UserSettingsEntry dbEntry = data.FirstOrDefault();
                if (dbEntry != null)
                {
                    dbEntry.ChartBeginTime = userSettings.ChartBeginTime;
                    dbEntry.ChartEndTime = userSettings.ChartEndTime;
                    dbEntry.CurrencyCodes = userSettings.CurrencyCodes;
                    dbEntry.Language = userSettings.Language;

                    _currencyDataContext.UserSettingsEntries.Update(dbEntry);

                    _currencyDataContext.SaveChanges();
                }
            }
        }

        public UserSettingsEntry LoadUserSettings(long uid)
        {
            throw new NotImplementedException();
        }

        public void AddUserLanguage(UserLanguageEntry userLanguageEntry)
        {
            var data = _currencyDataContext.UserLanguageEntries.Where(x => x.Equals(userLanguageEntry));

            if (!data.Any())
            {
                _currencyDataContext.UserLanguageEntries.Add(userLanguageEntry);

                _currencyDataContext.SaveChanges();
            }
        }

        public void GetUserLanguages()
        {
            throw new NotImplementedException();
        }
    }
}
