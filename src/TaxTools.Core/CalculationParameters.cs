namespace TaxTools.Core
{
    public class CalculationParameters
    {
        public int ExemptionQualifyYear { get; set; }
        public int CalculationYear { get; set; }
        public Dictionary<int, CalculationParameterYearDetail> YearDetails { get; }

        public CalculationParameters()
        {
            YearDetails = new Dictionary<int, CalculationParameterYearDetail>();
        }
    }

    public class CalculationParameterYearDetail
    {
        public decimal MCR { get; set; }
        public int TaxableValue { get; set; }
        public decimal OwnershipPercent { get; set; }
        public decimal CeilingAdjustment { get; set; }
    }
}
