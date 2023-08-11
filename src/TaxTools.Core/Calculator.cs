using System.Text;

namespace TaxTools.Core
{
    public static class Calculator
    {
        public static CalculationResult Calculate(CalculationParameters parameters)
        {
            var result = new CalculationResult
            {
                Details = new List<CalculationResultDetail>()
            };

            decimal? runningTotal = null;
            if (parameters.CalculationYear == 2023)
            {
                var startYear = parameters.ExemptionQualifyYear < 2019 ? 2018 : parameters.ExemptionQualifyYear;
                for (var year = startYear; year <= parameters.CalculationYear; year++)
                {
                    var detail = CalculateYear(parameters, year, runningTotal);
                    result.Details.Add(detail);
                    runningTotal = detail.RunningTotal;
                }
            }

            result.CalculatedCeiling = runningTotal ?? 0;
            return result;
        }

        private static CalculationResultDetail CalculateYear(CalculationParameters parameters, int year, decimal? runningTotal)
        {
            var mcrText = year == 2018 ? "T1 M&O Rate" : "MCR";
            var curYear = parameters.YearDetails[year];
            var result = new CalculationResultDetail
            {
                Year = year
            };
            if (!runningTotal.HasValue)
            {
                result.StartingAmount = curYear.CeilingAdjustment;
            }
            else
            {
                result.StartingAmount = runningTotal.Value;
                result.AdditionalImprovement = curYear.CeilingAdjustment;
            }

            result.TaxableValue = (int)Math.Round(curYear.TaxableValue * (curYear.OwnershipPercent / 100), 0);

            if (year < parameters.CalculationYear)
            {
                var nextYear = parameters.YearDetails[year + 1];
                
                if (parameters.EnableSB2Calculation && year + 1 == 2023 && parameters.ExemptionQualifyYear < 2022)
                {
                    var exemptAmount = 15000 * (curYear.OwnershipPercent / 100);
                    result.SB2Reduction = Math.Round(exemptAmount * curYear.TaxRate / 100, 2);
                    result.SB2CalculationText = $"({exemptAmount:N0} x {curYear.TaxRate}) / 100";
                }

                var amount =
                    Math.Round(result.TaxableValue * (curYear.MCR - nextYear.MCR) / 100, 2);
                result.SB12Reduction = Math.Max(amount, 0);
                result.SB12CalculationText =
                    $"{result.TaxableValue:N0} x ({year} {mcrText} ({curYear.MCR}) - {year + 1} MCR ({nextYear.MCR})) / 100";
            }
            else
            {
                if (parameters.ExemptionQualifyYear > 2022 || !parameters.EnableSB2Calculation)
                    return result;
                var exemptAmount = 60000 * (curYear.OwnershipPercent / 100);
                result.SB2Reduction = Math.Round(exemptAmount * curYear.TaxRate / 100, 2);
                result.SB2CalculationText = $"({exemptAmount:N0} x {curYear.TaxRate}) / 100";
            }

            return result;
        }
    }
}
