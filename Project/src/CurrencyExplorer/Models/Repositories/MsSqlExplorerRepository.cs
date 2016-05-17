using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Models.Enums;
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
            //var all = _currencyDataContext.UserSettingsEntries.ToArray();
            var langs = _currencyDataContext.UserLanguageEntries.ToArray();
            UserSettingsEntry data = _currencyDataContext.UserSettingsEntries.FirstOrDefault(x => x.Equals(userSettings));

            if (data == null)
            {
                _currencyDataContext.UserSettingsEntries.Add(userSettings);

                _currencyDataContext.SaveChanges();
            }
            else
            {
                // BUG: Updating is not implemented.

                /*data.ChartBeginTime = userSettings.ChartBeginTime;
                data.ChartEndTime = userSettings.ChartEndTime;
                data.Language = userSettings.Language;

                //_currencyDataContext.Entry(data).State = EntityState.Modified;

                _currencyDataContext.Update(data);

                _currencyDataContext.SaveChanges();*/

                // DANGEROUS!
                userSettings.Id = data.Id;
            }
        }

        public UserSettingsEntry LoadUserSettings(long uid)
        {
            UserSettingsEntry userSettingsEntry =
                _currencyDataContext.UserSettingsEntries.FirstOrDefault(x => x.CookieUid == uid);

            return userSettingsEntry;
        }

        public void AddUserLanguage(UserLanguageEntry userLanguageEntry)
        {
            UserLanguageEntry dbEntry = _currencyDataContext.UserLanguageEntries.FirstOrDefault(x => x.Equals(userLanguageEntry));

            if (dbEntry == null)
            {
                _currencyDataContext.UserLanguageEntries.Add(userLanguageEntry);

                _currencyDataContext.SaveChanges();
            }
            else
            {
                // DANGEROUS!
                userLanguageEntry.Id = dbEntry.Id;
            }
        }

        public IQueryable<UserLanguageEntry> GetUserLanguages()
        {
            return _currencyDataContext.UserLanguageEntries;
        }


        public void AddCorrespondenceEntry(CorrespondanceEntry correspondanceEntry)
        {
            var data = _currencyDataContext.CorrespondanceEntries.ToList().FirstOrDefault(x => x.Equals(correspondanceEntry));
            //CorrespondanceEntry data = _currencyDataContext.CorrespondanceEntries.FirstOrDefault(x => x.Equals(correspondanceEntry));

            if (data == null)
            {
                _currencyDataContext.CorrespondanceEntries.Add(correspondanceEntry);

                _currencyDataContext.SaveChanges();
            }
        }

        public IQueryable<CorrespondanceEntry> GetCorrespondanceEntries()
        {
            return _currencyDataContext.CorrespondanceEntries;
        }

        public IQueryable<CorrespondanceEntry> GetCorrespondanceEntries(UserSettingsEntry userSettingsEntry)
        {
            return _currencyDataContext.CorrespondanceEntries.Where(x => x.UserSettings.Equals(userSettingsEntry));
        }
    }
}
