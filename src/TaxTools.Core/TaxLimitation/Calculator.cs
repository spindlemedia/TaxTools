using System.Text;

namespace TaxTools.Core.TaxLimitation
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
            short startYear;

            if (parameters.CalculationYear == 2023)
            {
                startYear = (short)(parameters.ExemptionQualifyYear < 2019 ? 2018 : parameters.ExemptionQualifyYear);
            }
            else
            {
                startYear = (short)(parameters.CalculationYear - 1);
            }

            for (var year = startYear; year <= parameters.CalculationYear; year++)
            {
                var detail = CalculateYear(parameters, year, runningTotal);
                result.Details.Add(detail);
                runningTotal = detail.RunningTotal;
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

            result.TaxableValue = curYear.TaxableValue;

            if (year < parameters.CalculationYear)
            {
                var nextYear = parameters.YearDetails[year + 1];
                var amount =
                    Math.Round(result.TaxableValue * (curYear.MCR - nextYear.MCR) / 100, 2);
                result.SB12Reduction = Math.Max(amount, 0);
                result.SB12CalculationText =
                    $"{result.TaxableValue:N0} x ({year} {mcrText} ({curYear.MCR}) - {year + 1} MCR ({nextYear.MCR})) / 100";
            }
            else
            {
                switch (parameters.CalculationYear)
                {
                    case 2023:
                        Handle2023Calculation(parameters, year, result, curYear);
                        break;
                    case 2025:
                        Handle2025Calculation(parameters, result, curYear);
                        break;
                }
            }

            return result;
        }

        private static void Handle2023Calculation(CalculationParameters parameters, int year, CalculationResultDetail result,
            CalculationParameterYearDetail curYear)
        {
            var sb = new StringBuilder();
            if (parameters.ExemptionQualifyYear < 2022)
            {
                var prevYear = parameters.YearDetails[year - 1];
                var exemptAmount = 15000 * (prevYear.OwnershipPercent / 100);
                result.SB2Reduction = Math.Round(exemptAmount * prevYear.TaxRate / 100, 2);
                sb.Append($"({exemptAmount:N0} x {prevYear.TaxRate} / 100) + ");
            }

            if (parameters.ExemptionQualifyYear < 2023)
            {
                var exemptAmount = 60000 * (curYear.OwnershipPercent / 100);
                var sb2Amount = Math.Round(exemptAmount * curYear.TaxRate / 100, 2);
                if (result.SB2Reduction == null)
                    result.SB2Reduction = sb2Amount;
                else
                    result.SB2Reduction += sb2Amount;
                sb.Append($"({exemptAmount:N0} x {curYear.TaxRate} / 100)");
            }

            result.SB2CalculationText = sb.ToString();
        }

        private static void Handle2025Calculation(CalculationParameters parameters, CalculationResultDetail result,
            CalculationParameterYearDetail curYear)
        {
            if (parameters.ExemptionQualifyYear > 2024)
                return;

            if (parameters.EnableSB4Calculation)
            {
                var sb = new StringBuilder();
                var exemptAmount = 40000 * (curYear.OwnershipPercent / 100);
                var amount = Math.Round(exemptAmount * curYear.TaxRate / 100, 2);
                if (result.SB4Reduction == null)
                    result.SB4Reduction = amount;
                else
                    result.SB4Reduction += amount;
                sb.Append($"({exemptAmount:N0} x {curYear.TaxRate} / 100)");

                result.SB4CalculationText = sb.ToString();
            }

            if (parameters.EnableSB23Calculation)
            {
                var sb = new StringBuilder();
                var exemptAmount = 50000 * (curYear.OwnershipPercent / 100);
                var amount = Math.Round(exemptAmount * curYear.TaxRate / 100, 2);
                if (result.SB23Reduction == null)
                    result.SB23Reduction = amount;
                else
                    result.SB23Reduction += amount;
                sb.Append($"({exemptAmount:N0} x {curYear.TaxRate} / 100)");

                result.SB23CalculationText = sb.ToString();
            }
        }
    }
}
