using CivicHub.Application.Common.Results;
using CivicHub.Application.Common.Services;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Exceptions;
using MediatR;

namespace CivicHub.Application.Features.Persons.Commands.UploadPersonPicture;

public class UploadPersonPictureCommandHandler(IUnitOfWork unitOfWork, IPersonPictureService pictureService)
    : IRequestHandler<UploadPersonPictureCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadPersonPictureCommand request, CancellationToken cancellationToken)
    {
        var person = await GetPersonAsync(request.PersonId);

        var picturePath = await pictureService.SaveAsync(person.Id, request.File);
        
        person.PictureFullPath = picturePath;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return picturePath;
    }

    private async Task<Person> GetPersonAsync(long personId)
    {
        var person = await unitOfWork.PersonRepository.GetByIdAsync(personId);

        if (person is null)
        {
            throw new PersonDoesntExistException(personId);
        }

        return person;
    }
}