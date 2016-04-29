using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;
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
            IDictionary<CurrencyCodeEntry, ICollection<ChartCurrencyDataPoint<CurrencyDataEntry>>> points = null;

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

                    IDictionary<string, List< JsonChartPointData>> lazyPoints =
                        new Dictionary<string, List<JsonChartPointData>>();

                    foreach (var pair in points)
                    {
                        lazyPoints.Add(pair.Key.Alias, new List<JsonChartPointData>());

                        foreach (var currencyDataPoint in pair.Value)
                        {
                            // TODO: fix date conversion so that it is based on language.
                            lazyPoints[pair.Key.Alias].Add(new JsonChartPointData()
                            {
                                ActualDate = currencyDataPoint.DataObject.ActualDate.ToString(),
                                Value = currencyDataPoint.DataObject.Value,
                                Alias = currencyDataPoint.DataObject.DbCurrencyCodeEntry.Alias,
                                Name = currencyDataPoint.DataObject.DbCurrencyCodeEntry.Name
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
#pragma warning disable CS0162 // Unreachable code detected
                return HttpNotFound();
#pragma warning restore CS0162 // Unreachable code detected
            }



            return Json(resultJson);
        }
    }
}
