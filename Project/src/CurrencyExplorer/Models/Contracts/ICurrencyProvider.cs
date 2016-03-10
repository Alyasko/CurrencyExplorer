using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyProvider
    {
        void RequestCurrencyData(DateTime time, CurrencyCode currencyCode);
        CurrencyData Data { get; } 
    }
}
