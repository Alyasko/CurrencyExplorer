﻿using System;
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

        private IQueryable<CurrencyData> TryGetDatabaseData(DateTime time, CurrencyCode code)
        {
            var dbEntries = _currencyRepository.GetEntries().Where(data => data.ActualDate.Date == time.Date && code.Equals(data.CurrencyCode));

            return dbEntries.Any() ? dbEntries : null;
        }

        public IDictionary<CurrencyCode, CurrencyData> RequestSingleData(DateTime time, ICollection<CurrencyCode> codes)
        {
            IDictionary<CurrencyCode, CurrencyData> requiredSingleCurrencyData = null;

            var dbCodes = _currencyRepository.GetCodeEntries();

            foreach (CurrencyCode code in codes)
            {
                // Array of Data is received.
                var dbEntries = TryGetDatabaseData(time, code);

                if (dbEntries != null)
                {
                    // We have data in database
                    if (requiredSingleCurrencyData == null)
                    {
                        requiredSingleCurrencyData = new Dictionary<CurrencyCode, CurrencyData>();
                    }

                    requiredSingleCurrencyData.Add(code, dbEntries.First());
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

                        break;
                    }
                }
            }

            return requiredSingleCurrencyData;
        }

        public IDictionary<CurrencyCode, ICollection<CurrencyData>> RequestPeriodData(ChartTimePeriod timePeriod, ICollection<CurrencyCode> codes)
        {
            IDictionary<CurrencyCode, ICollection<CurrencyData>> periodCurrencyData =
                new Dictionary<CurrencyCode, ICollection<CurrencyData>>();

            DateTime beginTime = SelectWorkingDate(timePeriod.Begin, DateSelection.BeforeWeekends);
            DateTime endTime = SelectWorkingDate(timePeriod.End, DateSelection.AfterWeekends);

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

            return periodCurrencyData;
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
