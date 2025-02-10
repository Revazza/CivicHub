using CivicHub.Api;
using CivicHub.Api.ActionFilters;
using CivicHub.Api.Middlewares;
using CivicHub.Application;
using CivicHub.Infrastructure;
using CivicHub.Persistance;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers(filters =>
{
    filters.Filters.Add(new FieldValidationFilter());
    filters.Filters.Add(new ValidationResultFilter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApi()
    .AddApplication()
    .AddInfrastructure()
    .AddPersistance(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var supportedCultures = new[] { "en", "ka" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);
app.UseMiddleware<CorrelationMiddleware>();
app.UseMiddleware<LanguageMiddleware>();
app.UseMiddleware<GlobalExceptionLoggingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();