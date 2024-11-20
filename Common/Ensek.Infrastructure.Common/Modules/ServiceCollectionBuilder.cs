using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Infrastructure.Common.Modules;

public class ServiceCollectionBuilder : IServiceCollectionBuilder
{
    protected IServiceCollection serviceCollection;
    protected readonly List<IModule> modules = new();

    public ServiceCollectionBuilder(IServiceCollection serviceCollection)
    {
        this.serviceCollection = serviceCollection;
    }

    public IServiceCollectionBuilder RegisterModule(IModule module)
    {
        if (modules.All(m => m.GetType() != module.GetType()))
        {
            modules.Add(module);
        }
        return this;
    }

    public IServiceCollection Build()
    {
        modules.ForEach(m => m.Load(serviceCollection));
        return serviceCollection;
    }
}