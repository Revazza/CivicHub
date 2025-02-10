using AutoFixture;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CivicHub.Application.IntegrationTestsCommon;

public abstract class IntegrationTestBase
{
    private TestWebAppFactory _factory;
    private IServiceScope _scope;
    protected ISender Sender { get; set; }
    protected Fixture Fixture { get; set; }
    
    protected CivicHubContext Context { get; set; }

    [SetUp]
    public void IntegrationTestSetUp()
    {
        _factory = new TestWebAppFactory();
        _scope = _factory.Services.CreateScope();
        Fixture = new Fixture();
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        Sender = _scope.ServiceProvider.GetService<ISender>();
        Context = _scope.ServiceProvider.GetService<CivicHubContext>();
    }

    [TearDown]
    public async Task IntegrationTestTearDown()
    {
        await Context.DisposeAsync();
        _scope.Dispose();
        await _factory.DisposeAsync();
    }

    protected T GetService<T>() => _scope.ServiceProvider.GetRequiredService<T>();
}