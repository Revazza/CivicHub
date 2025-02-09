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
            .WithMessage("File size must not exceed 10 MB.");

        RuleFor(file => file.FileName)
            .Must(BeAValidExtension)
            .WithMessage($"Invalid file type. Only {string.Join(",", AllowedFileExtensions)} is allowed)");

        RuleFor(file => file.FileName)
            .Length(1, MaxFileNameLength)
            .WithMessage($"File name must be between 1 and {MaxFileNameLength} characters.");
    }

    private static bool BeAValidExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return AllowedFileExtensions.Contains(extension);
    }
}