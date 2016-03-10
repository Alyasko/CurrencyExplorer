using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities
{
    public class CurrencyData
    {
        /// <summary>
        /// The value of currency.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Short representation of name.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// The name of the currency.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The time of currency actuality.
        /// </summary>
        public DateTime Actual { get; set; }

        /// <summary>
        /// Unique code of currency.
        /// </summary>
        public CurrencyCode Code { get; set; }
    }
}
