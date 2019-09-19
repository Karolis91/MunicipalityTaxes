namespace MunicipalityTaxes.Models.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PriorityAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (!Enum.IsDefined(typeof(PriorityEnum), value))
            {
                return new ValidationResult("Priority does not exist");
            }

            return ValidationResult.Success;
        }
    }
}
