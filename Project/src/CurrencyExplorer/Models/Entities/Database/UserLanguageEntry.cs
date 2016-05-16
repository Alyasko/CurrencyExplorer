using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Enums;

namespace CurrencyExplorer.Models.Entities.Database
{
    public class UserLanguageEntry
    {
        public UserLanguageEntry()
        {
            
        }

        [Key]
        public int Id { get; set; }

        public CurrencyExplorerLanguage Language { get; set; }
    }
}
