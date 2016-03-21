using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CurrencyExplorer;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.CurrencyImporters;
using CurrencyExplorer.Models.Entities;
using Moq;
using Xunit;

namespace UnitTestProject
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Class1
    {

        [Fact]
        public async void JsonImportedDataIsNotNull()
        {
            // Arrange
            JsonCurrencyImporter currencyImporter = new JsonCurrencyImporter();

            // Act
            var data = await currencyImporter.ImportAsync(DateTime.Now);

            // Assert
            Assert.NotEqual(data, null);
        }

        [Fact]
        public async void IncorrectDateForBankApi1()
        {
            // Arrange
            JsonCurrencyImporter currencyImporter = new JsonCurrencyImporter();

            // Act
            var data = await currencyImporter.ImportAsync(DateTime.Parse("1900.03.10"));

            // Assert
            Assert.Null(data);
        }

        [Fact]
        public async void IncorrectDateForBankApi2()
        {
            // Arrange
            JsonCurrencyImporter currencyImporter = new JsonCurrencyImporter();

            // Act
            var data = await currencyImporter.ImportAsync(DateTime.Parse("2621.03.25"));

            // Assert
            Assert.Null(data);
        }

        [Fact]
        public void DateStringFormatTest()
        {
            string formated = Utils.GetFormattedDateString(DateTime.Parse("2016.03.10"));

            Assert.Equal("20160310", formated);
        }
    }
}
