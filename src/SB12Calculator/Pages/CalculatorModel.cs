using System.ComponentModel.DataAnnotations;

namespace SB12Calculator.Pages
{
    public class CalculatorModel
    {
        [Required(ErrorMessage = "You must select a school district")]
        public string? DistrictId { get; set; }

        [Range(1900, 2023, ErrorMessage = "Exemption Qualify Year must be between 1900 and 2023")]
        public int ExemptionQualifyYear { get; set; }

        [Range(0, 100_000_000, ErrorMessage = "Taxable value must be between 0 and 100,000,000")]
        public int TaxableValue2018 { get; set; }
        [Range(0, 100_000_000, ErrorMessage = "Taxable value must be between 0 and 100,000,000")]
        public int TaxableValue2019 { get; set; }
        [Range(0, 100_000_000, ErrorMessage = "Taxable value must be between 0 and 100,000,000")]
        public int TaxableValue2020 { get; set; }
        [Range(0, 100_000_000, ErrorMessage = "Taxable value must be between 0 and 100,000,000")]
        public int TaxableValue2021 { get; set; }
        [Range(0, 100_000_000, ErrorMessage = "Taxable value must be between 0 and 100,000,000")]
        public int TaxableValue2022 { get; set; }

        public decimal TaxesImposedIn2018 { get; set; }

        public decimal NewImprovement2019 { get; set; }
        public decimal NewImprovement2020 { get; set; }
        public decimal NewImprovement2021 { get; set; }
        public decimal NewImprovement2022 { get; set; }
        public decimal NewImprovement2023 { get; set; }

        [Required]
        public decimal MCR2023 { get; set; }

        public CalculatorModel()
        {
            ExemptionQualifyYear = 2018;
            TaxableValue2018 = 100000;
            TaxableValue2019 = (int)(TaxableValue2018 * 1.05);
            TaxableValue2020 = (int)(TaxableValue2019 * 1.05);
            TaxableValue2021 = (int)(TaxableValue2020 * 1.05);
            TaxableValue2022 = (int)(TaxableValue2021 * 1.05);
        }
    }
}
