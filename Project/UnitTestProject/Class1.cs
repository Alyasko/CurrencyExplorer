using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Contracts;
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

            ICurrencyProvider currencyProvider =
                Mock.Of<ICurrencyProvider>(
                    t => t.Data == new CurrencyData() {Actual = DateTime.Now, Code = new CurrencyCode() {Value = "r020"}});
            ICachingProcessor cachingProcessor =
                Mock.Of<ICachingProcessor>(
                    t => t.RequestSingleData(It.IsAny<DateTime>(), It.IsAny<IEnumerable<CurrencyCode>>()) == null);

            // Act
            var data = cachingProcessor.RequestSingleData(DateTime.Now, null);

            // Assert
            Assert.Equal(cachingProcessor, null);
        }
    }
}
