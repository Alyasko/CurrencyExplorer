using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        public void GeneralTest()
        {
            // Arrange
            JsonCurrencyImporter currencyImporter = new JsonCurrencyImporter();

            // Act
            currencyImporter.Import(DateTime.Now);

            var data = currencyImporter.StringData;

            // Assert
            Assert.Equal(data, null);
        }
    }
}
