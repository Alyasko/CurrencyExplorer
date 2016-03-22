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

        public IDictionary<CurrencyCode, CurrencyData> RequestCurrencyData(DateTime time)
        {
            return _iCurrencyImporter.ImportAsync(time);
        }

        public ICurrencyImporter ICurrencyImporter
        {
            get { return _iCurrencyImporter; }
            set { _iCurrencyImporter = value; }
        }
    }
}
