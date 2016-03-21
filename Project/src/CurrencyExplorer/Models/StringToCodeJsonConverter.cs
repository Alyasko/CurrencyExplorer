using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models
{
    public class StringToCodeJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            CurrencyCode obj = (CurrencyCode)value;

            // Write associative array field name
            writer.WriteValue(obj.Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = reader.Value.ToString();
            
            return new CurrencyCode() { Value = value };
        }

        public override bool CanConvert(Type objectType)
        {
            return false;
        }
    }
}
