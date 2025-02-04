using CivicHub.Domain.Persons;

namespace CivicHub.Application.Repositories;

public interface IPersonRepository : IGenericRepository<Person, int>
{
}