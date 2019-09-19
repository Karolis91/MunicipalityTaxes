namespace MunicipalityTaxes.Models.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DatesAreValidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var municipalityTax = (MunicipalityTax)validationContext.ObjectInstance;
            var dateTo = (DateTime)value;

            if (municipalityTax.DateFrom == default(DateTime) || dateTo == default(DateTime))
            {
                return new ValidationResult("DateTo and DateFrom must be filled in");
            }

            if (municipalityTax.DateFrom.AddDays(1) != dateTo &&
                municipalityTax.DateFrom.AddDays(7) != dateTo &&
                municipalityTax.DateFrom.AddMonths(1) != dateTo &&
                municipalityTax.DateFrom.AddYears(1) != dateTo)
            {
                return new ValidationResult("Only daily, weekly, monthly and yearly dates are allowed");
            }

            return ValidationResult.Success;
        }
    }
}
