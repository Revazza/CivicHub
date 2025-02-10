using AutoFixture;
using CivicHub.Application.IntegrationTestsCommon.Components;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CivicHub.Application.IntegrationTestsCommon;

public abstract class IntegrationTestBase
{
    private MssqlTestComponent _mssqlTestComponent;
    private TestWebAppFactory _factory;
    private IServiceScope _scope;
    protected ISender Sender { get; set; }
    protected Fixture Fixture { get; set; }

    protected CivicHubContext Context { get; set; }


    [OneTimeSetUp]
    public async Task IntegrationTestOneTimeSetup()
    {
        _mssqlTestComponent = new MssqlTestComponent();
        await _mssqlTestComponent.OneTimeSetUpAsync();
    }

    [OneTimeTearDown]
    public async Task IntegrationTestOneTimeTeardown()
    {
        await _mssqlTestComponent.OneTimeTearDownAsync();
    }

    [SetUp]
    public async Task IntegrationTestSetUp()
    {
        _factory = new TestWebAppFactory(_mssqlTestComponent.GetConnectionString());
        _scope = _factory.Services.CreateScope();
        
        Fixture = new Fixture();
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        Sender = _scope.ServiceProvider.GetService<ISender>();
        Context = _scope.ServiceProvider.GetService<CivicHubContext>();
        await _mssqlTestComponent.SetUpAsync(Context);
    }

    [TearDown]
    public async Task IntegrationTestTearDown()
    {
        await _mssqlTestComponent.ResetDatabaseAsync();
        await Context.DisposeAsync();
        _scope.Dispose();
        await _factory.DisposeAsync();
    }

    protected T GetService<T>() => _scope.ServiceProvider.GetRequiredService<T>();
}