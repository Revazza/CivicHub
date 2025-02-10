using System.Reflection;
using CivicHub.Application.Behaviours;
using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Common.Services;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CivicHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));
        
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationPipelineBehaviour<,>));

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        var validationLocalizer = services.BuildServiceProvider().GetService<IValidationLocalizer>();
        
        // Overriding default name display to give priority to .WithName() property
        // And if it doesn't exist then to use property name itself
        ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
        {
            if (memberInfo is null)
            {
                return expression.Parameters.FirstOrDefault()?.Name.Capitalize();
            }
            return validationLocalizer.Translate(memberInfo.Name) ?? memberInfo.Name;
        };
        return services;
    }
}