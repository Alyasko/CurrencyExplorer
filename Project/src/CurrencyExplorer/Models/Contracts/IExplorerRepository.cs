﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Entities;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Models.Contracts
{
    public interface IExplorerRepository : ICurrenciesRepository, IUserSettingsRepository
    {
        /// <summary>
        /// Exeutes specified query.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        void ExecuteQuery(string query);
    }
}
