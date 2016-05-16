using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities
{
    public class ChartDataRequest
    {

        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public List<string> CurrencyValues { get; set; } 
    }
}
