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
            IEnumerable<CurrencyCode> codes);

        IDictionary<CurrencyCode, IEnumerable<CurrencyData>> RequestPeriodData(
            ChartTimePeriod timePeriod,
            IEnumerable<CurrencyCode> codes);

    }
}
