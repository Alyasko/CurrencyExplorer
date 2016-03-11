using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using Microsoft.AspNet.Builder;

namespace CurrencyExplorer.Models
{
    public class NationalBankCurrencyProvider : ICurrencyProvider
    {
        private ICurrencyImporter _iCurrencyImporter;

        public NationalBankCurrencyProvider(ICurrencyImporter importer)
        {
            _iCurrencyImporter = importer;
        }

        public void RequestCurrencyData(DateTime time)
        {
            _iCurrencyImporter.Import(time);
            Data = _iCurrencyImporter.Data;
        }

        public IDictionary<CurrencyCode, CurrencyData> Data { get; private set; }

        public ICurrencyImporter ICurrencyImporter
        {
            get { return _iCurrencyImporter; }
            set { _iCurrencyImporter = value; }
        }
    }
}
