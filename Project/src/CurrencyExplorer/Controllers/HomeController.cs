using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Utilities;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;

namespace CurrencyExplorer.Controllers
{
    public class HomeController : Controller
    {
        private CurrencyXplorer _currencyXplorer;
        private CookiesManager _cookiesManager;

        private static int launchCounter = 0;

        public HomeController(CurrencyXplorer currencyXplorer)
        {
            launchCounter++;

            _currencyXplorer = currencyXplorer;
            Debug.WriteLine("Entry");
        }

        public IActionResult Index()
        {
            // TODO: add cookies usage.

            if (launchCounter < 3)
            {
                return Content("Update page, please");
            }

            _cookiesManager = new CookiesManager(Request, Response);

            long uid = _cookiesManager.GetUid();

            UserSettings userSettings = _currencyXplorer.RequestUserSettings(uid);

            ICollection<CurrencyCodeEntry> currencyCodesList = _currencyXplorer.GetAllCurrencyCodes();

            ViewBag.CurrencyCodesList = currencyCodesList;
            ViewBag.UserSettings = userSettings;

            return View();
        }
    }
}
