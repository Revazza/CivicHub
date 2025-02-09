using FluentValidation;

namespace CivicHub.Application.Features.Persons.Common.Validators;

public class AgeValidator : AbstractValidator<DateTime>
{
    private const int ValidAge = 18;
    private const int MagicAge = 120;
    
    public AgeValidator()
    {
        RuleFor(birthDate => birthDate)
            .Must(IsAdult)
            .WithMessage("Must be at least 18 years old")
            .Must(IsMagicAge)
            .WithMessage("Magic age entered");
    }

    private static bool IsAdult(DateTime birthDate) => birthDate <= DateTime.UtcNow.AddYears(-ValidAge);

    private static bool IsMagicAge(DateTime birthDate) => birthDate >= DateTime.UtcNow.AddYears(-MagicAge);
}