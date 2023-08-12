namespace TaxTools.Core.TaxCredit
{
    public class CalculationResult
    {
        public decimal LevyPreCredit { get; set; }
        public long LimitationExemptionAmount { get; set; }
        public long TaxCreditExemptionAmount { get; set; }
        public decimal CalculatedTax { get; set; }
    }
}
