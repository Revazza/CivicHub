using System.Text.RegularExpressions;
using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Common.Localization;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.Common.Extensions;

public static class ValidatorExtensions
{
    private const char Separator = ' ';
    private const string EnglishOrGeorgianPattern = @"^([a-zA-Z\s]+|[ა-ჰ\s]+)$";
    private const string EnglishPattern = "^[a-zA-Z]+$";
    private const string OnlyDigitsPattern = @"^\d+$";

    public static IRuleBuilderOptions<T, string> MustBeEnglish<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder)
        => ruleBuilder.Must(input => Regex.IsMatch(input, EnglishPattern))
            .WithMessage(ValidatorMessagesKeys.MustBeEnglish);

    public static IRuleBuilderOptions<T, string> MustBeEnglishOrGeorgian<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder)
        => ruleBuilder.Must(input => Regex.IsMatch(input, EnglishOrGeorgianPattern))
            .WithMessage(ValidatorMessagesKeys.MustBeEnglishOrGeorgian);

    public static IRuleBuilderOptions<T, string> MustBeOneWord<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder)
        => ruleBuilder.Must(HasExactlyOneWord)
            .WithMessage(ValidatorMessagesKeys.MustBeOneWord);

    public static IRuleBuilderOptions<T, string> MustContainOnlyDigits<T>(
        this IRuleBuilderOptions<T, string> ruleBuilder)
        => ruleBuilder.Must(HasOnlyDigits)
            .WithMessage(ValidatorMessagesKeys.MustContainOnlyDigits);

    private static bool HasExactlyOneWord(string input) => input.IsNotNullOrEmpty() && input.Split(Separator).Length == 1;

    private static bool HasOnlyDigits(string input) => input.IsNotNullOrEmpty() && Regex.IsMatch(input, OnlyDigitsPattern);
}