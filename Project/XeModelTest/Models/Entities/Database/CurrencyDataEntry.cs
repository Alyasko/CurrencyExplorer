using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Converters;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.Entities.Database
{
    public class CurrencyDataEntry
    {
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// The value of currency.
        /// </summary>
        public double Value { get; set; }


        /// <summary>
        /// The date of currency actuality.
        /// </summary>
        //[JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime ActualDate { get; set; }

        /// <summary>
        /// Unique code of currency.
        /// </summary>
        public CurrencyCodeEntry DbCurrencyCodeEntry { get; set; }

        public CurrencyDataEntry Clone()
        {
            return new CurrencyDataEntry()
            {
                DbCurrencyCodeEntry = DbCurrencyCodeEntry,
                Value = Value,
                ActualDate = ActualDate,
                Id = Id,
            };
        }

        #region EqualityMembers

        public override bool Equals(object o)
        {
            JsonCurrencyData other = o as JsonCurrencyData;
            bool result = other != null && this.Equals(other);

            return result;
        }

        protected bool Equals(CurrencyDataEntry other)
        {
            return Id == other.Id && Value.Equals(other.Value) && ActualDate.Equals(other.ActualDate) &&
                   Equals(DbCurrencyCodeEntry, other.DbCurrencyCodeEntry);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode*397) ^ Value.GetHashCode();
                hashCode = (hashCode*397) ^ ActualDate.GetHashCode();
                hashCode = (hashCode*397) ^ (DbCurrencyCodeEntry != null ? DbCurrencyCodeEntry.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(JsonCurrencyData other)
        {
            bool result = this.ActualDate.Date == other.ActualDate.Date &&
                          this.DbCurrencyCodeEntry.Equals(other.CurrencyCodeValue);
            return result;
        }

        #endregion


    }
}
