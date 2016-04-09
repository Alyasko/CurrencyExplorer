using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Converters;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.Entities
{
    [JsonObject]
    public class CurrencyData
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
        public string ShortName { get; set; }

        /// <summary>
        /// The name of the currency.
        /// </summary>
        [JsonProperty("txt")]
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
        [JsonConverter(typeof(StringToCodeJsonConverter))]
        [JsonProperty("r030")]
        public CurrencyCode CurrencyCode { get; set; }

        public override string ToString()
        {
            return $"{ShortName}:{Name}";
        }
    }
}
