namespace CivicHub.Application.Repositories;

public interface IUnitOfWork
{
    public IPersonRepository PersonRepository { get; }
}