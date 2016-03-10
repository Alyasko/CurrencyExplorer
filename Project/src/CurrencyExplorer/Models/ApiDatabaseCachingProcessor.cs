using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models
{
    public class ApiDatabaseCachingProcessor : ICachingProcessor
    {
        private ICurrencyProvider _iCurrencyProvider;

        public ApiDatabaseCachingProcessor()
        {
            _iCurrencyProvider = null;
            Data = null;
        }

        public IDictionary<CurrencyCode, CurrencyData> RequestSingleData(DateTime timePeriod, IEnumerable<CurrencyCode> codes)
        {
            bool existsInDb = CheckDbData();
            IDictionary<CurrencyCode, CurrencyData> singleCurrencyData =
                new Dictionary<CurrencyCode, CurrencyData>();


            if (existsInDb)
            {
                // Select data from DB.
            }
            else
            {
                // Download data from API.

                foreach (CurrencyCode currencyCode in codes)
                {
                    _iCurrencyProvider.RequestCurrencyData(timePeriod, currencyCode);

                    CurrencyData data = _iCurrencyProvider.Data;

                    singleCurrencyData.Add(currencyCode, data);
                }
            }

            return singleCurrencyData;
        }

        public IDictionary<CurrencyCode, IEnumerable<CurrencyData>> RequestPeriodData(ChartTimePeriod timePeriod, IEnumerable<CurrencyCode> codes)
        {
            IDictionary<CurrencyCode, List<CurrencyData>> periodCurrencyData =
                new Dictionary<CurrencyCode, List<CurrencyData>>();

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

                foreach (var pair in currentSingleData)
                {
                    periodCurrencyData[pair.Key].Add(pair.Value);
                }
            }

            // TODO: fix IEnumerable
            return periodCurrencyData as IDictionary<CurrencyCode, IEnumerable<CurrencyData>>;
        }


        private bool CheckDbData()
        {
            return false;
        }

        public IEnumerable<IDictionary<CurrencyCode, ChartCurrencyDataPoint>> Data { get; private set; }
    }
}
