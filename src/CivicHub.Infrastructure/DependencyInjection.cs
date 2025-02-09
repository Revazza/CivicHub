using CivicHub.Application.Common.Services;
using CivicHub.Application.Repositories;
using CivicHub.Infrastructure.Repositories;
using CivicHub.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CivicHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPersonPictureService, PersonPictureService>();
        services.AddRepositories();
        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddLazyRepository<IPersonRepository, PersonRepository>();
        services.AddLazyRepository<ICityRepository, CityRepository>();
        services.AddLazyRepository<IPersonConnectionRepository, PersonConnectionRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddLazyRepository<TRepository, TImplementation>(this IServiceCollection services)
        where TRepository : class
        where TImplementation : class, TRepository
    {
        services.AddScoped<TRepository, TImplementation>();
        services.AddScoped(sp =>
            new Lazy<TRepository>(sp.GetRequiredService<TRepository>));
    }
}