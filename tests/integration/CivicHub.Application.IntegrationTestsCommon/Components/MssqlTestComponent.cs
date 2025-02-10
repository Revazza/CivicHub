using System.Data;
using System.Data.Common;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Testcontainers.MsSql;

namespace CivicHub.Application.IntegrationTestsCommon.Components;

public class MssqlTestComponent
{
    private readonly RespawnerOptions _respawnerOptions;
    private Respawner _respawner = null!;
    private SqlConnection _connection;
    private readonly MsSqlContainer _container;
    private bool _isMigrated;

    public MssqlTestComponent()
    {
        _container = new MsSqlBuilder().WithName($"MssqlTestContainer-{Guid.NewGuid()}").Build();
        _respawnerOptions = new RespawnerOptions
        {
            SchemasToInclude =[CivicHubContext.SchemaName],
            DbAdapter = DbAdapter.SqlServer,
            WithReseed = true
        };
    }

    public async Task OneTimeSetUpAsync()
    {
        await _container.StartAsync();
        _connection = new SqlConnection(GetConnectionString());
    }

    public async Task OneTimeTearDownAsync()
    {
        await _connection.DisposeAsync();
        await _container.DisposeAsync();
    }

    public async Task SetUpAsync(CivicHubContext context)
    {
        await OpenConnectionIfNotOpenedAsync();
        await ApplyMigrationsIfNotAppliedAsync(context);
        _respawner ??= await Respawner.CreateAsync(_connection, _respawnerOptions);
    }

    public string GetConnectionString() => _container.GetConnectionString();

    public async Task ResetDatabaseAsync()
        => await _respawner.ResetAsync(_connection);

    private async Task OpenConnectionIfNotOpenedAsync()
    {
        if (_connection.State == ConnectionState.Open)
        {
            return;
        }

        await _connection.OpenAsync();
    }

    private async Task ApplyMigrationsIfNotAppliedAsync(CivicHubContext context)
    {
        if (_isMigrated)
        {
            return;
        }

        _isMigrated = true;
        await context.Database.MigrateAsync();
    }
}