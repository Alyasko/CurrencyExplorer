using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities
{
    public class CurrencyCode
    {
        /// <summary>
        /// String representation of code.
        /// </summary>
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            CurrencyCode currency = obj as CurrencyCode;

            if (currency == null)
            {
                return false;
            }

            return string.Equals(Value, currency.Value);
        }
    }
}
