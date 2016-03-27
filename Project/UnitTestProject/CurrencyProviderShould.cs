using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.CurrencyImporters;
using Xunit;

namespace UnitTestProject
{
    public class CurrencyProviderShould
    {
        [Fact]
        public void ImportDataFromBankAPI()
        {
            ICurrencyImporter importer = new JsonCurrencyImporter();
            ICurrencyProvider currencyProvider = new NationalBankCurrencyProvider(importer);

            try
            {
                var data = currencyProvider.RequestCurrencyData(DateTime.Now);

                Assert.NotNull(data);
            }
            catch (Exception e)
            {
                Assert.True(e is NoItemsException);
            }
        }
    }
}
