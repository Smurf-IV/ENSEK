using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using AngleSharp.Dom;
using AngleSharp.Html.Dom;

using Aspire.Hosting;

using Ensek.Database.Contexts;
using Ensek.Dto.Common;

using FluentAssertions;


// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
#pragma warning disable CA8618
#pragma warning disable CA2234


namespace Ensek.Tests;

[TestFixture, ExcludeFromCodeCoverage]
public class WebTests
{
    private DistributedApplication _app;
    private HttpClient _httpClient;
    private HttpClient _httpClient1;

    private static void CreateCleanStorage()
    {
        var dir = Directory.GetCurrentDirectory();
        using var p = new Process
        {
            StartInfo =
            {
                FileName = Path.Combine(dir,
                    "../../../../../Data_Service/Ensek.Database.Builder/bin",
#if DEBUG
                    "Debug",
#else
                "Release"
#endif
                    "net9.0/Ensek.Database.Builder.exe"),
                Arguments =
                    $@"--connectionString ""{IConstants.STANDARD_LOCAL_CONNECTION_STRING}"" --StartupAction Create --Run true --Type Domain",
                UseShellExecute = false,    // redirecting
                RedirectStandardError = true
            }
        };
        p.Start();
        var output = p.StandardError.ReadToEnd();
        p.WaitForExit();

        output.Should().BeNullOrWhiteSpace();
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        CreateCleanStorage();

        IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Ensek_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        _app = await appHost.BuildAsync();
        var resourceNotificationService = _app.Services.GetRequiredService<ResourceNotificationService>();
        await _app.StartAsync();

        // Act
        _httpClient = _app.CreateHttpClient(@"webfrontend");
        await resourceNotificationService.WaitForResourceAsync(@"webfrontend", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        _httpClient1 = _app.CreateHttpClient(@"apiservice");
        //await resourceNotificationService.WaitForResourceAsync(@"apiservice", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _httpClient?.Dispose();
        _httpClient1?.Dispose();
        if (_app != null!)
        {
            await _app.StopAsync();
            if (_app is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                await _app.DisposeAsync();
            }
        }
    }

    [Test]
    public async Task A010_GetWebResourceRootReturnsOkStatusCode()
    {
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync("/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task A020_CheckUploadReadings()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/readings");
        IHtmlDocument responseBody = await HtmlHelpers.GetDocumentAsync(response);
        IElement? descriptionElement = responseBody.QuerySelector("div.upload-section");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        descriptionElement.Should().NotBeNull();
        descriptionElement.InnerHtml.Should().ContainEquivalentOf("upload-components");
    }


    [Test]
    public async Task A030_Upload1stAndAgain()
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(File.OpenRead("Data/Meter_Reading.csv")), "file", "Data/Meter_Reading.csv" }
        };

        HttpResponseMessage response = await _httpClient1.PostAsync("/meter-reading-uploads", content);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        ImportResultDto? result = await response.Content.ReadFromJsonAsync<ImportResultDto>(jsonSerializerOptions);
        result.Should().NotBeNull();
        result.SuccessLines.Should().Be(24);
        result.FailedLines.Should().Be(11);

        response = await _httpClient1.PostAsync("/meter-reading-uploads", content);
        result = await response.Content.ReadFromJsonAsync<ImportResultDto>(jsonSerializerOptions);
        result.Should().NotBeNull();
        result.SuccessLines.Should().Be(0, "All previous values should not be replaced");
        result.FailedLines.Should().Be(35, "All previous values should not be replaced");

    }
}
