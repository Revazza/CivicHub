using CivicHub.Application.Repositories;
using CivicHub.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CivicHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddLazyRepository<IPersonRepository, PersonRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
    }

    private static void AddLazyRepository<TRepository, TImplementation>(this IServiceCollection services)
        where TRepository : class
        where TImplementation : class, TRepository
    {
        services.AddScoped<TRepository, TImplementation>();
        services.AddScoped<Lazy<TRepository>>(sp =>
            new Lazy<TRepository>(sp.GetRequiredService<TRepository>));
    }
}