using System.ComponentModel.DataAnnotations;

namespace SB12Calculator.Pages
{
    public class CalculatorYearDetailModel
    {
        public int Year { get; set; }
        [Range(0, 100_000_000, ErrorMessage = "Taxable value must be between 0 and 100,000,000")]
        public int TaxableValue { get; set; }
        [Range(0f, 100_000_000f, ErrorMessage = "Amount must be between 0 and 100,000,000")]
        public decimal CeilingAdditionalImprovement { get; set; }

        [Required(ErrorMessage = "MCR is required")]
        public decimal? MCR { get; set; }

        public bool MCRPopulated { get; set; }
    }

    public class CalculatorModel
    {
        [Required(ErrorMessage = "You must select a school district")]
        public string? DistrictId { get; set; }

        [Range(1900, 2023, ErrorMessage = "Exemption Qualify Year must be between 1900 and 2023")]
        public int ExemptionQualifyYear { get; set; }

        [ValidateComplexType]
        public List<CalculatorYearDetailModel> Details { get; set; }

        public CalculatorModel()
        {
            Details = new List<CalculatorYearDetailModel>();
        }
    }
}
