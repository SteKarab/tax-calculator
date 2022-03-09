using FluentValidation;
using TaxCalculator.Dtos;

namespace TaxCalculator.Validation;

public class TaxPayerDtoValidator: AbstractValidator<TaxPayerDto>
{
    private static string _fullNameRegex = @"^([a-zA-Z]*?)(\s)([a-zA-Z\s]*)$";
    private static string _ssnRegex = @"^([0-9]*)$";
    
    public TaxPayerDtoValidator()
    {
        RuleFor(taxPayer => taxPayer.FullName).NotNull().Matches(_fullNameRegex).WithMessage("FullName must contain letters or spaces only and be at least 2 separate words!");

        RuleFor(taxPayer => taxPayer.SSN).NotNull().MinimumLength(5).MaximumLength(10).Matches(_ssnRegex).WithMessage("SSN must only contain digits!");

        RuleFor(taxPayer => taxPayer.GrossIncome).NotNull().Must(BeHigherOrEqualToZero).WithMessage("GrossIncome must be higher than or equal to 0!");

        RuleFor(taxPayer => taxPayer.CharitySpent).Must(BeHigherOrEqualToZero).WithMessage("CharitySpent must be higher than or equal to 0!");;
    }

    private bool BeHigherOrEqualToZero(decimal number)
    {
        return number >= 0;
    }
}