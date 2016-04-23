﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Utilities;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.CurrencyImporters
{
    public class JsonCurrencyImporter : AbstractCurrencyImporter
    {
        public override IDictionary<CurrencyCodeEntry, JsonCurrencyData> Import(DateTime date)
        {
            IDictionary<CurrencyCodeEntry, JsonCurrencyData> currencyCodeResult = null;

            string jsonStringResult = "";

            string dateFormated = Utils.GetFormattedDateString(date);
            string requestUrl =
                $"http://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date={dateFormated}&json";

            HttpClient httpClient = new HttpClient();
            
            Stream resposeStream = httpClient.GetStreamAsync(requestUrl).Result;

            if (resposeStream != null)
            {
                StreamReader reader = new StreamReader(resposeStream);
                jsonStringResult = reader.ReadToEnd();

                //JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                //jsonSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                //jsonSettings.DateFormatString = "dd.mm.yyyy";

                IEnumerable<JsonCurrencyData> jsonCurrencyData =
                    JsonConvert.DeserializeObject<IEnumerable<JsonCurrencyData>>(jsonStringResult);

                if (jsonCurrencyData.Any())
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

            }

            return currencyCodeResult;
        }
    }
}
