using System.Diagnostics.CodeAnalysis;

using Ensek.Database.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Database.Migrations.Modules;

[ExcludeFromCodeCoverage]
public class LocalDatabaseModule : BaseDatabaseModule
{
    public LocalDatabaseModule(DatabaseOptions options = null,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        : base(options, serviceLifetime)
    {
    }

    public static LocalDatabaseModule ForTestDomain(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        return new(new DatabaseOptions
        {
            ConnectionString = IConstants.TEST_LOCAL_CONNECTION_STRING,
            StartupAction = DbStartup.Migrate
        }, serviceLifetime);
    }

    public override void Load(IServiceCollection services)
    {
        _ = LoadClass.KeepMigrationsDll;
        services.AddDomainContexts(o => o.UseSqlServer(DatabaseOptions.ConnectionString ?? IConstants.STANDARD_LOCAL_CONNECTION_STRING,
            o => o.MigrationsAssembly(IConstants.MIGRATIONS_ASSEMBLY)), ServiceLifetime);

        base.Load(services);
    }
}