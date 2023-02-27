namespace SB12Calculator.Core
{
    public static class Calculator
    {
        public static CalculationResult Calculate(CalculationParameters parameters)
        {
            var result = new CalculationResult
            {
                Output = new List<string>()
            };

            decimal runningTotal = 0;
            if (parameters.CalculationYear == 2023)
            {
                var startYear = parameters.ExemptionQualifyYear < 2019 ? 2018 : parameters.ExemptionQualifyYear;
                for (var year = startYear; year < parameters.CalculationYear; year++)
                {
                    runningTotal = CalculateYear(parameters, result, year, runningTotal, year == startYear);
                }
            }

            result.CalculatedCeiling = runningTotal;
            return result;
        }

        private static decimal CalculateYear(CalculationParameters parameters, CalculationResult result, int year, decimal input, bool firstYear)
        {
            var mcrText = year == 2018 ? "T1 M&O Rate" : "MCR";
            var curYear = parameters.YearDetails[year];
            var nextYear = parameters.YearDetails[year + 1];

            if (firstYear)
                result.Output.Add($"Taxes imposed in {year}: {curYear.TaxesOrNewImprovement:C2}");
            else
                result.Output.Add($"{input:C2} + {curYear.TaxesOrNewImprovement:C2} (new improvement in {year})");

            input += curYear.TaxesOrNewImprovement;

            var amount =
                Math.Round(curYear.TaxableValue * (curYear.MCR - nextYear.MCR) / 100, 2);
            var adjustment = Math.Max(amount, 0);
            input = Math.Max(input - adjustment, 0);
            result.Output.Add(
                $"{year} Taxable Value ({curYear.TaxableValue}) x ({year} {mcrText} ({curYear.MCR}) - {year + 1} MCR ({nextYear.MCR})) / 100 = {amount:N2}. Reduction: {adjustment:C2}. Running Total: {input:C2}");
            return input;
        }
    }
}
