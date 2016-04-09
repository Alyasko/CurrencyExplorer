using System;
using System.Collections.Generic;
using Microsoft.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities.Database
{
    public class CurrencyDataContext : DbContext
    {
        public DbSet<CurrencyData> CurrencyEntries { get; set; }
        public DbSet<CurrencyCode> CurrencyCodes { get; set; }
    }
}
