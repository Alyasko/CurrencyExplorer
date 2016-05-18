using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities.Database;
using CurrencyExplorer.Models.Enums;
using CurrencyExplorer.Models.Localizations;
using CurrencyExplorer.Utilities;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Headers;

namespace CurrencyExplorer.Controllers
{
    public class HomeController : Controller
    {
        private CurrencyXplorer _currencyXplorer;
        private CookiesManager _cookiesManager;
        private IApplicationEnvironment _applicationEnvironment;

        private static int launchCounter = 0;

        public HomeController(CurrencyXplorer currencyXplorer, IApplicationEnvironment appEnv)
        {
            launchCounter++;

            _currencyXplorer = currencyXplorer;
            _applicationEnvironment = appEnv;

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

            CurrencyExplorerLanguage language = _currencyXplorer.CurrencyExplorerLanguage;
            //CurrencyExplorerLanguage language = CurrencyExplorerLanguage.Ukrainian;

            ILocalization localization = null;

            switch (language)
            {
                case CurrencyExplorerLanguage.Russian:
                    localization = new RussianLocalization(_applicationEnvironment);
                    break;
                case CurrencyExplorerLanguage.Ukrainian:
                    localization = new UkrainianLocalization(_applicationEnvironment);
                    break;
                case CurrencyExplorerLanguage.English:
                    localization = new EnglishLocalization(_applicationEnvironment);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ViewBag.Localization = localization;
            ViewBag.UiLanguage = language;
            ViewBag.CurrencyCodesList = currencyCodesList;
            ViewBag.UserSettings = userSettings;

            return View();
        }
    }
}
