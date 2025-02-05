using System.Text.RegularExpressions;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Extensions;

public static class ValidatorExtensions
{
    private const char Separator = ' ';
    private const string EnglishOrGeorgianPattern = @"^([a-zA-Z\s]+|[ა-ჰ\s]+)$";
    private const string EnglishPattern = @"^[a-zA-Z\s]+$";
    private const string OnlyDigitsPattern = @"^\d+$";

    public static IRuleBuilderOptions<T, string> MustBeEnglish<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder)
        => ruleBuilder.Must(input => Regex.IsMatch(input, EnglishPattern))
            .WithMessage("'{PropertyName}' must be English");

    public static IRuleBuilderOptions<T, string> MustBeEnglishOrGeorgian<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder)
        => ruleBuilder.Must(input => Regex.IsMatch(input, EnglishOrGeorgianPattern))
            .WithMessage("'{PropertyName}' must be English or Georgian");

    public static IRuleBuilderOptions<T, string> MustBeOneWord<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder, string propertyName)
        => ruleBuilder.Must(HasExactlyOneWord)
            .WithMessage($"{propertyName} should not be more than one word");

    public static IRuleBuilderOptions<T, string> MustContainOnlyDigits<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder, string propertyName)
        => ruleBuilder.Must(HasOnlyDigits)
            .WithMessage($"{propertyName} should contain only digits");

    public static IRuleBuilderOptions<T, string> MustContainOnlyDigits<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder)
        => ruleBuilder.Must(HasOnlyDigits)
            .WithMessage("{PropertyName} should contain only digits");

    private static bool HasExactlyOneWord(string lastname) => lastname.Split(Separator).Length == 1;

    private static bool HasOnlyDigits(string personalNumber) => Regex.IsMatch(personalNumber, OnlyDigitsPattern);
}