using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities
{
    public class JsonChartPointData
    {
        public string ActualDate { get; set; }
        public double Value { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
    }
}
