using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyImporter
    {
        IDictionary<CurrencyCode, CurrencyData> Import(DateTime time);
    }
}
