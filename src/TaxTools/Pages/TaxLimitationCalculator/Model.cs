using System.ComponentModel.DataAnnotations;

namespace TaxTools.Pages.TaxLimitationCalculator
{
    public class Model
    {
        public int Year { get; set; }
        [Range(0, 100_000_000, ErrorMessage = "Taxable value must be between 0 and 100,000,000")]
        public int TaxableValue { get; set; }
        public decimal CeilingAdjustment { get; set; }

        [Range(1, 100, ErrorMessage = "Percent must be between 1 and 100")]
        public decimal OwnershipPercent { get; set; }

        [Required(ErrorMessage = "MCR is required")]
        public decimal? MCR { get; set; }

        public bool MCRPopulated { get; set; }

        public Model()
        {
            OwnershipPercent = 100;
        }
    }

    public class CalculatorModel
    {
        [Required(ErrorMessage = "You must select a school district")]
        public string? DistrictId { get; set; }

        [Range(1900, 2023, ErrorMessage = "Exemption Qualify Year must be between 1900 and 2023")]
        public int ExemptionQualifyYear { get; set; }

        [ValidateComplexType]
        public List<Model> Details { get; set; }

        public CalculatorModel()
        {
            Details = new List<Model>();
        }
    }
}
