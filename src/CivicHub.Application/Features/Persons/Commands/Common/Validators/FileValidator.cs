using CivicHub.Application.Common.Localization;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CivicHub.Application.Features.Persons.Commands.Common.Validators;

public class FileValidator : AbstractValidator<IFormFile>
{
    private const long TenMb = 10 * 1024 * 1024;
    private const long MaxFileSize = TenMb * 1024 * 1024;
    private static readonly string[] AllowedFileExtensions = [".png"];
    private const int MaxFileNameLength = 255;

    public FileValidator()
    {
        RuleFor(file => file.Length)
            .LessThanOrEqualTo(MaxFileSize)
            .WithMessage(ValidatorMessagesKeys.FileSizeExceeded);

        RuleFor(file => file.FileName)
            .Must(BeAValidExtension)
            .WithMessage(ValidatorMessagesKeys.InvalidFileType);

        RuleFor(file => file.FileName)
            .MinimumLength(1)
            .MaximumLength(MaxFileNameLength);
    }

    private static bool BeAValidExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return AllowedFileExtensions.Contains(extension);
    }
}