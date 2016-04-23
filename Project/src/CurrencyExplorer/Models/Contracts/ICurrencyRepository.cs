using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ICurrencyRepository
    {
        /// <summary>
        /// Queries all entries from database.
        /// </summary>
        /// <returns>All entries.</returns>
        IQueryable<CurrencyDataEntry> GetDataEntries();

        /// <summary>
        /// Get entries withing specified time period.
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <returns>Entries withing specified time period.</returns>
        IQueryable<CurrencyDataEntry> GetDataEntries(ChartTimePeriod timePeriod);

        /// <summary>
        /// Get all code entries.
        /// </summary>
        /// <returns>The list of code entries</returns>
        IQueryable<CurrencyCodeEntry> GetCodeEntries();

        /// <summary>
        /// Adds entry to the database.
        /// </summary>
        /// <param name="currencyData">The data to be added.</param>
        void AddDataEntry(CurrencyDataEntry currencyData);

        /// <summary>
        /// Adds list of entries to the database.
        /// </summary>
        /// <param name="entries">The list of entries to be added.</param>
        void AddDataEntries(ICollection<CurrencyDataEntry> entries);

        /// <summary>
        /// Adds list of code entries to the database.
        /// </summary>
        /// <param name="codeEntries">The list of code entries to be added.</param>
        void AddCodeEntries(ICollection<CurrencyCodeEntry> codeEntries);

        /// <summary>
        /// Adds code entry to the database.
        /// </summary>
        /// <param name="codeEntryEntry">The data to be added.</param>
        void AddCodeEntry(CurrencyCodeEntry codeEntryEntry);

        /// <summary>
        /// Removes all database entries from the database that satisfy specified currency code.
        /// </summary>
        /// <param name="entryToRemove">Specified currency to be removed.</param>
        void RemoveDataEntries(CurrencyCodeEntry entryToRemove);

        /// <summary>
        /// Removes all entries within specified time period.
        /// </summary>
        /// <param name="timePeriod">The period of time which entries are to be removed.</param>
        void RemoveDataEntries(ChartTimePeriod timePeriod);

        /// <summary>
        /// Removes all entries that satisfy specified currency code within period of time.
        /// </summary>
        /// <param name="entryToRemove">Specified currency to be removed.</param>
        /// <param name="timePeriod">The period of time which entries are to be removed.</param>
        void RemoveDataEntries(CurrencyCodeEntry entryToRemove, ChartTimePeriod timePeriod);

        /// <summary>
        /// Exeutes specified query.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        void ExecuteQuery(string query);
    }
}
