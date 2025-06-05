namespace TaxTools.Core.TaxLimitation
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
        //2023 only
        public decimal? SB2Reduction { get; set; }
        //2025 only
        public decimal? SB4Reduction { get; set; }
        //2025 only
        public decimal? SB23Reduction { get; set; }
        public decimal RunningTotal => Math.Max(StartingAmount + AdditionalImprovement - SB12Reduction
            - (SB2Reduction ?? 0) - (SB4Reduction ?? 0) - (SB23Reduction ?? 0), 0);

        public string SB12CalculationText { get; set; }
        public string SB2CalculationText { get; set; }
        public string SB4CalculationText { get; set; }
        public string SB23CalculationText { get; set; }
    }
}
