namespace TaxTools.Core.TaxCredit
{
    public class CalculationParameters
    {
        public long MarketValue { get; set; }
        public int LimitationAmount { get; set; }
        public decimal MORate { get; set; }
        public decimal ISRate { get; set; }
        public decimal TotalTaxRate => MORate + ISRate;
        public decimal TaxCredit { get; set; }
    }
}
