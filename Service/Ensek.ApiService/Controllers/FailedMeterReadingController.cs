using Ensek.Service.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ensek.Service.Controllers;

[ApiController]
[Route("api/FailedMeterReading")]
public class FailedMeterReadingController : ControllerBase
{
    private readonly ILogger<FailedMeterReadingController> _logger;
    private readonly IFailedMeterReadingService _failedMeterReadingService;

    public FailedMeterReadingController(ILogger<FailedMeterReadingController> logger,
        IFailedMeterReadingService failedMeterReadingService)
    {
        _logger = logger;
        _failedMeterReadingService = failedMeterReadingService;
    }
}