using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace TaxTools.Pages.TaxCreditCalculator
{
    public class CalculatorModel
    {
        [Range(0, 10_000_000_000, ErrorMessage = "Taxable value must be between 0 and 10,000,000,000")]
        public long MarketValue { get; set; }
        public int LimitationAmount { get; set; }
        [Required(ErrorMessage = "M&O Rate is required")]
        public decimal? MORate { get; set; }
        [Required(ErrorMessage = "I&S Rate is required")]
        public decimal? ISRate { get; set; }
        [Required(ErrorMessage = "Tax Credit is required")]
        public decimal? TaxCredit { get; set; }
    }
}
