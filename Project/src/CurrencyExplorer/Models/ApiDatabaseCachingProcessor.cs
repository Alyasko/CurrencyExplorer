using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models
{
    public class ApiDatabaseCachingProcessor : ICachingProcessor
    {

        public ApiDatabaseCachingProcessor(ICurrencyProvider iCurrencyProvider)
        {
            CurrencyProvider = iCurrencyProvider;
            Data = null;
        }

        private IDictionary<CurrencyCode, CurrencyData> RequestSingleData(DateTime time)
        {
            IDictionary<CurrencyCode, CurrencyData> result = null;
            try
            {
                result = CurrencyProvider.RequestCurrencyData(time);
            }
            catch (NoItemsException e)
            {
                Debug.WriteLine($"Time: {time.ToString()}, Message: {e.Message}");
            }

            return result;
        }

        public IDictionary<CurrencyCode, CurrencyData> RequestSingleData(DateTime timePeriod, ICollection<CurrencyCode> codes)
        {
            bool existsInDb = CheckDbData();
            IDictionary<CurrencyCode, CurrencyData> requiredSingleCurrencyData = null;

            if (codes == null)
            {
                throw new NullReferenceException("Currency codes dictionary is null.");
            }

            if (existsInDb)
            {
                // Select data from DB.

                // TODO: implement selection from DB.
            }
            else
            {
                // Download data from API.

                var allCurrencyDataPerDay = RequestSingleData(timePeriod);

                if (allCurrencyDataPerDay != null)
                {
                    requiredSingleCurrencyData =
                        allCurrencyDataPerDay.Where(x => codes.Contains(x.Key)).ToDictionary(k => k.Key, v => v.Value);
                }

                // TODO: save allCurrencyDataPerDay to the database.

            }

            return requiredSingleCurrencyData;
        }

        public IDictionary<CurrencyCode, ICollection<CurrencyData>> RequestPeriodData(ChartTimePeriod timePeriod, ICollection<CurrencyCode> codes)
        {
            IDictionary<CurrencyCode, ICollection<CurrencyData>> periodCurrencyData =
                new Dictionary<CurrencyCode, ICollection<CurrencyData>>();

            DateTime beginTime = timePeriod.Begin;
            DateTime endTime = timePeriod.End;

            foreach (CurrencyCode code in codes)
            {
                periodCurrencyData.Add(code, new List<CurrencyData>());
            }

            // TODO: fix iterator to make possible to change the step of selection so that select less data in large queries.
            for (DateTime iterator = beginTime; iterator < endTime; iterator = iterator.AddDays(1))
            {
                IDictionary<CurrencyCode, CurrencyData> currentSingleData = this.RequestSingleData(iterator, codes);

                if (currentSingleData != null)
                {
                    foreach (var pair in currentSingleData)
                    {
                        periodCurrencyData[pair.Key].Add(pair.Value);
                    }
                }
            }

            return periodCurrencyData;
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
                result = responce.Select(p => p.Key).ToList();
            }

            return result;
        }


        private bool CheckDbData()
        {
            return false;
        }

        public IEnumerable<IDictionary<CurrencyCode, ChartCurrencyDataPoint>> Data { get; private set; }

        public ICurrencyProvider CurrencyProvider { get; }
    }
}
