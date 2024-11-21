using System;
using System.Threading.Tasks;

using Ensek.Dto.Common;
using Ensek.Service.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TSID.Creator.NET;

#pragma warning disable SCS0016

namespace Ensek.Service.Controllers;

[ApiController]
[Route("meter-reading-uploads")]
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
    //[Route(@"Import")]
    public async Task<ImportResultDto> ImportAsync([FromForm] IFormFileCollection file)
    {
        long tsId = TsidCreator.GetTsid().ToLong();
        _logger.LogTrace("{TsId} ImportAsync IN", tsId);
        ImportResultDto? importResult = null;
        try
        {
            importResult = await _meterReadingService.ImportAsync(tsId, file[0].OpenReadStream());
            if (importResult is null)
            {
                // Handle Mocking hiding the call, Or something has gone wrong with the DI
                throw new FieldAccessException("Unable to access file");
            }
            importResult.FileName = file[0].FileName;
            return importResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ImportAsync");
            importResult ??= new ImportResultDto();
            importResult.AddError(null, ex.Message);
            return importResult;
        }
        finally
        {
            _logger.LogTrace("{TsId} ImportAsync OUT", tsId);
        }
    }

}