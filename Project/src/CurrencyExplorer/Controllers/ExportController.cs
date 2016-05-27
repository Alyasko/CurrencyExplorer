using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Entities;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;

namespace CurrencyExplorer.Controllers
{
    public class ExportController : Controller
    {
        private CurrencyXplorer _currencyExplorer;

        public ExportController(CurrencyXplorer currencyExplorer)
        {
            _currencyExplorer = currencyExplorer;
        }

        [HttpPost]
        public IActionResult ExportAsImage(string json)
        {
            ChartDataRequest clientRequest = JsonConvert.DeserializeObject<ChartDataRequest>(json);

            string status = "OK";
            string filePath = "";

            if (clientRequest != null)
            {
                filePath = _currencyExplorer.ExportChartAsImage(clientRequest);
            }

            if (System.IO.File.Exists(filePath) == false)
            {
                status = "Fail";
            }

            return Json(new { Status = status, Path = filePath });
        }

        [HttpPost]
        public IActionResult ExportAsTable(string json)
        {
            ChartDataRequest clientRequest = JsonConvert.DeserializeObject<ChartDataRequest>(json);

            string status = "OK";
            string filePath = "";

            if (clientRequest != null)
            {
                filePath = _currencyExplorer.ExportChartAsTable(clientRequest);
            }

            if (System.IO.File.Exists(filePath) == false)
            {
                status = "Fail";
            }

            return Json(new { Status = status, Path = filePath });
        }

    }
}

