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
        /// <summary>
        /// Exports currency chart as image.
        /// </summary>
        /// <returns>The string pointing to the file.</returns>
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

        /// <summary>
        /// Exports currency chart as table.
        /// </summary>
        /// <returns>The string pointing to the file.</returns>
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

        /// <summary>
        /// Common method for chart exporting.
        /// </summary>
        /// <typeparam name="TResult">The type of result.</typeparam>
        /// <param name="exporter">Specific exporter.</param>
        /// <returns></returns>
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
