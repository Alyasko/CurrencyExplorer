using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CurrencyExplorer;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.CurrencyImporters;
using CurrencyExplorer.Models.Entities;
using Xunit;

namespace UnitTestProject
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class CurrencyExplorerShould
    {

        [Fact]
        public void ReturnNotNullJsonData()
        {
            // Arrange
            JsonCurrencyImporter currencyImporter = new JsonCurrencyImporter();

            try
            {
                // Act
                var data = currencyImporter.Import(DateTime.Now);

                // Assert
                Assert.NotEqual(data, null);
            }
            catch (Exception e)
            {
                Assert.True(e is NoItemsException);
            }

            
        }

        [Fact]
        public async void WorkCorrectlyWithIncorrectDate()
        {
            // Arrange
            JsonCurrencyImporter currencyImporter = new JsonCurrencyImporter();

            // Act
            //var data = await currencyImporter.ImportAsync(DateTime.Parse("1900.03.10"));

            // Assert
            //Assert.Null(data);
        }

        [Fact]
        public void IncorrectDateForBankApi2()
        {
            // Arrange
            JsonCurrencyImporter currencyImporter = new JsonCurrencyImporter();

            // Act
            try
            {
                var data = currencyImporter.Import(DateTime.Parse("2621.03.25"));

                Assert.Null(data);
            }
            catch (Exception e)
            {
                Assert.True(e is NoItemsException);
            }
            
        }

        [Fact]
        public void TestDateStringFormat()
        {
            string formated = Utils.GetFormattedDateString(DateTime.Parse("2016.03.10"));

            Assert.Equal("20160310", formated);
        }

        //[Fact]
        //public void TestDateTimeConvertion()
        //{
        //    DateTime dateTime = DateTime.Parse("22.03.2016");

        //    string formatted = dateTime.ToString("dd.mm.yyyy");

        //    Assert.Equal("22.03.2016", formatted);
        //}
    }
}
