using Ensek.DataAccess.Design;
using Ensek.Infrastructure.Common.Modules;

using Microsoft.Extensions.DependencyInjection;

namespace Ensek.DataAccess.Modules;

public class DatabaseAccessModule : IModule
{
    /// <inheritdoc />
    public void Load(IServiceCollection services)
    {
        // Make sure module is loaded
        services.AddScoped<IAccountReader, AccountReader>();
        services.AddScoped<IAccountWriter, AccountWriter>();

        services.AddScoped<IMeterReadingReader, MeterReadingReader>();
        services.AddScoped<IMeterReadingWriter, MeterReadingWriter>();

        services.AddScoped<IFailedMeterReadingReader, FailedMeterReadingReader>();
        services.AddScoped<IFailedMeterReadingWriter, FailedMeterReadingWriter>();
    }
}