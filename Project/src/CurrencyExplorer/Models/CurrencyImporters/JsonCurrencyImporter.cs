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
        public override IDictionary<CurrencyCode, CurrencyData> Import(DateTime date)
        {
            IDictionary<CurrencyCode, CurrencyData> currencyCodeResult = null;

            string jsonStringResult = "";

            string dateFormated = Utils.GetFormattedDateString(date);
            string requestUrl =
                $"http://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date={dateFormated}&json";
            WebRequest httpRequest = WebRequest.Create(requestUrl);
            WebResponse response = httpRequest.GetResponse();

            Stream resposeStream = response.GetResponseStream();
            if (resposeStream != null)
            {
                StreamReader reader = new StreamReader(resposeStream);
                jsonStringResult = reader.ReadToEnd();

                //JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                //jsonSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                //jsonSettings.DateFormatString = "dd.mm.yyyy";

                IEnumerable<CurrencyData> jsonCurrencyData =
                    JsonConvert.DeserializeObject<IEnumerable<CurrencyData>>(jsonStringResult);

                if (jsonCurrencyData.Any())
                {
                    currencyCodeResult = new Dictionary<CurrencyCode, CurrencyData>();

                    foreach (CurrencyData currencyData in jsonCurrencyData)
                    {
                        currencyCodeResult.Add(currencyData.Code, currencyData);
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
