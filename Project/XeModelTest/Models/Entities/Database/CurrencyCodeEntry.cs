

namespace CurrencyExplorer.Models.Entities.Database
{
    public class CurrencyCodeEntry
    {
        public CurrencyCodeEntry()
        {
            
        }

        public CurrencyCodeEntry(string value, string alias)
        {
            Value = value;
            Alias = alias;
        }


        public int Id { get; set; }

        /// <summary>
        /// String representation of code.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The description of the currency code.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The alias for Value. For example USD, UAH.
        /// </summary>
        public string Alias { get; set; }

        public override string ToString()
        {
            return Value;
        }

        #region EqualityMembers

        public override bool Equals(object obj)
        {
            CurrencyCodeEntry dbCurrency = obj as CurrencyCodeEntry;

            if (dbCurrency == null)
            {
                return false;
            }

            return string.Equals(Value, dbCurrency.Value);
        }

        protected bool Equals(CurrencyCodeEntry other)
        {
            return string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        #endregion

    }
}
