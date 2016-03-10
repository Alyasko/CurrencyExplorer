using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities
{
    public class ChartTimePeriod
    {
        /// <summary>
        /// Begin time.
        /// </summary>
        public DateTime Begin { get; set; }

        /// <summary>
        /// End time.
        /// </summary>
        public DateTime End { get; set; }
    }
}
