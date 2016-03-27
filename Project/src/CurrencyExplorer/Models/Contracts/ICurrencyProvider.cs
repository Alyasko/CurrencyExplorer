using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyProvider
    {
        IDictionary<CurrencyCode, CurrencyData> RequestCurrencyData(DateTime time);
        ICollection<CurrencyCode> RequestAllCurrencyCodes(DateTime time);
    }
}
