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
    public class CurrencyRepository : ICurrencyRepository
    {
        private CurrencyDataContext _currencyDataContext;

        public CurrencyRepository(CurrencyDataContext currencyDataContext)
        {
            _currencyDataContext = currencyDataContext;
        }

        public IQueryable<CurrencyData> GetEntries()
        {
            return _currencyDataContext.CurrencyEntries;
        }

        public IQueryable<CurrencyData> GetEntries(ChartTimePeriod timePeriod)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<CurrencyCode> GetCodeEntries()
        {
            return _currencyDataContext.CurrencyCodes;
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

            if (!_currencyDataContext.CurrencyEntries.Any(data => data.Equals(currencyData)))
            {
                _currencyDataContext.CurrencyEntries.Add(currencyData);

                _currencyDataContext.SaveChanges();
            }
        }

        public void AddEntries(ICollection<CurrencyData> entries)
        {
            bool isEntryAdded = false;

            foreach (CurrencyData entry in entries)
            {
                if (!_currencyDataContext.CurrencyEntries.Any(code => code.Equals(entry)))
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
