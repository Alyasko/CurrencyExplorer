using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyProvider
    {
        IDictionary<CurrencyCodeEntry, CurrencyDataEntry> RequestSingleCurrencyData(DateTime time);
        IDictionary<CurrencyCodeEntry, List<CurrencyDataEntry>> RequestPeriodCurrencyData(ChartTimePeriod period);
        ICollection<CurrencyCodeEntry> RequestAllCurrencyCodes(DateTime time);
    }
}
