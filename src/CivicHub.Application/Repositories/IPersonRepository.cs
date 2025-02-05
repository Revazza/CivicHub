using CivicHub.Domain.Persons;

namespace CivicHub.Application.Repositories;

public interface IPersonRepository : IGenericRepository<Person, int>
{
    Task<bool> DoesExistAsync(string personalNumber, CancellationToken cancellationToken = default);

    Task<Person> GetForUpdateAsync(string personalNumber, CancellationToken cancellationToken = default);
}