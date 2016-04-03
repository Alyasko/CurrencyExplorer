namespace CurrencyExplorer.Models.Entities
{
    public class ChartCurrencyDataPoint<T>
    {
        /// <summary>
        /// Specific currency data.
        /// </summary>
        public T DataObject { get; set; }
    }
}
