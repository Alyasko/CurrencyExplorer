using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeModelTest
{
    public class IConfiguration
    {
        private Dictionary<string, string> asd = new Dictionary<string, string>();

        public string this[string index]
        {
            get
            {
                return asd[index];
            }

            set
            {
                asd[index] = value;
            }
        }
    }

    public interface IApplicationEnvironment
    {
        
    }

    public class HttpRequest
    {
        
    }

    public class HttpResponse
    {
        
    }

    public class CookieOptions
    {
        
    }
}
