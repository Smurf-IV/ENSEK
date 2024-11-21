using System.Diagnostics.CodeAnalysis;

using Ensek.DataAccess.Modules;
using Ensek.Database.Contexts;
using Ensek.Database.Migrations;
using Ensek.Database.Migrations.Modules;
using Ensek.Infrastructure.Common.Modules;
using Ensek.Service.Logging;
using Ensek.Service.Services.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Library.Tests;

// TODO: Move to Test Util project
[ExcludeFromCodeCoverage]
internal static class ServiceTestUtils
{
    public static IServiceCollection InitServiceCollection(IServiceCollection services, IConfigurationRoot configuration)
    {
        // Taken from the service exe this sets up

        var dbOptions = new DatabaseOptions();
        configuration.GetSection(IConstants.DB_CONFIG_SECTION).Bind(dbOptions);

        // Use DI reflection to auto register known modules
        new ServiceCollectionBuilder(services)
            .RegisterModule(new SqlServerDatabaseModule(dbOptions))
            .RegisterModule(new DatabaseAccessModule())
            .RegisterModule(new CommonInfrastructureModule(configuration))
            .RegisterModule(new LogConfigurationModule(configuration))
            .RegisterModule(new ServicesModule())
            //.RegisterModule(new ControllersModule()) // For some reason was not able to get this to work here !
            .Build();

        return services;
    }

}