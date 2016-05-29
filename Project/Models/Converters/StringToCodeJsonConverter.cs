using System;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.Converters
{
    public class StringToCodeJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            CurrencyCodeEntry obj = (CurrencyCodeEntry)value;

            // Write associative array field name
            writer.WriteValue(obj.Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = reader.Value.ToString();
            
            return new CurrencyCodeEntry() { Value = value };
        }

        public override bool CanConvert(Type objectType)
        {
            return false;
        }
    }
}
