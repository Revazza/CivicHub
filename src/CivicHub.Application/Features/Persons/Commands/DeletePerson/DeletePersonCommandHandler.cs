using CivicHub.Application.Common.Results;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CivicHub.Application.Features.Persons.Commands.DeletePerson;

public class DeletePersonCommandHandler(IUnitOfWork unitOfWork, ILogger<DeletePersonCommandHandler> logger)
    : IRequestHandler<DeletePersonCommand, Result>
{
    public async Task<Result> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await GetPersonToDeleteAsync(request.PersonalNumber, cancellationToken);
        await DeletePersonAsync(person, cancellationToken);
        return Result.Success();
    }

    private async Task<Person> GetPersonToDeleteAsync(string personalNumber, CancellationToken cancellationToken)
    {
        var person = await unitOfWork.PersonRepository.GetByPersonalNumberAsync(personalNumber, cancellationToken);

        if (person is null)
        {
            throw new PersonDoesntExistException(personalNumber);
        }

        return person;
    }

    private async Task DeletePersonAsync(Person person, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Transaction started for Person ID: {PersonId}", person.Id);
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            await ExecuteNecessaryDeletionsAsync(person, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Deleted Person with ID: {PersonId}", person.Id);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            logger.LogInformation("Transaction commited for Person ID: {PersonId}", person.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Transaction failed for Person ID: {PersonId}", person.Id);

            await unitOfWork.RollbackTransactionAsync(cancellationToken);

            logger.LogInformation("Transaction rolled back for Person ID: {PersonId}", person.Id);
            throw;
        }
    }

    private async Task ExecuteNecessaryDeletionsAsync(Person person, CancellationToken cancellationToken)
    {
        await DeletePersonConnectionsAndSaveAsync(person.Id, cancellationToken);
        DeletePerson(person);
    }

    private async Task DeletePersonConnectionsAndSaveAsync(long personId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting Person connections");
        await unitOfWork.PersonConnectionRepository.ExecuteDeleteAsync(personId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Deleted Person connections");
    }

    private void DeletePerson(Person person)
    {
        unitOfWork.PersonRepository.Delete(person);
        logger.LogInformation("Delete Person");
    }
}