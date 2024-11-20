using System;

using Ensek.Database.Migrations;
using Ensek.Database.Migrations.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ensek.Database.Builder;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            //.UseApplicationLogging("Model.Builder")
            .ConfigureAppConfiguration(c => c.AddCommandLine(args))
            .ConfigureServices((hostContext, services) =>
            {
                var dbOptions = new DatabaseOptions();
                hostContext.Configuration.Bind(dbOptions);
                bool shouldRun = (hostContext.Configuration[@"run"] ?? string.Empty).Equals(@"true", StringComparison.InvariantCulture);
                var dbModule = new LocalDatabaseModule(dbOptions);
                dbModule.Load(services);

                // https://www.codeproject.com/Tips/775607/How-to-fix-LocalDB-Requested-Login-failed
                // But use (localdb)\MSSQLLocalDB as the server name
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<CreateAll>(sp, shouldRun));
            });
    }
}