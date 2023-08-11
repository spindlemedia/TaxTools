using Shouldly;
using TaxTools.Core.TaxCredit;

namespace TaxTools.Tests
{
    public class TaxCreditTests
    {
        [Fact]
        public void ShouldCalculateWhenCreditHigherThanLimitedLevy()
        {
            var p = new CalculationParameters
            {
                MarketValue = 705_192_480,
                TaxCredit = 581_729.84m,
                LimitationAmount = 30_000_000,
                MORate = 0.8646m,
                ISRate = 0.1517m
            };
  
            var result = Calculator.Calculate(p);
            result.LevyPreCredit.ShouldBe(1_329_156.99m);
            result.LimitationExemptionAmount.ShouldBe(492_700_824);
            result.TaxCreditExemptionAmount.ShouldBe(212_491_656);
        }
    }
}