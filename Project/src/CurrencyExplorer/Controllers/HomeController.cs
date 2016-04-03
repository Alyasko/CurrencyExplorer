using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;

namespace CurrencyExplorer.Controllers
{
    public class HomeController : Controller
    {
        private CurrencyXplorer _currencyXplorer;

        public HomeController(CurrencyXplorer currencyXplorer)
        {
            _currencyXplorer = currencyXplorer;
            Debug.WriteLine("Entry");
        }

        public IActionResult Index()
        {
            // TODO: add cookies usage.
            
            UserSettings userSettings = _currencyXplorer.RequestUserSettings("cookie");

            if (userSettings == null)
            {
                userSettings = _currencyXplorer.RequestDefaultUserSettings();
            }

            ViewBag.UserSettings = userSettings;

            return View();
        }

        private void TestCookies()
        {
            string cookieUid = "";
            long uid = DateTime.Now.ToFileTime();

            // Cookies.

            if (Request.Cookies.ContainsKey("uid"))
            {
                cookieUid = Request.Cookies["uid"];
            }

            if (String.IsNullOrWhiteSpace(cookieUid) == false)
            {
                uid = long.Parse(cookieUid);
            }
            else
            {
                Response.Cookies.Append("uid", uid.ToString());
            }

            // Settings.

            UserSettings userSettings = _currencyXplorer.RequestUserSettings(uid.ToString());

            if (userSettings == null)
            {
                userSettings = _currencyXplorer.RequestDefaultUserSettings();
            }

            ViewBag.UserSettings = userSettings;

            ViewBag.Info = $"Cookie: {cookieUid}. Generated: {uid}.";

        }
    }
}
