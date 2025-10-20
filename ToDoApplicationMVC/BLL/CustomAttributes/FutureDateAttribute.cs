using System.ComponentModel.DataAnnotations;

namespace ToDoApplicationMVC.BLL.CustomAttributes;

public class FutureDateAttribute : ValidationAttribute
{
    public string CompareToProperty { get; set; }

    public FutureDateAttribute(string compareToProperty)
    {
        this.CompareToProperty = compareToProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateOnly dateToValidate)
        {
            return new ValidationResult("Invalid date format");
        }

        var propertyInfo = validationContext.ObjectType.GetProperty(this.CompareToProperty);

        if (propertyInfo == null)
        {
            return new ValidationResult($"Unknown property: {this.CompareToProperty}");
        }

        var comparisonValue = propertyInfo.GetValue(validationContext.ObjectInstance);

        if (comparisonValue is not DateOnly dateToCompare)
        {
            return new ValidationResult($"{this.CompareToProperty} must be a DateOnly");
        }

        if (dateToValidate <= dateToCompare)
        {
            return new ValidationResult(this.ErrorMessage ?? $"Date must be after {dateToCompare}");
        }

        return ValidationResult.Success;
    }
}
