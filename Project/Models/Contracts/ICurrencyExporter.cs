using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyExporter<TResult> : IChartExporter
    {
        void Export();
        TResult Result { get; }
    }

    public interface IChartExporter
    {
        IDictionary<CurrencyCodeEntry, ICollection<ChartCurrencyDataPoint<CurrencyDataEntry>>> InputData { get; set; }
        ChartDataRequest ChartDataRequest { get; set; }
    }
}
