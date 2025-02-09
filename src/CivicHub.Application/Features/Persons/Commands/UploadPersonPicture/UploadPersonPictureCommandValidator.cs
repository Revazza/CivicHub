using CivicHub.Application.Features.Persons.Commands.Common.Validators;
using FluentValidation;

namespace CivicHub.Application.Features.Persons.Commands.UploadPersonPicture;

public class UploadPersonPictureCommandValidator : AbstractValidator<UploadPersonPictureCommand>
{
    public UploadPersonPictureCommandValidator()
    {
        RuleFor(request => request.PersonId)
            .GreaterThan(0);

        RuleFor(request => request.File)
            .SetValidator(new FileValidator());
    }
}