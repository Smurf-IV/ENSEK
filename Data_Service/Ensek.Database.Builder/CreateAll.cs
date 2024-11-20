using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Ensek.Database.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ensek.Database.Builder;

public class CreateAll : IHostedService
{
    private IServiceProvider ServiceProvider { get; }
    private IHostApplicationLifetime HostApplicationLifetime { get; }
    private ILogger<CreateAll> Logger { get; }
    private bool ShouldRun { get; }

    public CreateAll(IServiceProvider sp, 
        IHostApplicationLifetime appLifetime, 
        ILogger<CreateAll> logger,
        bool shouldRun)
    {
        ShouldRun = shouldRun;
        ServiceProvider = sp;
        HostApplicationLifetime = appLifetime;
        Logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!ShouldRun)
            {
                return Task.CompletedTask;
            }

            ActivateContextsInAssembly(IConstants.CONTEXTS_ASSEMBLY);
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            Logger.LogError(ex, "Domain Builder Failure");
            Environment.ExitCode = 1;
        }
        finally
        {
            HostApplicationLifetime.StopApplication();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private void ActivateContextsInAssembly(string assemblyName)
    {
        Assembly assembly = Assembly.Load(assemblyName);
        var contextTypes = assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(DbContext)));
        foreach (Type type in contextTypes)
        {
            ServiceProvider.GetService(type);
        }
    }
}