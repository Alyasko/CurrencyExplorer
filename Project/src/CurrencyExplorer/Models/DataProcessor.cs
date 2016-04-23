using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models
{
    public class DataProcessor
    {
        private ICachingProcessor _iCachingProcessor;

        public DataProcessor(ICachingProcessor iCachingProcessor)
        {
            _iCachingProcessor = iCachingProcessor;
        }

        /// <summary>
        /// Requests currency data for the chart within specified time period for specified currency codes.
        /// </summary>
        /// <param name="chartTimePeriod">The time period which the currency data should be returned for.</param>
        /// <param name="chartCurrencyCodes">The list of currency codes which currency data should be returned for.</param>
        /// <returns>The dictionary of currency code as key and the list of chart data points as value.</returns>
        public IDictionary<CurrencyCodeEntry, ICollection<ChartCurrencyDataPoint<CurrencyDataEntry>>> GetChartData(ChartTimePeriod chartTimePeriod, ICollection<CurrencyCodeEntry> chartCurrencyCodes)
        {
            // Request datat from caching processor.
            var currencyData = _iCachingProcessor.RequestPeriodData(chartTimePeriod, chartCurrencyCodes);

            var currencyDataPoints = new Dictionary<CurrencyCodeEntry, ICollection<ChartCurrencyDataPoint<CurrencyDataEntry>>>();

            // Format the structure of data representation.
            foreach (KeyValuePair<CurrencyCodeEntry, ICollection<CurrencyDataEntry>> pair in currencyData)
            {
                List<ChartCurrencyDataPoint<CurrencyDataEntry>> dataPoints = new List<ChartCurrencyDataPoint<CurrencyDataEntry>>();
                foreach (CurrencyDataEntry data in pair.Value)
                {
                    dataPoints.Add(new ChartCurrencyDataPoint<CurrencyDataEntry>() { DataObject = data});
                }

                currencyDataPoints.Add(pair.Key, dataPoints);
            }

            return currencyDataPoints;
        }

        /// <summary>
        /// Requests currency data for specified date for specified currency codes.
        /// </summary>
        /// <param name="date">The date which currencies should be returned on.</param>
        /// <param name="chartCurrencyCodes">The list of currency codes which currency data should be returned for.</param>
        /// <returns>The dictionary of currency code as key and currency data as value</returns>
        public IDictionary<CurrencyCodeEntry, CurrencyDataEntry> GetDailyCurrencies(DateTime date, ICollection<CurrencyCodeEntry> chartCurrencyCodes)
        {
            return _iCachingProcessor.RequestSingleData(date, chartCurrencyCodes);
        }
    }
}
