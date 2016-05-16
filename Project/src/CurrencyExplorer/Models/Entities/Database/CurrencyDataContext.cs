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

        public DbSet<CurrencyDataEntry> CurrencyDataEntries { get; set; }
        public DbSet<CurrencyCodeEntry> CurrencyCodesEntries { get; set; }

        public DbSet<UserLanguageEntry> UserLanguageEntries { get; set; } 
        public DbSet<UserSettingsEntry> UserSettingsEntries { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
