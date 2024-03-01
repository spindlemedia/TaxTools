using System.ComponentModel.DataAnnotations;

namespace TaxTools.Pages.TaxLimitationCalculator
{
    public class DetailModel
    {
        private readonly CalculatorModel _parent;

        public int Year { get; }
        [Range(0, 100_000_000, ErrorMessage = "Taxable value must be between 0 and 100,000,000")]
        public int TaxableValue { get; set; }
        public decimal CeilingAdjustment { get; set; }

        [Range(1, 100, ErrorMessage = "Percent must be between 1 and 100")]
        public decimal OwnershipPercent { get; set; }

        [Required(ErrorMessage = "MCR is required")]
        public decimal? MCR { get; set; }

        [TaxRateConditionallyRequired]
        public decimal? TaxRate { get; set; }

        public bool MCRPopulated { get; set; }
        public bool TaxRatePopulated { get; set; }

        public bool RequireTaxRate => _parent.EnableSB2Calculation &&
                                      (( _parent.ExemptionQualifyYear <= 2022 && Year == 2023) || (_parent.ExemptionQualifyYear <= 2021 && Year == 2022));

        public DetailModel(CalculatorModel parent, int year)
        {
            _parent = parent;
            Year = year;
            OwnershipPercent = 100;
        }
    }

    public class CalculatorModel
    {
        [Required(ErrorMessage = "You must select a school district")]
        public string? DistrictId { get; set; }

        [Range(2023, 2050, ErrorMessage = "Tax Year must be 2023 or greater")]
        public int TaxYear { get; set; }

        [Range(1900, 2050, ErrorMessage = "Exemption Qualify Year must be greater than 1900")]
        public int ExemptionQualifyYear { get; set; }

        public bool EnableSB2Calculation { get; set; }

        [ValidateComplexType]
        public List<DetailModel> Details { get; set; }

        public CalculatorModel()
        {
            Details = new List<DetailModel>();
            EnableSB2Calculation = true;
            TaxYear = 2023;
        }
    }
}
