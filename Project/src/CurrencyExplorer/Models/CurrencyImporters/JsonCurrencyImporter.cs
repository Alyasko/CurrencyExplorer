using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Newtonsoft.Json;

namespace CurrencyExplorer.Models.CurrencyImporters
{
    public class JsonCurrencyImporter : AbstractCurrencyImporter
    {
        public override async Task<IDictionary<CurrencyCode, CurrencyData>> ImportAsync(DateTime date)
        {
            IDictionary<CurrencyCode, CurrencyData> currencyCodeResult = null;

            string jsonStringResult = "";

            try
            {
                string dateFormated = Utils.GetFormattedDateString(date);
                WebRequest httpRequest = WebRequest.Create($"http://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date={dateFormated}&json");
                WebResponse response = await httpRequest.GetResponseAsync();

                Stream resposeStream = response.GetResponseStream();
                if (resposeStream != null)
                {
                    StreamReader reader = new StreamReader(resposeStream);
                    jsonStringResult = reader.ReadToEnd();

                    IEnumerable<CurrencyData> jsonCurrencyData = JsonConvert.DeserializeObject<IEnumerable<CurrencyData>>(jsonStringResult);

                    if (jsonCurrencyData.Any())
                    {
                        currencyCodeResult = new Dictionary<CurrencyCode, CurrencyData>();

                        foreach (CurrencyData currencyData in jsonCurrencyData)
                        {
                            currencyCodeResult.Add(currencyData.Code, currencyData);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                // TODO: handle exception.
                throw;
            }

            return currencyCodeResult;
        }
    }
}
