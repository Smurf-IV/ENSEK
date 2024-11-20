using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Infrastructure.Common.Modules;

public interface IModule
{
    void Load(IServiceCollection services);
}