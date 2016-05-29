using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using Microsoft.AspNet.Builder;

namespace CurrencyExplorer.Models
{
    public class NationalBankCurrencyProvider : ICurrencyProvider
    {
        private ICurrencyImporter _iCurrencyImporter;

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="importer">Specific currency importer.</param>
        public NationalBankCurrencyProvider(ICurrencyImporter importer)
        {
            _iCurrencyImporter = importer;
        }

        /// <summary>
        /// Requests single data.
        /// </summary>
        /// <param name="time">The time of chart data to be received.</param>
        /// <returns>The dictionary with data.</returns>
        public IDictionary<CurrencyCodeEntry, CurrencyDataEntry> RequestSingleCurrencyData(DateTime time)
        {
            IDictionary<CurrencyCodeEntry, JsonCurrencyData> jsonResult = _iCurrencyImporter.Import(time);

            IDictionary<CurrencyCodeEntry, CurrencyDataEntry> result = null;

            if (jsonResult != null)
            {
                result = jsonResult.ToDictionary(
                    k => k.Key,
                    v => new CurrencyDataEntry()
                    {
                        ActualDate = v.Value.ActualDate,
                        Value = v.Value.Value,
                        DbCurrencyCodeEntry = v.Key
                    });
            }

            return result;
        }

        public IDictionary<CurrencyCodeEntry, List<CurrencyDataEntry>> RequestPeriodCurrencyData(ChartTimePeriod period)
        {
            throw new NotImplementedException();
        }

        public ICollection<CurrencyCodeEntry> RequestAllCurrencyCodes(DateTime time)
        {
            var allData = _iCurrencyImporter.Import(time);

            return allData.Select(currencyData => currencyData.Key).ToList();
        }

        public ICurrencyImporter ICurrencyImporter
        {
            get { return _iCurrencyImporter; }
            set { _iCurrencyImporter = value; }
        }
    }
}
