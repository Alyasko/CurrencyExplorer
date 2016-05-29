using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models.CurrencyImporters
{
    public class XmlCurrencyImporter : AbstractCurrencyImporter
    {
        public override IDictionary<CurrencyCodeEntry, JsonCurrencyData> Import(DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
