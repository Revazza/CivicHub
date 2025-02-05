using CivicHub;
using CivicHub.Application;
using CivicHub.Infrastructure;
using CivicHub.Middlewares;
using CivicHub.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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