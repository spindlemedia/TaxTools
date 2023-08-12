
namespace TaxTools.Core.TaxCredit
{
    public static class Calculator
    {
        public static CalculationResult Calculate(CalculationParameters parameters)
        {
            var limitationExemption = Math.Max(parameters.MarketValue - parameters.LimitationAmount, 0);
            var limitedLevy = Math.Round(parameters.LimitationAmount * parameters.TotalTaxRate / 100, 2);
            var isLevy = Math.Round(limitationExemption * parameters.ISRate / 100, 2);
            var levyPreCredit = limitedLevy + isLevy;
            decimal taxDue;
            long effectiveLimitationExemption;
            long taxCreditExemption;

            if (parameters.TaxCredit <= limitedLevy)
            {
                taxDue = limitedLevy - parameters.TaxCredit;
                effectiveLimitationExemption = limitationExemption;
                taxCreditExemption = (long)Math.Round(parameters.TaxCredit / parameters.TotalTaxRate * 100, 0);
            }
            else
            {
                taxDue = levyPreCredit - parameters.TaxCredit;
                effectiveLimitationExemption = (long)Math.Round(taxDue / (parameters.ISRate / 100), 0);
                taxCreditExemption = parameters.MarketValue - effectiveLimitationExemption;
            }

            var result = new CalculationResult
            {
                LevyPreCredit = levyPreCredit,
                LimitationExemptionAmount = effectiveLimitationExemption,
                TaxCreditExemptionAmount = taxCreditExemption,
                CalculatedTax = taxDue
            };

            return result;
        }
    }
}
