using FluentValidation;

namespace CivicHub.Application.Features.Persons.Queries.GetFullInformation;

public class GetFullInformationQueryValidator : AbstractValidator<GetFullInformationQuery>
{
    public GetFullInformationQueryValidator()
    {
        RuleFor(request => request.PersonId)
            .GreaterThan(0);
    }
}