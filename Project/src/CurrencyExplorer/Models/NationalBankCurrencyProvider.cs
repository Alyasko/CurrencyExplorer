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

        public NationalBankCurrencyProvider()
        {
            _iCurrencyImporter = null;
        }

        public void RequestCurrencyData(DateTime time, CurrencyCode currencyCode)
        {
            throw new NotImplementedException();
        }

        public CurrencyData Data { get; }
    }
}
