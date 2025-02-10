using CivicHub.Application.Common.Localization;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class AgeValidator : AbstractValidator<DateTime>
{
    private const int ValidAge = 18;
    private const int MagicAge = 120;
    
    public AgeValidator()
    {
        RuleFor(birthDate => birthDate)
            .Must(IsAdult)
            .WithName(ValidatorMessagesKeys.Age)
            .WithMessage(ValidatorMessagesKeys.MustBeAdult)
            .Must(IsMagicAge)
            .WithMessage(ValidatorMessagesKeys.MagicNumberEntered);
    }

    private static bool IsAdult(DateTime birthDate) => birthDate <= DateTime.UtcNow.AddYears(-ValidAge);

    private static bool IsMagicAge(DateTime birthDate) => birthDate >= DateTime.UtcNow.AddYears(-MagicAge);
}