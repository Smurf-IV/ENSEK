using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Ensek.Dto.Common;
using Ensek.Service.Controllers;
using Ensek.Service.Services;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

using Moq;

namespace Ensek.Service.Tests;

[TestFixture, ExcludeFromCodeCoverage]
public class ControllerTests
{
    private MeterReadingController _meterReadingController;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var mockLogger = new Mock<ILogger<MeterReadingController>>();
        var mockMeterReadingService = new Mock<IMeterReadingService>();

        _meterReadingController = new MeterReadingController(mockLogger.Object, mockMeterReadingService.Object);
    }

    [Test]
    public void A010_MockController()
    {
        _meterReadingController.Should().NotBeNull();
        Type type = _meterReadingController.GetType();
        MethodInfo? methodInfo = type.GetMethod("ImportAsync", new Type[] { typeof(IFormFileCollection) });
        methodInfo.Should().NotBeNull("Api's should exist");
        List<RouteAttribute> attrRoutes = type.GetCustomAttributes(typeof(RouteAttribute), true).Cast<RouteAttribute>().ToList();
        attrRoutes[0].Template
            .Should()
            .BeEquivalentTo("meter-reading-uploads", "Expected route should exist");
    }

    [Test]
    public async Task A020_MockSendNoFile()
    {
        IFormFileCollection file = new FormFileCollection();
        ImportResultDto result = await _meterReadingController.ImportAsync(file);
        result.Should().NotBeNull();
        result.ExceptionMessage.Should().NotBeNullOrEmpty("An Exception should have been thrown for the missing file!");
    }

    private static IFormFileCollection GetFormFileCollection(string pathToFile)
    {
        var dir = Directory.GetCurrentDirectory();
        var fileInfo = new FileInfo(Path.Combine(dir, pathToFile));
        List<string> filesPathsListToUpload = [fileInfo.FullName];

        FormFileCollection filesCollection = new FormFileCollection();
        foreach (var filePath in filesPathsListToUpload)
        {
            var stream = File.OpenRead(filePath);
            IFormFile file = new FormFile(stream, 0, stream.Length, "files", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = filePath.Split('.')[1] == "jpg" ? "image/jpeg"
                    : filePath.Split('.')[1] == "png" ? "image/png"
                    : "image/bmp",
            };

            filesCollection.Add(file);
        }

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
        httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), filesCollection);

        return httpContext.Request.Form.Files;
    }

    private static IFormFile GetMockFormFile(string modelName, string filename)
    {
        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.Name).Returns(modelName);
        formFile.Setup(f => f.FileName).Returns(filename);
        formFile.Setup(f => f.OpenReadStream()).Returns(File.OpenRead(filename));

        return formFile.Object;
    }

    [Test]
    public async Task A021_MockSendEmptyFile()
    {
        IFormFileCollection formFileCollection = GetFormFileCollection("Data/Meter_Reading_Empty.csv");
        var dir = Directory.GetCurrentDirectory();
        var fileInfo = new FileInfo(Path.Combine(dir, "Data/Meter_Reading_Empty.csv"));
        var formFiles = new FormFileCollection
        {
            GetMockFormFile("file", fileInfo.FullName)
        };
        ImportResultDto result = await _meterReadingController.ImportAsync(formFiles);
        result.Should().NotBeNull();
        result.ExceptionMessage.Should().NotBeNullOrEmpty("An Exception should have been thrown for the missing file!");
    }

}