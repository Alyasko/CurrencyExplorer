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

        public string Language { get; set; }

        public override bool Equals(object obj)
        {
            UserLanguageEntry other = obj as UserLanguageEntry;
            bool result = other != null && this.Equals(other);

            return result;
        }

        protected bool Equals(UserLanguageEntry other)
        {
            return string.Equals(Language, other.Language);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id*397) ^ (Language != null ? Language.GetHashCode() : 0);
            }
        }
    }
}
