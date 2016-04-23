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

            CurrencyDataEntry currencyData = entries.ElementAt(0);

            //var sql = "SELECT d.* FROM CurrencyData d " +
            //          "INNER JOIN CurrencyCode c ON c.Id = d.CurrencyCodeId " +
            //          $"WHERE ActualDate = STR_TO_DATE('{currencyData.ActualDate}', '%Y-%m-%d %H:%i:%s') AND c.Alias = '{currencyData.CurrencyCode.Alias}'";

            //var dat = _currencyDataContext.CurrencyEntries.FromSql(sql).ToList();

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
    }
}
