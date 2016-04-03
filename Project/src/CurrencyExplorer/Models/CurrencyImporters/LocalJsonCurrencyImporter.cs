﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.CurrencyImporters
{
    public class LocalJsonCurrencyImporter : AbstractCurrencyImporter
    {
        public override IDictionary<CurrencyCode, CurrencyData> Import(DateTime time)
        {
            IDictionary<CurrencyCode, CurrencyData> currencyCodeResult = null;

            string path = $"Json//test{time.ToShortDateString()}.txt";

            string jsonStringResult = "";

            if (File.Exists(path))
            {
                jsonStringResult = File.ReadAllText(path);
            }

            IEnumerable<CurrencyData> jsonCurrencyData =
                JsonConvert.DeserializeObject<IEnumerable<CurrencyData>>(jsonStringResult);

            if (jsonCurrencyData != null && jsonCurrencyData.Any())
            {
                currencyCodeResult = new Dictionary<CurrencyCode, CurrencyData>();

                foreach (CurrencyData currencyData in jsonCurrencyData)
                {
                    currencyData.Code.Alias = currencyData.ShortName;
                    currencyCodeResult.Add(currencyData.Code, currencyData);
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