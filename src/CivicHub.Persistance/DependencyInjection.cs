using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CivicHub.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInMemoryDatabase();
        return services;
    }

    private static IServiceCollection AddInMemoryDatabase(
        this IServiceCollection services)
    {
        return services.AddDbContext<CivicHubContext>(options => { options.UseInMemoryDatabase("CivicHub"); });
    }

    //Todo:
    //Replace in memory database with real database
    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddDbContext<CivicHubContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(CivicHubContext.SectionName));
        });
    }
}