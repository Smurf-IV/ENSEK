using Ensek.Infrastructure.Common.Modules;

using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Service.Controllers.Modules;

public class ControllersModule : IModule
{
    /// <inheritdoc />
    public void Load(IServiceCollection services)
    {
        services.AddControllersWithViews();

        //services.AddMvc()
        //    .AddApplicationPart(Assembly.GetAssembly(typeof(Contoso.School.UserService.TeacherController)));
    }
}