using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons;
using CivicHub.Persistance.Contexts.CivicHubContexts;

namespace CivicHub.Infrastructure.Repositories;

public class PersonRepository(CivicHubContext context)
    : GenericRepository<Person, int>(context), IPersonRepository
{
    
}