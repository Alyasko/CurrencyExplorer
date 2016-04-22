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
    public class MsSqlCurrencyRepository : ICurrencyRepository
    {
        private CurrencyDataContext _currencyDataContext;

        public MsSqlCurrencyRepository(CurrencyDataContext currencyDataContext)
        {
            _currencyDataContext = currencyDataContext;
        }

        public IQueryable<CurrencyData> GetEntries()
        {
            var data = _currencyDataContext.CurrencyEntries.FromSql("SELECT * FROM CurrencyData");
            return data;
        }

        public IQueryable<CurrencyData> GetEntries(ChartTimePeriod timePeriod)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<CurrencyCode> GetCodeEntries()
        {
            var data = _currencyDataContext.CurrencyCodes.FromSql("SELECT * FROM CurrencyCode");
            return data;
        }

        public void AddEntry(CurrencyData currencyData)
        {
            //bool contains = false;
            //foreach (CurrencyData data in _currencyDataContext.CurrencyEntries)
            //{
            //    Debug.WriteLine($"{data.CurrencyCode}, {data.ActualDate}, {data.Name}, {data.ShortName}, {data.Value}, {data.ActualDateString}");
            //    Debug.WriteLine(
            //        $"{currencyData.CurrencyCode}, {currencyData.ActualDate}, {currencyData.Name}, {currencyData.ShortName}, {currencyData.Value}, {currencyData.ActualDateString}");

            //    if (data.Equals(currencyData))
            //    {
            //        Debug.WriteLine("Equals!");
            //    }

            //}

            var data = _currencyDataContext.CurrencyEntries.FromSql($"SELECT * FROM CurrencyData d " +
                                                                    "INNER JOIN CurrencyCode c ON c.Id = d.CurrencyCodeId " +
                                                                    "WHERE ActualDate = p0 AND c.Alias = 'p1'",
                currencyData.ActualDate, currencyData.CurrencyCode.Alias).ToList();

            if (!data.Any())
            {
                _currencyDataContext.CurrencyEntries.Add(currencyData);

                _currencyDataContext.SaveChanges();
            }
        }

        public void AddEntries(ICollection<CurrencyData> entries)
        {
            bool isEntryAdded = false;

            CurrencyData currencyData = entries.ElementAt(0);

            //var sql = "SELECT d.* FROM CurrencyData d " +
            //          "INNER JOIN CurrencyCode c ON c.Id = d.CurrencyCodeId " +
            //          $"WHERE ActualDate = STR_TO_DATE('{currencyData.ActualDate}', '%Y-%m-%d %H:%i:%s') AND c.Alias = '{currencyData.CurrencyCode.Alias}'";

            //var dat = _currencyDataContext.CurrencyEntries.FromSql(sql).ToList();


            foreach (CurrencyData entry in entries)
            {
                if (!_currencyDataContext.CurrencyEntries.Any(data => data.Equals(entry)))
                {
                    _currencyDataContext.CurrencyEntries.Add(entry);
                    isEntryAdded = true;
                }
            }

            if (isEntryAdded)
            {
                _currencyDataContext.SaveChanges();
            }
        }

        public void AddCodeEntries(ICollection<CurrencyCode> codeEntries)
        {
            bool isEntryAdded = false;

            foreach (CurrencyCode codeEntry in codeEntries)
            {
                if (!_currencyDataContext.CurrencyCodes.Any(code => code.Equals(codeEntry)))
                {
                    _currencyDataContext.CurrencyCodes.Add(codeEntry);
                    isEntryAdded = true;
                }
            }

            if (isEntryAdded)
            {
                _currencyDataContext.SaveChanges();
            }
        }

        public void AddCodeEntry(CurrencyCode codeEntry)
        {
            if (!_currencyDataContext.CurrencyCodes.Any(code => code.Equals(codeEntry)))
            {
                _currencyDataContext.CurrencyCodes.Add(codeEntry);

                _currencyDataContext.SaveChanges();
            }
        }

        public void RemoveEntries(CurrencyCode entryToRemove)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEntries(ChartTimePeriod timePeriod)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEntries(CurrencyCode entryToRemove, ChartTimePeriod timePeriod)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteQuery(string query)
        {
            throw new System.NotImplementedException();
        }
    }
}
