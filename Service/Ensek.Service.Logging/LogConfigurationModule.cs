using Ensek.Infrastructure.Common.Modules;

using Microsoft.ApplicationInsights.NLogTarget;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NLog.Config;
using NLog.Extensions.Logging;

namespace Ensek.Service.Logging;

public class LogConfigurationModule : IModule
{
    private readonly IConfiguration _configuration;

    public LogConfigurationModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Load(IServiceCollection services)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        ConfigurationItemFactory.Default.Targets.RegisterDefinition(
#pragma warning restore CS0618 // Type or member is obsolete
            "ApplicationInsightsTarget",
            typeof(ApplicationInsightsTarget)
        );

        // The following line enables Application Insights telemetry collection.
        // Requires ApplicationInsights.InstrumentationKey to be set in appsettings.json
        // https://gavilan.blog/2020/01/29/asp-net-core-3-1-using-application-insights-to-save-logs/
        services.AddApplicationInsightsTelemetry();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
                
            builder.AddNLog(new NLogLoggingConfiguration(_configuration.GetSection("NLog")));
                
        });
    }
}