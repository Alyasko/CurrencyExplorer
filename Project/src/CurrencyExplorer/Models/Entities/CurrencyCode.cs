using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities
{
    public class CurrencyCode
    {
        public CurrencyCode()
        {
            
        }

        public CurrencyCode(string value, string alias)
        {
            Value = value;
            Alias = alias;
        }

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// String representation of code.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The alias for Value. For example USD, UAH.
        /// </summary>
        public string Alias { get; set; }

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

        protected bool Equals(CurrencyCode other)
        {
            return string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
