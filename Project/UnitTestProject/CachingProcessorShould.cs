using System;
using System.Collections;
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
                    Value = "36"
                },
                new CurrencyCode()
                {
                    Value = "826"
                }
            });

            Assert.Equal(2, data.Count);
        }

        [Fact]
        public void ImportDataWithNoCurrencyCodesSpecified()
        {
            ICurrencyImporter importer = new JsonCurrencyImporter();
            ICurrencyProvider currencyProvider = new NationalBankCurrencyProvider(importer);
            ICachingProcessor cachingProcessor = new ApiDatabaseCachingProcessor(currencyProvider);

            var data = cachingProcessor.RequestSingleData(DateTime.Now, new CurrencyCode[]
            {
                
            });

            Assert.Equal(0, data.Count);
        }

        [Fact]
        public void ImportDataWithNullCurrencyCodes()
        {
            ICurrencyImporter importer = new JsonCurrencyImporter();
            ICurrencyProvider currencyProvider = new NationalBankCurrencyProvider(importer);
            ICachingProcessor cachingProcessor = new ApiDatabaseCachingProcessor(currencyProvider);

            Action action = () =>
            {
                cachingProcessor.RequestSingleData(DateTime.Now, null);
            };

            Assert.ThrowsAny<NullReferenceException>(action);
        }
    }
}
