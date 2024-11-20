using System.Reflection;
using Ensek.Infrastructure.Common.Modules;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Service.Controllers.Modules;

public class ControllersModule : IModule
{
    /// <inheritdoc />
    public void Load(IServiceCollection services)
    {
        Assembly assembly = typeof(AccountController).Assembly;
        // This creates an AssemblyPart, but does not create any related parts for items such as views.
        var part = new AssemblyPart(assembly);
        services.AddControllersWithViews()
            .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(part));

        //services.AddControllersWithViews();

//        services.AddMvc()
//#pragma warning disable CS8604 // Possible null reference argument.
//            .AddApplicationPart(Assembly.GetAssembly(typeof(Ensek.Service.Controllers.AccountController)));
//#pragma warning restore CS8604 // Possible null reference argument.
    }
}