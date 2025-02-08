using CivicHub.Api;
using CivicHub.Api.ActionFilters;
using CivicHub.Api.Middlewares;
using CivicHub.Application;
using CivicHub.Infrastructure;
using CivicHub.Persistance;

var builder = WebApplication.CreateBuilder(args);

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

app.UseMiddleware<GlobalExceptionLoggingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();