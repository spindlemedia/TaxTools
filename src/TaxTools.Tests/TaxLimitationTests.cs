using Shouldly;
using TaxTools.Core.TaxLimitation;

namespace TaxTools.Tests
{
    public class TaxLimitationTests
    {
        [Fact]
        public void ShouldCalculateForSingleYear()
        {
            var p = new CalculationParameters
            {
                CalculationYear = 2023,
                ExemptionQualifyYear = 2022
            };
            p.YearDetails.Add(2022,
                new CalculationParameterYearDetail
                {
                    TaxableValue = 100000,
                    CeilingAdjustment = 1000,
                    MCR = 0.8779m,
                    OwnershipPercent = 100
                });
            p.YearDetails.Add(2023,
                new CalculationParameterYearDetail
                {
                    CeilingAdjustment = 0,
                    MCR = 0.8m,
                });
            var result = Calculator.Calculate(p);
            result.CalculatedCeiling.ShouldBe(922.10m);
        }

        [Fact]
        public void ShouldNotReduceWhenMCRIsGreater()
        {
            var beginCeiling = 123.45m;
            var prevMCR = 0.8779m;
            var p = new CalculationParameters
            {
                CalculationYear = 2023,
                ExemptionQualifyYear = 2022
            };
            p.YearDetails.Add(2022,
                new CalculationParameterYearDetail
                {
                    TaxableValue = 100000,
                    CeilingAdjustment = beginCeiling,
                    MCR = prevMCR,
                    OwnershipPercent = 100
                });
            p.YearDetails.Add(2023,
                new CalculationParameterYearDetail
                {
                    CeilingAdjustment = 0,
                    MCR = prevMCR + 0.1m
                });
            var result = Calculator.Calculate(p);
            result.CalculatedCeiling.ShouldBe(beginCeiling);
        }

        [Fact]
        public void ShouldNotReduceWhenMCRIsSame()
        {
            var beginCeiling = 123.45m;
            var prevMCR = 0.8779m;
            var p = new CalculationParameters
            {
                CalculationYear = 2023,
                ExemptionQualifyYear = 2022
            };
            p.YearDetails.Add(2022,
                new CalculationParameterYearDetail
                {
                    TaxableValue = 100000,
                    CeilingAdjustment = beginCeiling,
                    MCR = prevMCR,
                    OwnershipPercent = 100
                });
            p.YearDetails.Add(2023,
                new CalculationParameterYearDetail
                {
                    CeilingAdjustment = 0,
                    MCR = prevMCR
                });
            var result = Calculator.Calculate(p);
            result.CalculatedCeiling.ShouldBe(beginCeiling);
        }
    }
}