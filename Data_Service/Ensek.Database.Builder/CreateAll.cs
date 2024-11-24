﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using CsvHelper;

using Ensek.Database.Contexts;
using Ensek.Database.Tables;
using Ensek.Dto.Common;

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
            var res = ServiceProvider.GetService(type);
            if (res is EnsekContext ensekContext)
            {
                // Load the CSV File
                var dir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                var file = new StreamReader(File.OpenRead(Path.Combine(dir,"Data/Test_Accounts.csv")));
                var csv = new CsvReader(file, CultureInfo.InvariantCulture);

                var batchAccounts = new List<Account>();
                foreach (var account in csv.GetRecords<AccountDto>())
                {
                    batchAccounts.Add(new Account
                    {
                        AccountId = account.AccountId,
                        FirstName = account.FirstName,
                        LastName = account.LastName
                    });
                }
                ensekContext.Accounts.AddRange(batchAccounts);
                ensekContext.SaveChanges();
            }
        }
    }
}