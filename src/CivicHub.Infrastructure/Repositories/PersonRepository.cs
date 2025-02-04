using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons;
using CivicHub.Persistance.Contexts.CivicHubContexts;

namespace CivicHub.Infrastructure.Repositories;

public class PersonRepository : GenericRepository<Person, int>, IPersonRepository
{
    public PersonRepository(CivicHubContext context) : base(context.Persons)
    {
        
    }
}