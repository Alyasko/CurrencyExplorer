namespace CurrencyExplorer.Models.Entities
{
    public class ChartCurrencyDataPoint
    {
        /// <summary>
        /// The position on the chart.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Specific currency data.
        /// </summary>
        public CurrencyData CurrencyDataObject { get; set; }
    }
}
