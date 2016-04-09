using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyRepository
    {
        IQueryable<CurrencyData> GetAllEntries();
        void AddEntry(CurrencyData currencyData);
        void ExecuteQuery(string query);
    }
}
