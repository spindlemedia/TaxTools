using System.ComponentModel.DataAnnotations;

namespace TaxTools.Pages.TaxLimitationCalculator
{
    public class TaxRateConditionallyRequired : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var model = (DetailModel)context.ObjectInstance;
            if (!model.RequireTaxRate)
                return ValidationResult.Success;

            var taxRate = value as decimal?;
            return taxRate is > 0 ? ValidationResult.Success : new ValidationResult($"Tax Rate is required for {model.Year}");
        }
    }
}
