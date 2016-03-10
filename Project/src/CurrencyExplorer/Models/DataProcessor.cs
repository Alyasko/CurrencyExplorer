using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models
{
    public class DataProcessor
    {
        private ICachingProcessor _iCachingProcessor;

        public DataProcessor()
        {
            _iCachingProcessor = null;
        }

        /// <summary>
        /// Requests currency data for the chart within specified time period for specified currency codes.
        /// </summary>
        /// <param name="chartTimePeriod">The time period which the currency data should be returned for.</param>
        /// <param name="chartCurrencyCodes">The list of currency codes which currency data should be returned for.</param>
        /// <returns>The dictionary of currency code as key and the list of chart data points as value.</returns>
        public IDictionary<CurrencyCode, IEnumerable<ChartCurrencyDataPoint>> GetChartData(ChartTimePeriod chartTimePeriod, IEnumerable<CurrencyCode> chartCurrencyCodes)
        {
            var currencyData = _iCachingProcessor.RequestPeriodData(chartTimePeriod, chartCurrencyCodes);
            var currencyDataPoints = new Dictionary<CurrencyCode, IEnumerable<ChartCurrencyDataPoint>>();

            foreach (KeyValuePair<CurrencyCode, IEnumerable<CurrencyData>> pair in currencyData)
            {
                List<ChartCurrencyDataPoint> dataPoints = new List<ChartCurrencyDataPoint>();
                foreach (CurrencyData data in pair.Value)
                {
                    // Fix positions.
                    dataPoints.Add(new ChartCurrencyDataPoint() { CurrencyDataObject = data, Position = new Point()});
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
        public IDictionary<CurrencyCode, CurrencyData> GetDailyCurrencies(DateTime date, IEnumerable<CurrencyCode> chartCurrencyCodes)
        {
            return _iCachingProcessor.RequestSingleData(date, chartCurrencyCodes);
        }
    }
}
