using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using XeModelTest;

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

            return uid;
        }

        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
    }
}
