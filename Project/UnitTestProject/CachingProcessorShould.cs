using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.CurrencyImporters;
using CurrencyExplorer.Models.Entities;
using Xunit;

namespace UnitTestProject
{
    public class CachingProcessorShould
    {
        [Fact]
        public void ImportData()
        {
            ICurrencyImporter importer = new JsonCurrencyImporter();
            ICurrencyProvider currencyProvider = new NationalBankCurrencyProvider(importer);
            ICachingProcessor cachingProcessor = new ApiDatabaseCachingProcessor(currencyProvider);

            var data = cachingProcessor.RequestSingleData(DateTime.Now, new CurrencyCode[]
            {
                new CurrencyCode()
                {
                    Value = "USD"
                },
                new CurrencyCode()
                {
                    Value = "EUR"
                }
            });

            Assert.NotNull(data);
        }
    }
}
