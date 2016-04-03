using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Entities;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyExplorer.Controllers
{
    public class ChartController : Controller
    {
        private CurrencyXplorer _currencyXplorer;

        public ChartController(CurrencyXplorer explorer)
        {
            _currencyXplorer = explorer;
        }

        [HttpPost]
        public IActionResult LoadChartData(string json)
        {
            ChartDataRequest clientRequest = null;
            IDictionary<CurrencyCode, ICollection<ChartCurrencyDataPoint<CurrencyData>>> points = null;

            string resultJson = "";

            try
            {
                clientRequest = JsonConvert.DeserializeObject<ChartDataRequest>(json);

                if (clientRequest != null)
                {
                    _currencyXplorer.ChartTimePeriod = new ChartTimePeriod(clientRequest.Begin, clientRequest.End);
                    _currencyXplorer.ChartCurrencyCodeStrings = clientRequest.Currencies;
                    _currencyXplorer.RequestChartData();

                    points = _currencyXplorer.ChartDataPoints;

                    // Minimify all

                    IDictionary<string, List<string[]>> lazyPoints =
                        new Dictionary<string, List<string[]>>();

                    foreach (var pair in points)
                    {
                        lazyPoints.Add(pair.Key.Alias, new List<string[]>());

                        foreach (ChartCurrencyDataPoint<CurrencyData> currencyDataPoint in pair.Value)
                        {
                            // TODO: fix date conversion so that it is based on language.
                            lazyPoints[pair.Key.Alias].Add(new string[]
                            {
                                currencyDataPoint.DataObject.ActualDate.ToLongDateString(),
                                currencyDataPoint.DataObject.Value.ToString(),
                                currencyDataPoint.DataObject.ShortName,
                                currencyDataPoint.DataObject.Name
                            });
                        }
                    }

                    resultJson = JsonConvert.SerializeObject(lazyPoints);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
                return HttpNotFound();
            }



            return Json(resultJson);
        }
    }
}
