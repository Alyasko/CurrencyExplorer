using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.Entities
{
    [JsonObject]
    public class CurrencyData
    {
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
        /// The time of currency actuality.
        /// </summary>
        [JsonProperty("exchangedate")]
        public DateTime Actual { get; set; }

        /// <summary>
        /// Unique code of currency.
        /// </summary>
        [JsonConverter(typeof(StringToCodeJsonConverter))]
        [JsonProperty("r030")]
        public CurrencyCode Code { get; set; }

        public override string ToString()
        {
            return $"{ShortName}:{Name}";
        }
    }
}
