using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICachingProcessor
    {
        IDictionary<CurrencyCodeEntry, CurrencyDataEntry> RequestSingleData(
            DateTime time,
            ICollection<CurrencyCodeEntry> codes,
            bool useCaching = true);

        IDictionary<CurrencyCodeEntry, ICollection<CurrencyDataEntry>> RequestPeriodData(
            ChartTimePeriod timePeriod,
            ICollection<CurrencyCodeEntry> codes);

        ICollection<CurrencyCodeEntry> RequestAllCurrencyCodes();

    }
}
