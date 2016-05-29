using System.Collections.Generic;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models.Exporters
{
    public abstract class AbstractCurrencyExporter<TResult> : ICurrencyExporter<TResult>
    {
        public abstract void Export();

        public TResult Result { get; protected set; }

        public IDictionary<CurrencyCodeEntry, ICollection<ChartCurrencyDataPoint<CurrencyDataEntry>>> InputData
        {
            get; set;
        }

        public ChartDataRequest ChartDataRequest { get; set; }

    }
}
