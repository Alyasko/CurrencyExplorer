using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Models.Exporters;

namespace CurrencyExplorer.Models
{
    public class DataPresenter
    {
        public string ExportAsImage()
        {
            if (InputPoints == null)
            {
                throw new NullReferenceException();
            }

            ICurrencyExporter<string> exporter = new JpegCurrencyExporter();

            string result = ExportData<string>(exporter);

            return result;
        }

        public string ExportAsTable()
        {
            if (InputPoints == null)
            {
                throw new NullReferenceException();
            }

            ICurrencyExporter<string> exporter = new CsvCurrencyExporter();

            string result = ExportData<string>(exporter);

            return result;
        }

        private TResult ExportData<TResult>(ICurrencyExporter<TResult> exporter)
        {
            if (exporter == null)
            {
                throw new NullReferenceException();
            }

            exporter.InputData = InputPoints;
            exporter.ChartDataRequest = DataRequest;
            exporter.Export();

            TResult result = exporter.Result;

            return result;
        }

        public IDictionary<CurrencyCodeEntry, ICollection<ChartCurrencyDataPoint<CurrencyDataEntry>>> InputPoints
        {
            get;
            set;
        }

        public ChartDataRequest DataRequest { get; set; }
    }
}
