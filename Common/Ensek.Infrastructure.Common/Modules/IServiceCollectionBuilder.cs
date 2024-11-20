using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Infrastructure.Common.Modules;

public interface IServiceCollectionBuilder
{
    IServiceCollectionBuilder RegisterModule(IModule module);

    IServiceCollection Build();
}