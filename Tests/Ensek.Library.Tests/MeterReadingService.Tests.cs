using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvHelper;

using Ensek.DataAccess.Design;
using Ensek.Database.Contexts;
using Ensek.Database.Tables;
using Ensek.Dto.Common;
using Ensek.Service.Services;

using FluentAssertions;
using FluentAssertions.Common;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

namespace Ensek.Library.Tests;

[TestFixture, ExcludeFromCodeCoverage]
public class MeterReadingServiceTests
{
    private ServiceProvider _serviceProvider;
    private MeterReadingService _meterReadingService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var services = new ServiceCollection();
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", true, true)
            .AddEnvironmentVariables();
        IConfigurationRoot configuration = builder.Build();

        ServiceTestUtils.InitServiceCollection(services, configuration);

        LocalDatabaseModule.ForTestDomain().Load(services);
        Database.Migrations.Modules.LocalDatabaseModule.ForTestDomain().Load(services);

        _serviceProvider = services.BuildServiceProvider();

        SeedDatabase(_serviceProvider);
        //AccountReader(EnsekContext ensekContext)

        var cLogger = TestLogger.Create<MeterReadingService>();
        _meterReadingService = new MeterReadingService(cLogger,
            _serviceProvider.GetService<ICSVService>(), // Not currently used !
            _serviceProvider.GetService<IAccountReader>(),
            _serviceProvider.GetService<IMeterReadingReader>(),
            _serviceProvider.GetService<IMeterReadingWriter>(),
            _serviceProvider.GetService<IFailedMeterReadingWriter>()
        );
    }

    // TODO: Move this into a Util, so that it's the same one as the DatabaseBuilder code
    private void SeedDatabase(ServiceProvider serviceProvider)
    {
        var res = serviceProvider.GetService< EnsekContext>();
        if (res is EnsekContext ensekContext)
        {
            // Load the CSV File
            var dir = Directory.GetCurrentDirectory();
            var file = new StreamReader(File.OpenRead(Path.Combine(dir, "Data/Test_Accounts.csv")));
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

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _serviceProvider?.Dispose();
    }

    [Test]
    public void A010_CreateService()
    {
        _meterReadingService.Should().NotBeNull();
        _meterReadingService.Should().BeAssignableTo<IMeterReadingService>();
    }

    [Test]
    public async Task A020_SendEmptyFile()
    {
        var dir = Directory.GetCurrentDirectory();
        FileStream file = File.OpenRead( Path.Combine( dir, "Data/Meter_Reading_Empty.csv"));
        ImportResultDto result = await _meterReadingService.ImportAsync(0L, file );
        result.SuccessLines.Should().Be(0);
        result.FailedLines.Should().Be(0);
    }

    //[Test]
    //public async Task A021_SendInvalidAccountFile()
    //{
    //}
    //[Test]
    //public async Task A022_SendInvalidDateFile()
    //{
    //}
    //[Test]
    //public async Task A023_SendInvalidReadingFile()
    //{
    //}
    //[Test]
    //public async Task A024_SendInvalidDuplicateReadingsFile()
    //{
    //}

    [Test]
    public async Task A025_SendComplexFile()
    {
        var dir = Directory.GetCurrentDirectory();
        FileStream file = File.OpenRead(Path.Combine(dir, "Data/Meter_Reading.csv"));
        ImportResultDto result = await _meterReadingService.ImportAsync(0L, file);
        result.SuccessLines.Should().Be(24);
        result.FailedLines.Should().Be(11);
    }
}