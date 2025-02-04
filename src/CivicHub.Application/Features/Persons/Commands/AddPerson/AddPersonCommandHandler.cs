using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons;
using MediatR;

namespace CivicHub.Application.Features.Persons.Commands.AddPerson;

public class AddPersonCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddPersonCommand,Result<AddPersonResponse>>
{
    public async Task<Result<AddPersonResponse>> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        var person = ConvertToPerson(request);
        await unitOfWork.PersonRepository.InsertAsync(person);
        return null;
    }

    private static Person ConvertToPerson(AddPersonCommand request)
        => new Person
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PersonalNumber = request.PersonalNumber,
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            PhoneNumber = null,
            Location = null,
            Connections = [],
        };
}