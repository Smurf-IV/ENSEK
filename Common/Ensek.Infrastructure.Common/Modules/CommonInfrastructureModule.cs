using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Infrastructure.Common.Modules;

public class CommonInfrastructureModule : IModule
{
    private readonly IConfiguration? _configuration;

    public CommonInfrastructureModule(IConfiguration? configuration)
    {
        _configuration = configuration;
    }

    public void Load(IServiceCollection services)
    {
        services.AddSingleton<FileSignatureConfig>();
        // Allow the file sizes to temporarily override via a config
        IConfigurationSection? configurationSection = _configuration?.GetSection(ConfigurationKeys.SIZES_CONFIG_SECTION);
        if (configurationSection != null)
        {
            services.Configure<FileSignatureConfig>(configurationSection);
        }
    }
}