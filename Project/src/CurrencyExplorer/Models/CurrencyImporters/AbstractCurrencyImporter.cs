using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models.CurrencyImporters
{
    public abstract class AbstractCurrencyImporter : ICurrencyImporter
    {
        public abstract Task<IDictionary<CurrencyCode, CurrencyData>> ImportAsync(DateTime time);
    }
}
