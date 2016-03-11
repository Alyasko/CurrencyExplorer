using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;

namespace CurrencyExplorer.Models.CurrencyImporters
{
    public class JsonCurrencyImporter : AbstractCurrencyImporter
    {
        public override void Import(DateTime time)
        {
            //HttpRequest htppRequest = new DefaultHttpRequest(); HttpRequest.Create();

            WebClient webClient = new WebClient();
            StringData = webClient.DownloadString("@http://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date=20160310");
        }

        [Obsolete]
        public string StringData { get; set; }
    }
}
