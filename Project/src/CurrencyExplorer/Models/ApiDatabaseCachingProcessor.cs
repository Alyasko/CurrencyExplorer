using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Models.Enums;
using CurrencyExplorer.Models.Repositories;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models
{
    public class ApiDatabaseCachingProcessor : ICachingProcessor
    {
        private ICurrencyProvider _currencyProvider;
        private ICollection<DayOfWeek> _weekends;
        private IExplorerRepository _currencyRepository;

        public ApiDatabaseCachingProcessor(
            ICurrencyProvider iCurrencyProvider,
            IExplorerRepository iCurrencyRepository)
        {
            _currencyProvider = iCurrencyProvider;
            _currencyRepository = iCurrencyRepository;

            _weekends = new List<DayOfWeek>() { DayOfWeek.Sunday, DayOfWeek.Saturday };

            Data = null;

            var allCodes = RequestAllCurrencyCodes();
            //CurrencyRepository.AddCodeEntries(allCodes);
        }

        public ApiDatabaseCachingProcessor(
            ICurrencyProvider iCurrencyProvider,
            IExplorerRepository iCurrencyRepository,
            ICollection<DayOfWeek> weekends)
            : this(iCurrencyProvider, iCurrencyRepository)
        {
            // Override weekends.
            _weekends = weekends;
        }

        private IDictionary<CurrencyCodeEntry, CurrencyDataEntry> RequestSingleData(DateTime time)
        {
            // TODO: FIX. Do we need this method?
            IDictionary<CurrencyCodeEntry, CurrencyDataEntry> result = null;
            try
            {
                result = _currencyProvider.RequestSingleCurrencyData(time);
            }
            catch (NoItemsException e)
            {
                Debug.WriteLine($"Time: {time.ToString()}, Message: {e.Message}");
            }

            return result;
        }

        private CurrencyDataEntry TryGetSingleDatabaseData(DateTime time, CurrencyCodeEntry codeEntry)
        {
            CurrencyDataEntry result = null;

            var dbEntries = _currencyRepository.GetDataEntries().Where(data => data.ActualDate.Date == time.Date && codeEntry.Equals(data.DbCurrencyCodeEntry));
            //var dbCode = _currencyRepository.GetCodeEntries().First(x => x.Equals(code));

            if (dbEntries.Any())
            {
                result = dbEntries.First();
                result.DbCurrencyCodeEntry = codeEntry;
            }

            return result;
        }

        public IDictionary<CurrencyCodeEntry, CurrencyDataEntry> RequestSingleData(DateTime time, ICollection<CurrencyCodeEntry> codes, bool useCaching = true)
        {
            IDictionary<CurrencyCodeEntry, CurrencyDataEntry> requiredSingleCurrencyData = null;

            foreach (CurrencyCodeEntry code in codes)
            {
                // Array of Data is received.
                var dbEntry = TryGetSingleDatabaseData(time, code);

                if (dbEntry != null)
                {
                    // We have data in database
                    if (requiredSingleCurrencyData == null)
                    {
                        requiredSingleCurrencyData = new Dictionary<CurrencyCodeEntry, CurrencyDataEntry>();
                    }

                    requiredSingleCurrencyData.Add(code, dbEntry);

                    Debug.WriteLine($"{DateTime.Now}: data acquired from database.");
                }
                else
                {
                    // No data in database.
                    IDictionary<CurrencyCodeEntry, CurrencyDataEntry> allCurrencyDataPerDay = null;
                    
                    // Download data.
                    try
                    {
                        allCurrencyDataPerDay = _currencyProvider.RequestSingleCurrencyData(time);
                    }
                    catch (NoItemsException e)
                    {
                        Debug.WriteLine($"Time: {time.ToString()}, Message: {e.Message}");
                    }

                    // If it has returned something.
                    if (allCurrencyDataPerDay != null)
                    {
                        // Add to the database.
                        _currencyRepository.AddCodeEntries(allCurrencyDataPerDay.Keys);

                        var codesFromDb = _currencyRepository.GetCodeEntries();

                        foreach (KeyValuePair<CurrencyCodeEntry, CurrencyDataEntry> currencyData in allCurrencyDataPerDay)
                        {
                            currencyData.Value.DbCurrencyCodeEntry = codesFromDb.First(x => x.Equals(currencyData.Key));
                        }

                        _currencyRepository.AddDataEntries(allCurrencyDataPerDay.Values);

                        // Save acquired data.
                        requiredSingleCurrencyData = allCurrencyDataPerDay.Where(x => codes.Contains(x.Key)).ToDictionary(k => k.Key, v => v.Value);

                        Debug.WriteLine($"{DateTime.Now}: data acquired from API.");

                        break;
                    }
                }
            }

            return requiredSingleCurrencyData;
        }

        private ICollection<CurrencyDataEntry> TryGetPeriodDatabaseData(DateTime beginTime, DateTime endTime, CurrencyCodeEntry codeEntry)
        {
            ICollection<CurrencyDataEntry> result = null;

            var dbEntries = _currencyRepository.GetDataEntries().Where(data => 
                data.ActualDate.Date >= beginTime.Date &&
                data.ActualDate.Date <= endTime.Date &&
                data.DbCurrencyCodeEntry.Equals(codeEntry));

            if (dbEntries.Any())
            {
                result = dbEntries.ToList();

                foreach (CurrencyDataEntry data in result)
                {
                    data.DbCurrencyCodeEntry = codeEntry;
                }
            }

            return result;
        }

        public IDictionary<CurrencyCodeEntry, ICollection<CurrencyDataEntry>> RequestPeriodData(ChartTimePeriod timePeriod, ICollection<CurrencyCodeEntry> codes)
        {
            IDictionary<CurrencyCodeEntry, ICollection<CurrencyDataEntry>> periodCurrencyData =
                new Dictionary<CurrencyCodeEntry, ICollection<CurrencyDataEntry>>();

            DateTime beginTime = SelectWorkingDate(timePeriod.Begin, DateSelection.BeforeWeekends);
            DateTime endTime = SelectWorkingDate(timePeriod.End, DateSelection.AfterWeekends);


            foreach (CurrencyCodeEntry code in codes)
            {
                // Try get from database.
                var dbEntries = TryGetPeriodDatabaseData(beginTime, endTime, code);

                List<CurrencyDataEntry> correctedEntries = null;

                if (dbEntries == null)
                {
                    correctedEntries = new List<CurrencyDataEntry>();
                }
                else
                {
                    correctedEntries = dbEntries.ToList();
                }

                CurrencyDataEntry lastSingleData = null;
                CurrencyDataEntry tempSingleData = null;

                // Check currency data per date.
                for (DateTime iterator = beginTime; iterator < endTime; iterator = iterator.AddDays(1))
                {
                    tempSingleData = correctedEntries.FirstOrDefault(x => x.ActualDate.Date == iterator.Date);

                    if (
                        !correctedEntries.Exists(
                            p => p.ActualDate.Date == iterator.Date && p.DbCurrencyCodeEntry.Equals(code)))
                    {
                        if (_weekends.Contains(iterator.DayOfWeek))
                        {
                            if (lastSingleData != null)
                            {
                                CurrencyDataEntry newCurrencyData = lastSingleData.Clone();
                                newCurrencyData.ActualDate = iterator;
                                correctedEntries.Add(newCurrencyData);
                            }
                            continue;
                        }

                        // If database doesn't have record for this date.
                        var downloadedData = RequestSingleData(iterator, new CurrencyCodeEntry[] {code}, false);

                        if (tempSingleData == null)
                        {
                            tempSingleData = lastSingleData;
                        }

                        if (downloadedData != null)
                        {
                            correctedEntries.Add(downloadedData[code]);
                        }


                    }

                    lastSingleData = tempSingleData;
                }

                correctedEntries.Sort(
                    (d1, d2) => d1.ActualDate > d2.ActualDate ? 1 : (d1.ActualDate == d2.ActualDate ? 0 : -1));

                periodCurrencyData.Add(code, correctedEntries);

            }

            return periodCurrencyData;

/*
            //var mData = TryGetPeriodDatabaseData(beginTime, endTime, codes.ElementAt(0)).ToList();

            // Format the keys of data structure to be returned.
            foreach (CurrencyCode code in codes)
            {
                periodCurrencyData.Add(code, new List<CurrencyData>());
            }

            IDictionary<CurrencyCode, CurrencyData> lastSingleData = null;
            IDictionary<CurrencyCode, CurrencyData> tempSingleData = null;

            // TODO: fix iterator to make possible to change the step of selection so that select less data in large queries.
            for (DateTime iterator = beginTime; iterator < endTime; iterator = iterator.AddDays(1))
            {
                // TODO: select using one query instead of query per day.

                tempSingleData = this.RequestSingleData(iterator, codes);

                if (tempSingleData == null)
                {
                    tempSingleData = lastSingleData;
                }

                if (tempSingleData != null)
                {
                    foreach (var pair in tempSingleData)
                    {
                        periodCurrencyData[pair.Key].Add(pair.Value);
                    }
                }
                else
                {
                    throw new Exception("Unhandled NULL reference exception. The previous data does not");
                }


                lastSingleData = tempSingleData;
            }

            return periodCurrencyData;*/
            }

        private DateTime SelectWorkingDate(DateTime dateTime, DateSelection selectionType)
        {
            DateTime selectedDateTime = dateTime;

            switch (selectionType)
            {
                case DateSelection.AfterWeekends:
                    if (_weekends.Contains(DateTime.Now.DayOfWeek))
                    {
                        selectedDateTime = SelectWorkingDate(dateTime, DateSelection.BeforeWeekends);
                    }
                    else
                    {
                        while (_weekends.Contains(selectedDateTime.DayOfWeek))
                        {
                            if (selectedDateTime >= DateTime.Now)
                            {
                                selectedDateTime = dateTime;
                            }
                            else
                            {
                                selectedDateTime = selectedDateTime.AddDays(1);
                            }
                        }
                    }
                    break;
                case DateSelection.BeforeWeekends:
                    while (_weekends.Contains(selectedDateTime.DayOfWeek))
                    {
                        selectedDateTime = selectedDateTime.AddDays(-1);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectionType), selectionType, null);
            }

            return selectedDateTime;
        }

        public ICollection<CurrencyCodeEntry> RequestAllCurrencyCodes()
        {
            ICollection<CurrencyCodeEntry> result = null;

            IDictionary<CurrencyCodeEntry, CurrencyDataEntry> responce = RequestSingleData(DateTime.Now);

            DateTime startDate = DateTime.Now;

            while (responce == null)
            {
                responce = RequestSingleData(startDate);
                startDate = startDate.Subtract(TimeSpan.FromDays(1));
            }

            if (responce.Count != 0)
            {
                // Elements are the same.
                result = responce.Select(p => p.Key).ToList();
            }

            return result;
        }


        public IEnumerable<IDictionary<CurrencyCodeEntry, ChartCurrencyDataPoint<CurrencyDataEntry>>> Data { get; private set; }
    }
}
