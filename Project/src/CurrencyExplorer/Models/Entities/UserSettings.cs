﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities
{
    public class UserSettings
    {
        /// <summary>
        /// The language of web-site.
        /// </summary>
        public CurrencyExplorerLanguage Language { get; set; }

        /// <summary>
        /// Selected currencies.
        /// </summary>
        public IEnumerable<CurrencyData> Currencies { get; set; } 

        /// <summary>
        /// Selected time period.
        /// </summary>
        public ChartTimePeriod TimePeriod { get; set; }
    }
}