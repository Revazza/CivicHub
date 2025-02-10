using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CivicHub.Application.IntegrationTestsCommon;

public class TestWebAppFactory(string connectionString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<CivicHubContext>>();

            services.AddDbContext<CivicHubContext>(options =>
                options.UseSqlServer(connectionString, 
                    sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", CivicHubContext.SchemaName)));
        });
    }
    
}