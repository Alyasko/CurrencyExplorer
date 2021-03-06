﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Converters;
using CurrencyExplorer.Models.Entities.Database;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.Entities
{
    [JsonObject]
#pragma warning disable CS0659 // 'JsonCurrencyData' overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class JsonCurrencyData
#pragma warning restore CS0659 // 'JsonCurrencyData' overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private string _actualDateString;

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Returns DateTime object from specified string.
        /// </summary>
        /// <param name="inputValue">The string represntation of DateTime.</param>
        /// <returns>The DateTime object.</returns>
        private DateTime ProcessActualDateString(string inputValue)
        {
            // TODO: parse using DateTime.
            // TODO: move Name and other to the CD table.
            // TODO: change Today.
            // TODO: currency doesn't change on weekends.
            // TODO: при отсутствии данных запросить данные раньше, когда они были.

            DateTime result;

            Regex regex = new Regex(@"(?<d>\d{2})\.(?<m>\d{2}).(?<y>\d{4})");

            Match match = regex.Match(inputValue);

            int d = 0;
            int m = 0;
            int y = 0;

            if (!Int32.TryParse(match.Groups["d"].Value, out d))
            {
                throw new FormatException("Bad input date string from API. Parameter 'Day'.");
            }

            if (!Int32.TryParse(match.Groups["m"].Value, out m))
            {
                throw new FormatException("Bad input date string from API. Parameter 'Month'.");
            }

            if (!Int32.TryParse(match.Groups["y"].Value, out y))
            {
                throw new FormatException("Bad input date string from API. Parameter 'Year'.");
            }

            result = new DateTime(y, m, d);

            return result;
        }

        /// <summary>
        /// The value of currency.
        /// </summary>
        [JsonProperty("rate")]
        public double Value { get; set; }

        /// <summary>
        /// Short representation of name.
        /// </summary>
        [JsonProperty("cc")]
        [NotMapped]
        public string ShortName { get; set; }

        /// <summary>
        /// The name of the currency.
        /// </summary>
        [JsonProperty("txt")]
        [NotMapped]
        public string Name { get; set; }

        /// <summary>
        /// The date of currency actuality.
        /// </summary>
        //[JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime ActualDate { get; private set; }

        /// <summary>
        /// The string representaton of actuality date from API.
        /// </summary>
        [JsonProperty("exchangedate")]
        [NotMapped]
        public String ActualDateString
        {
            get { return _actualDateString; }
            set
            {
                _actualDateString = value;

                ActualDate = ProcessActualDateString(value);
            }
        }

        /// <summary>
        /// Unique code of currency.
        /// </summary>
        [JsonProperty("r030")]
        public string CurrencyCodeValue { get; set; }

        public int CurrencyCodeId { get; set; }

        public override string ToString()
        {
            return $"{ShortName}:{Name}";
        }

        public override bool Equals(object o)
        {
            JsonCurrencyData other = o as JsonCurrencyData;
            bool result = other != null && this.Equals(other);

            return result;
        }

        protected bool Equals(JsonCurrencyData other)
        {
            bool result = this.ActualDate.Date == other.ActualDate.Date &&
                          this.CurrencyCodeValue.Equals(other.CurrencyCodeValue);
            return result;
        }

        public JsonCurrencyData Clone()
        {
            return new JsonCurrencyData()
            {
                Name = Name,
                CurrencyCodeValue = CurrencyCodeValue,
                Value = Value,
                ActualDateString = ActualDateString,
                CurrencyCodeId =  CurrencyCodeId,
                Id = Id,
                ShortName = ShortName,
            };
        }
    }
}
