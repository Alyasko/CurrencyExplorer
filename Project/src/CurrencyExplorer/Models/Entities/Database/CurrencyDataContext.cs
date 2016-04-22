using System;
using System.Collections.Generic;
using Microsoft.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities.Database
{
    public class CurrencyDataContext : DbContext
    {
        private string _connectionString;

        public CurrencyDataContext()
        {
            _connectionString = "Server=(localdb)\\ProjectsV12;Database=CurrenciesCache;Trusted_Connection=True;";
        }

        public CurrencyDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<CurrencyData> CurrencyEntries { get; set; }
        public DbSet<CurrencyCode> CurrencyCodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
