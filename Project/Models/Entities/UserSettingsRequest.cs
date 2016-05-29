using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Converters;
using CurrencyExplorer.Models.Enums;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.Entities
{
    public class UserSettingsRequest
    {

        [JsonConverter(typeof(UserLanguageJsonConverter))]
        public CurrencyExplorerLanguage Language { get; set; }

        public ICollection<string> CurrencyValues { get; set; } 

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
