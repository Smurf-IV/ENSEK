using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Database.Migrations.Modules;

public class SqlServerDatabaseModule : BaseDatabaseModule
{
    public SqlServerDatabaseModule(DatabaseOptions options,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) : base(options, serviceLifetime)
    {
    }
        
    public override void Load(IServiceCollection services)
    {
        services.AddDomainContexts(o => o.UseSqlServer(DatabaseOptions.ConnectionString), ServiceLifetime);
        base.Load(services);
    }
}