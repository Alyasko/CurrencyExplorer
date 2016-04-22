using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyProvider
    {
        IDictionary<CurrencyCode, CurrencyData> RequestSingleCurrencyData(DateTime time);
        IDictionary<CurrencyCode, List<CurrencyData>> RequestPeriodCurrencyData(ChartTimePeriod period);
        ICollection<CurrencyCode> RequestAllCurrencyCodes(DateTime time);
    }
}
