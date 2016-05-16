using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Utilities;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyExplorer.Controllers
{
    public class SettingsController : Controller
    {
        private CookiesManager _cookiesManager;
        private CurrencyXplorer _currencyXplorer;

        public SettingsController(CurrencyXplorer explorer)
        {
            _currencyXplorer = explorer;
        }

        public IActionResult SaveUserSettings(string json)
        {
            _cookiesManager = new CookiesManager(Request, Response);
            UserSettingsRequest settings = JsonConvert.DeserializeObject<UserSettingsRequest>(json);

            long cookie = _cookiesManager.GetUid();

            _currencyXplorer.SaveUserSettings(cookie, settings);

            return Json(new { Result = "OK" });
        }
    }
}
