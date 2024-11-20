using System;
using System.Linq;
using Ensek.DataAccess.Modules;
using Ensek.Database.Contexts;

using Ensek.Database.Migrations;
using Ensek.Database.Migrations.Modules;
using Ensek.Infrastructure.Common.Modules;
using Ensek.Service.Controllers.Modules;
using Ensek.Service.Logging;
using Ensek.Service.Services.Modules;
using Ensek.Web;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
// =================================== Normally place this stuff into a startup.cs!!
var dbOptions = new DatabaseOptions();
ConfigurationManager configuration = builder.Configuration;
configuration.GetSection(IConstants.DB_CONFIG_SECTION).Bind(dbOptions);

// Use DI reflection to auto register known modules
new ServiceCollectionBuilder(builder.Services)
    .RegisterModule(new SqlServerDatabaseModule(dbOptions))
    .RegisterModule(new DatabaseAccessModule())
    .RegisterModule(new CommonInfrastructureModule(configuration))
    .RegisterModule(new LogConfigurationModule(configuration))
    .RegisterModule(new ServicesModule())
    .RegisterModule(new ControllersModule())
    .Build();

// =================================== End
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/weatherforecast", () =>
{
    WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            10,"10"
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapDefaultEndpoints();

app.Run();