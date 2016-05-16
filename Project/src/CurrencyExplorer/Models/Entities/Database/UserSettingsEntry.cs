using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Enums;

namespace CurrencyExplorer.Models.Entities.Database
{
    public class UserSettingsEntry
    {
        public UserSettingsEntry()
        {
            
        }

        [Key]
        public int Id { get; set; }

        public long CookieUid { get; set; }

        public ICollection<CurrencyCodeEntry> CurrencyCodes { get; set; } 

        public DateTime ChartBeginTime { get; set; }

        public DateTime ChartEndTime { get; set; }

        public UserLanguageEntry Language { get; set; }
    }
}
