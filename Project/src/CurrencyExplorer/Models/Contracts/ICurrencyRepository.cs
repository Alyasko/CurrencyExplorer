using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyRepository
    {
        /// <summary>
        /// Queries all entries from database.
        /// </summary>
        /// <returns>All entries.</returns>
        IQueryable<CurrencyData> GetEntries();

        /// <summary>
        /// Get entries withing specified time period.
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <returns>Entries withing specified time period.</returns>
        IQueryable<CurrencyData> GetEntries(ChartTimePeriod timePeriod);

        /// <summary>
        /// Get all code entries.
        /// </summary>
        /// <returns>The list of code entries</returns>
        IQueryable<CurrencyCode> GetCodeEntries();

        /// <summary>
        /// Adds entry to the database.
        /// </summary>
        /// <param name="currencyData">The data to be added.</param>
        void AddEntry(CurrencyData currencyData);

        /// <summary>
        /// Adds list of entries to the database.
        /// </summary>
        /// <param name="entries">The list of entries to be added.</param>
        void AddEntries(ICollection<CurrencyData> entries);

        /// <summary>
        /// Adds list of code entries to the database.
        /// </summary>
        /// <param name="codeEntries">The list of code entries to be added.</param>
        void AddCodeEntries(ICollection<CurrencyCode> codeEntries);

        /// <summary>
        /// Adds code entry to the database.
        /// </summary>
        /// <param name="codeEntry">The data to be added.</param>
        void AddCodeEntry(CurrencyCode codeEntry);

        /// <summary>
        /// Removes all database entries from the database that satisfy specified currency code.
        /// </summary>
        /// <param name="entryToRemove">Specified currency to be removed.</param>
        void RemoveEntries(CurrencyCode entryToRemove);

        /// <summary>
        /// Removes all entries within specified time period.
        /// </summary>
        /// <param name="timePeriod">The period of time which entries are to be removed.</param>
        void RemoveEntries(ChartTimePeriod timePeriod);

        /// <summary>
        /// Removes all entries that satisfy specified currency code within period of time.
        /// </summary>
        /// <param name="entryToRemove">Specified currency to be removed.</param>
        /// <param name="timePeriod">The period of time which entries are to be removed.</param>
        void RemoveEntries(CurrencyCode entryToRemove, ChartTimePeriod timePeriod);

        /// <summary>
        /// Exeutes specified query.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        void ExecuteQuery(string query);
    }
}
