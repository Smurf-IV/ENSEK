using Ensek.Database.Contexts.Initializers;
using Ensek.Database.Migrations.Initializers;
using Ensek.Infrastructure.Common.Modules;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ensek.Database.Migrations.Modules;

public abstract class BaseDatabaseModule : IModule
{
    protected DatabaseOptions DatabaseOptions { get; }
    protected ServiceLifetime ServiceLifetime { get; }

    public BaseDatabaseModule(DatabaseOptions options, ServiceLifetime serviceLifetime)
    {
        DatabaseOptions = options ?? new DatabaseOptions();
        ServiceLifetime = serviceLifetime;
    }

    public virtual void Load(IServiceCollection services)
    {
        switch (DatabaseOptions.StartupAction)
        {
            case DbStartup.Create:
                services.TryAddTransient<IDatabaseInitializer, Create>();
                break;
            case DbStartup.Migrate:
                services.TryAddTransient<IDatabaseInitializer, Migrate>();
                break;
            default:
                services.TryAddTransient<IDatabaseInitializer, None>();
                break;
        }
    }
}