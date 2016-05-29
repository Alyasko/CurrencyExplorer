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

        public DateTime ChartBeginTime { get; set; }

        public DateTime ChartEndTime { get; set; }

        public UserLanguageEntry Language { get; set; }

        public override bool Equals(object obj)
        {
            UserSettingsEntry other = obj as UserSettingsEntry;
            bool result = other != null && this.Equals(other);

            return result;
        }

        protected bool Equals(UserSettingsEntry other)
        {
            return CookieUid == other.CookieUid;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ CookieUid.GetHashCode();
                hashCode = (hashCode * 397) ^ ChartBeginTime.GetHashCode();
                hashCode = (hashCode * 397) ^ ChartEndTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (Language != null ? Language.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
