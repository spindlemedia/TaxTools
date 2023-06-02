namespace TaxTools.Core
{
    public class CalculationResult
    {
        public List<CalculationResultDetail> Details { get; set; }
        public decimal CalculatedCeiling { get; set; }
    }

    public class CalculationResultDetail
    {
        public int Year { get; set; }
        public decimal StartingAmount { get; set; }
        public decimal AdditionalImprovement { get; set; }
        public int TaxableValue { get; set; }
        public decimal SB12Reduction { get; set; }
        public decimal? SB1Reduction { get; set; }
        public decimal RunningTotal => Math.Max(StartingAmount + AdditionalImprovement - SB12Reduction - (SB1Reduction ?? 0), 0);

        public string SB12CalculationText { get; set; }
        public string SB1CalculationText { get; set; }
    }
}
