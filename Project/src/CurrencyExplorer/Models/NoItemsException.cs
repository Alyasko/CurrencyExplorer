using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models
{
    public class NoItemsException : Exception
    {
        public NoItemsException()
        {
            
        }

        public NoItemsException(string message) : base(message)
        {
            
        }
    }
}
