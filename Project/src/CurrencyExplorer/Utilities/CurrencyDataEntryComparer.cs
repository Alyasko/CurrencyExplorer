using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Utilities
{
    public class CurrencyDataEntryComparer : IComparer<CurrencyDataEntry>
    {
        public int Compare(CurrencyDataEntry x, CurrencyDataEntry y)
        {
            return String.Compare(x.DbCurrencyCodeEntry.Value, y.DbCurrencyCodeEntry.Value, StringComparison.Ordinal);
        }
    }
}
