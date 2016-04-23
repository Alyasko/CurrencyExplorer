using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyImporter
    {
        IDictionary<CurrencyCodeEntry, JsonCurrencyData> Import(DateTime time);
    }
}
