using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer
{
    public static class Utils
    {
        public static string GetFormattedDateString(DateTime date)
        {
            return $"{date.Year:0000}{date.Month:00}{date.Day:00}";
        }
    }
}
