using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Enums;
using CurrencyExplorer.Models.Repositories;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models
{
    public class ApiDatabaseCachingProcessor : ICachingProcessor
    {
        private ICurrencyProvider _currencyProvider;
        private ICollection<DayOfWeek> _weekends;
        private ICurrencyRepository _currencyRepository;

        public ApiDatabaseCachingProcessor(
            ICurrencyProvider iCurrencyProvider,
            ICurrencyRepository iCurrencyRepository)
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
            ICurrencyRepository iCurrencyRepository,
            ICollection<DayOfWeek> weekends)
            : this(iCurrencyProvider, iCurrencyRepository)
        {
            // Override weekends.
            _weekends = weekends;
        }

        private IDictionary<CurrencyCode, CurrencyData> RequestSingleData(DateTime time)
        {
            // TODO: FIX. Do we need this method?
            IDictionary<CurrencyCode, CurrencyData> result = null;
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

        private CurrencyData TryGetSingleDatabaseData(DateTime time, CurrencyCode code)
        {
            CurrencyData result = null;

            var dbEntries = _currencyRepository.GetEntries().Where(data => data.ActualDate.Date == time.Date && code.Equals(data.CurrencyCode));
            //var dbCode = _currencyRepository.GetCodeEntries().First(x => x.Equals(code));

            if (dbEntries.Any())
            {
                result = dbEntries.First();
                result.CurrencyCode = code;
            }

            return result;
        }

        public IDictionary<CurrencyCode, CurrencyData> RequestSingleData(DateTime time, ICollection<CurrencyCode> codes, bool useCaching = true)
        {
            IDictionary<CurrencyCode, CurrencyData> requiredSingleCurrencyData = null;

            var dbCodes = _currencyRepository.GetCodeEntries();

            foreach (CurrencyCode code in codes)
            {
                // Array of Data is received.
                var dbEntry = TryGetSingleDatabaseData(time, code);

                if (dbEntry != null)
                {
                    // We have data in database
                    if (requiredSingleCurrencyData == null)
                    {
                        requiredSingleCurrencyData = new Dictionary<CurrencyCode, CurrencyData>();
                    }

                    requiredSingleCurrencyData.Add(code, dbEntry);

                    Debug.WriteLine($"{DateTime.Now}: data acquired from database.");
                }
                else
                {
                    // No data in database.
                    IDictionary<CurrencyCode, CurrencyData> allCurrencyDataPerDay = null;
                    
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

                        foreach (KeyValuePair<CurrencyCode, CurrencyData> currencyData in allCurrencyDataPerDay)
                        {
                            currencyData.Value.CurrencyCode = codesFromDb.First(x => x.Equals(currencyData.Key));
                        }

                        _currencyRepository.AddEntries(allCurrencyDataPerDay.Values);

                        // Save acquired data.
                        requiredSingleCurrencyData = allCurrencyDataPerDay.Where(x => codes.Contains(x.Key)).ToDictionary(k => k.Key, v => v.Value);

                        Debug.WriteLine($"{DateTime.Now}: data acquired from API.");

                        break;
                    }
                }
            }

            return requiredSingleCurrencyData;
        }

        private ICollection<CurrencyData> TryGetPeriodDatabaseData(DateTime beginTime, DateTime endTime, CurrencyCode code)
        {
            ICollection<CurrencyData> result = null;

            var dbEntries = _currencyRepository.GetEntries().Where(data => 
                data.ActualDate.Date >= beginTime.Date &&
                data.ActualDate.Date <= endTime.Date &&
                data.CurrencyCode.Equals(code));

            if (dbEntries.Any())
            {
                result = dbEntries.ToList();

                foreach (CurrencyData data in result)
                {
                    data.CurrencyCode = code;
                }
            }

            return result;
        }

        public IDictionary<CurrencyCode, ICollection<CurrencyData>> RequestPeriodData(ChartTimePeriod timePeriod, ICollection<CurrencyCode> codes)
        {
            IDictionary<CurrencyCode, ICollection<CurrencyData>> periodCurrencyData =
                new Dictionary<CurrencyCode, ICollection<CurrencyData>>();

            DateTime beginTime = SelectWorkingDate(timePeriod.Begin, DateSelection.BeforeWeekends);
            DateTime endTime = SelectWorkingDate(timePeriod.End, DateSelection.AfterWeekends);


            foreach (CurrencyCode code in codes)
            {
                // Try get from database.
                var dbEntries = TryGetPeriodDatabaseData(beginTime, endTime, code);

                List<CurrencyData> entries = null;

                if (dbEntries == null)
                {
                    entries = new List<CurrencyData>();
                }
                else
                {
                    entries = dbEntries.ToList();
                }

                List<CurrencyData> correctedEntries = new List<CurrencyData>();

                // Add already acquired entries from database.
                correctedEntries.AddRange(entries);

                CurrencyData lastSingleData = null;
                CurrencyData tempSingleData = null;

                // Check currency data per date.
                for (DateTime iterator = beginTime; iterator < endTime; iterator = iterator.AddDays(1))
                {
                    tempSingleData = correctedEntries.FirstOrDefault(x => x.ActualDate.Date == iterator.Date);

                    if (
                        !correctedEntries.Exists(
                            p => p.ActualDate.Date == iterator.Date && p.CurrencyCode.Equals(code)))
                    {
                        if (_weekends.Contains(iterator.DayOfWeek))
                        {
                            if (lastSingleData != null)
                            {
                                CurrencyData newCurrencyData = lastSingleData.Clone();
                                newCurrencyData.ActualDateString =
                                    $"{iterator.Day:00}.{iterator.Month:00}.{iterator.Year:0000}";
                                correctedEntries.Add(newCurrencyData);
                            }
                            continue;
                        }

                        // If database doesn't have record for this date.
                        var downloadedData = RequestSingleData(iterator, new CurrencyCode[] {code}, false);

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

        public ICollection<CurrencyCode> RequestAllCurrencyCodes()
        {
            ICollection<CurrencyCode> result = null;

            IDictionary<CurrencyCode, CurrencyData> responce = RequestSingleData(DateTime.Now);

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


        public IEnumerable<IDictionary<CurrencyCode, ChartCurrencyDataPoint<CurrencyData>>> Data { get; private set; }
    }
}
