using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Enums;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.Converters
{
    public class UserLanguageJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            CurrencyExplorerLanguage language = CurrencyExplorerLanguage.English;
            string shortLanValue = reader.Value.ToString();

            switch (shortLanValue)
            {
                case "ru":
                    language = CurrencyExplorerLanguage.Russian;
                    break;
                case "ua":
                    language = CurrencyExplorerLanguage.Ukrainian;
                    break;
            }

            return language;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.Name == "UserSettingsRequest")
                return true;
            return false;
        }
    }
}
