using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.CurrencyImporters;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Models.Repositories;
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
            ICachingProcessor cachingProcessor = new ApiDatabaseCachingProcessor(currencyProvider, new CurrencyRepository(new CurrencyDataContext()));

            IDictionary<CurrencyCode, CurrencyData> data = null;

            try
            {
                data = cachingProcessor.RequestSingleData(DateTime.Now, new CurrencyCode[]
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
            catch (Exception e)
            {
                Assert.True(e is NoItemsException);
                Assert.Null(data);
            }
        }

        [Fact]
        public void ImportDataWithNoCurrencyCodesSpecified()
        {
            ICurrencyImporter importer = new JsonCurrencyImporter();
            ICurrencyProvider currencyProvider = new NationalBankCurrencyProvider(importer);
            ICachingProcessor cachingProcessor = new ApiDatabaseCachingProcessor(currencyProvider, new CurrencyRepository(new CurrencyDataContext()));

            IDictionary<CurrencyCode, CurrencyData> data = null;

            try
            {
                data = cachingProcessor.RequestSingleData(DateTime.Now, new CurrencyCode[]
                {

                });

                Assert.Equal(0, data.Count);
            }
            catch (Exception e)
            {
                Assert.True(e is NoItemsException);
                Assert.Null(data);
            }
        }

        [Fact]
        public void ImportDataWithNullCurrencyCodes()
        {
            ICurrencyImporter importer = new JsonCurrencyImporter();
            ICurrencyProvider currencyProvider = new NationalBankCurrencyProvider(importer);
            ICachingProcessor cachingProcessor = new ApiDatabaseCachingProcessor(currencyProvider, new CurrencyRepository(new CurrencyDataContext()));

            Action action = () =>
            {
                cachingProcessor.RequestSingleData(DateTime.Now, null);
            };

            Assert.ThrowsAny<NullReferenceException>(action);
        }

        [Fact]
        public void ImportPeriodicData()
        {
            ICurrencyImporter importer = new JsonCurrencyImporter();
            ICurrencyProvider currencyProvider = new NationalBankCurrencyProvider(importer);
            ICachingProcessor cachingProcessor = new ApiDatabaseCachingProcessor(currencyProvider, new CurrencyRepository(new CurrencyDataContext()));

            ChartTimePeriod timePeriod = new ChartTimePeriod(DateTime.Now.Subtract(TimeSpan.FromDays(10)), DateTime.Now);

            var data = cachingProcessor.RequestPeriodData(timePeriod, new CurrencyCode[]
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


            Assert.NotEqual(0, data.Count);
        }

        [Fact]
        public void ImportPeriodicDataWhenCurrencyCodesAreEmpty()
        {
            ICurrencyImporter importer = new JsonCurrencyImporter();
            ICurrencyProvider currencyProvider = new NationalBankCurrencyProvider(importer);
            ICachingProcessor cachingProcessor = new ApiDatabaseCachingProcessor(currencyProvider, new CurrencyRepository(new CurrencyDataContext()));

            ChartTimePeriod timePeriod = new ChartTimePeriod(DateTime.Now.Subtract(TimeSpan.FromDays(10)), DateTime.Now);

            var data = cachingProcessor.RequestPeriodData(timePeriod, new CurrencyCode[]
            {
            });

            Assert.Equal(0, data.Count);
        }

        [Fact]
        public void ImportPeriodicDataWhenCurrencyCodesArrayIsNull()
        {
            ICurrencyImporter importer = new JsonCurrencyImporter();
            ICurrencyProvider currencyProvider = new NationalBankCurrencyProvider(importer);
            ICachingProcessor cachingProcessor = new ApiDatabaseCachingProcessor(currencyProvider, new CurrencyRepository(new CurrencyDataContext()));

            ChartTimePeriod timePeriod = new ChartTimePeriod(DateTime.Now.Subtract(TimeSpan.FromDays(10)), DateTime.Now);

            Action action = () =>
            {
                cachingProcessor.RequestPeriodData(timePeriod, null);
            };

            Assert.ThrowsAny<NullReferenceException>(action);
        }
    }
}
