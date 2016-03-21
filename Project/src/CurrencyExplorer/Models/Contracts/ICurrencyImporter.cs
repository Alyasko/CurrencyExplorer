using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyImporter
    {
        Task<IDictionary<CurrencyCode, CurrencyData>> ImportAsync(DateTime time);
    }
}
