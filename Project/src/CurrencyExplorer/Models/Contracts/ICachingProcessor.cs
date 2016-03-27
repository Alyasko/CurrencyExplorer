using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICachingProcessor
    {
        IDictionary<CurrencyCode, CurrencyData> RequestSingleData(
            DateTime timePeriod,
            ICollection<CurrencyCode> codes);

        IDictionary<CurrencyCode, ICollection<CurrencyData>> RequestPeriodData(
            ChartTimePeriod timePeriod,
            ICollection<CurrencyCode> codes);

        ICollection<CurrencyCode> RequestAllCurrencyCodes();

    }
}
