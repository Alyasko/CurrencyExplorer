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
        public abstract void Import(DateTime time);

        //protected virtual 

        public IDictionary<CurrencyCode, CurrencyData> Data { get; set; }
    }
}
