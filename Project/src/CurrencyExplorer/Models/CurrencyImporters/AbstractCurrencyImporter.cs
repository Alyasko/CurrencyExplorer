using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models.CurrencyImporters
{
    public abstract class AbstractCurrencyImporter : ICurrencyImporter
    {
        public abstract IDictionary<CurrencyCodeEntry, JsonCurrencyData> Import(DateTime time);
    }
}
