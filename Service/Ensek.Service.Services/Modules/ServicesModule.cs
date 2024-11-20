using Ensek.Infrastructure.Common.Modules;

using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Service.Services.Modules;

public class ServicesModule : IModule
{
    public void Load(IServiceCollection services)
    {
        // These are just redirects with no state, so make them `Scoped`
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IMeterReadingService, MeterReadingService>();
        services.AddScoped<IFailedMeterReadingService, FailedMeterReadingService>();
        services.AddScoped<ICSVService, CSVService>();
    }
}