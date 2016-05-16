using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace CurrencyExplorer.Utilities
{
    public class CookiesManager
    {
        public CookiesManager(HttpRequest request, HttpResponse response)
        {
            Request = request;
            Response = response;
        }

        public long GetUid()
        {
            long uid = -1;

            if (Request.Cookies.ContainsKey(Constants.CookieUidName))
            {
                string cookie = Request.Cookies[Constants.CookieUidName];
                if (long.TryParse(cookie, out uid) == false)
                {
                    throw new Exception("Broken UID-cookie.");
                }
                Debug.WriteLine($"Cookie got from client: {cookie}");
            }
            else
            {
                uid = DateTime.Now.ToFileTime();

                // Save cookies
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(20);

                Response.Cookies.Append(Constants.CookieUidName, uid.ToString(), cookieOptions);
            }

            return uid;
        }

        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
    }
}
