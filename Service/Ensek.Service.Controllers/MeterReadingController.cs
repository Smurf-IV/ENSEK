using System.Threading.Tasks;

using Ensek.Dto.Common;
using Ensek.Service.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

#pragma warning disable SCS0016

namespace Ensek.Service.Controllers;

[ApiController]
[Route("api/MeterReading")]
public class MeterReadingController : ControllerBase
{
    private readonly ILogger<MeterReadingController> _logger;
    private readonly IMeterReadingService _meterReadingService;

    public MeterReadingController(ILogger<MeterReadingController> logger,
        IMeterReadingService meterReadingService)
    {
        _logger = logger;
        _meterReadingService = meterReadingService;
    }

    [HttpPost]
    [Route(@"List")]
    public async Task<IActionResult> ImportAsync([FromForm] IFormFileCollection file)
    {
        ulong tsId = 0;

        ImportResultDto importResult = await _meterReadingService.ImportAsync(tsId, file[0].OpenReadStream());
        return Ok(importResult);
    }

}