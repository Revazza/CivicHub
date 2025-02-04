using System.Text.RegularExpressions;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Extensions;

public static class ValidatorExtensions
{
    private const char Separator = ' ';

    public static IRuleBuilderOptions<T, string> MustBeEnglishOrGeorgian<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(input => Regex.IsMatch(input, @"^([a-zA-Z\s]+|[ა-ჰ\s]+)$"))
            .WithMessage("'{PropertyName}' must be English or Georgian");
    }

    public static IRuleBuilderOptions<T, string> MustBeOneWord<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder, string propertyName)
    {
        return ruleBuilder.Must(lastname => !HasMoreThanOneWord(lastname))
            .WithMessage($"{propertyName} should not be more than one word");
    }

    private static bool HasMoreThanOneWord(string lastname) => lastname.Split(Separator).Length > 1;
}