using System.Reflection;

using Ensek.Database.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Database.Migrations
{
    public static class DomainServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainContexts(this IServiceCollection sc, Action<DbContextOptionsBuilder> optionsBuilder, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        {
            MethodInfo addMethod = typeof(EntityFrameworkServiceCollectionExtensions).GetMethods()
                .Single(m => m.Name == "AddDbContext"
                             && m.GetGenericArguments().Length == 1
                             && m.GetParameters()[1].ParameterType == typeof(Action<DbContextOptionsBuilder>));
            var contextTypes = GetContexts(IConstants.CONTEXTS_ASSEMBLY);
            foreach (Type type in contextTypes)
            {
                MethodInfo genericMethod = addMethod.MakeGenericMethod(type);
                genericMethod.Invoke(sc,
                [
                    sc,
                    optionsBuilder,
                    contextLifetime,
                    addMethod.GetParameters()[3].DefaultValue
                ]);
            }
            return sc;
        }

        private static IEnumerable<Type> GetContexts(string assemblyName)
        {
            Assembly domainAssembly = Assembly.Load(assemblyName);
            return domainAssembly.GetTypes()
                .Where(t => !t.IsAbstract
                            && t.IsSubclassOf(typeof(DbContext)));
        }
    }
}
