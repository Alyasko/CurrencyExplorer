using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities.Database
{
    public class CorrespondanceEntry
    {
        public CorrespondanceEntry()
        {
            
        }

        public int Id { get; set; }

        public CurrencyCodeEntry CurrencyCode { get; set; }

        public UserSettingsEntry UserSettings { get; set; }

        public override bool Equals(object obj)
        {
            CorrespondanceEntry other = obj as CorrespondanceEntry;
            bool result = other != null && this.Equals(other);

            return result;
        }

        protected bool Equals(CorrespondanceEntry other)
        {
            bool result = CurrencyCode != null && UserSettings != null;
            return result && CurrencyCode.Equals(other.CurrencyCode) && UserSettings.Equals(other.UserSettings);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode*397) ^ (CurrencyCode != null ? CurrencyCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (UserSettings != null ? UserSettings.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
