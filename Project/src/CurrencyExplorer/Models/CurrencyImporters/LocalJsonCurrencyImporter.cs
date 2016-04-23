using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.CurrencyImporters
{
    public class LocalJsonCurrencyImporter : AbstractCurrencyImporter
    {
        public override IDictionary<CurrencyCodeEntry, JsonCurrencyData> Import(DateTime time)
        {
            IDictionary<CurrencyCodeEntry, JsonCurrencyData> currencyCodeResult = null;

            // TODO: add culture specific date time.

            CultureInfo ci = new CultureInfo("ru-RU");

            string path = $"Json//test{time.ToString("d", ci)}.txt";

            string jsonStringResult = "";

            if (File.Exists(path))
            {
                jsonStringResult = File.ReadAllText(path);
            }

            IEnumerable<JsonCurrencyData> jsonCurrencyData =
                JsonConvert.DeserializeObject<IEnumerable<JsonCurrencyData>>(jsonStringResult);

            if (jsonCurrencyData != null && jsonCurrencyData.Any())
            {
                currencyCodeResult = new Dictionary<CurrencyCodeEntry, JsonCurrencyData>();

                foreach (JsonCurrencyData currencyData in jsonCurrencyData)
                {
                    CurrencyCodeEntry currencyCodeEntry = new CurrencyCodeEntry()
                    {
                        Value = currencyData.CurrencyCodeValue,
                        Alias = currencyData.ShortName,
                        Name = currencyData.Name
                    };
                    currencyCodeResult.Add(currencyCodeEntry, currencyData);
                }
            }
            else
            {
                throw new NoItemsException("Json result returned no items.");
            }

            return currencyCodeResult;
        }
    }
}
