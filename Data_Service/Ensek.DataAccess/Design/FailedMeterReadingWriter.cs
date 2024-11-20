using Ensek.Database.Contexts;
using Ensek.Database.Tables;
using Ensek.Dto.Common;

using Microsoft.Extensions.Logging;

namespace Ensek.DataAccess.Design;

/// <summary>
/// AccountId,MeterReadingDateTime,MeterReadValue
/// </summary>
internal class FailedMeterReadingWriter : BaseWriter<EnsekContext, FailedMeterReading>, IFailedMeterReadingWriter
{
    private readonly ILogger<BaseWriter<EnsekContext, FailedMeterReading>> _logger;

    public FailedMeterReadingWriter(EnsekContext ensekContext,
        ILogger<BaseWriter<EnsekContext, FailedMeterReading>> logger)
        : base(ensekContext, logger)
    {
        _logger = logger;
    }

    public async Task<int> CreateFailedMeterReadingAsync(ulong tsId, MeterReadingDto failedReading)
    {
        _logger.LogTrace("CreateMeterReadingAsync IN {AccountId} {ReadingDateTime} {MeterReadValue}", failedReading.AccountId, failedReading.MeterReadingDateTime, failedReading.MeterReadValue);
        var entity = new FailedMeterReading
        {
            TsIdentifier = tsId,
            AccountId = failedReading.AccountId,
            ReadingDateTime = failedReading.MeterReadingDateTime,
            MeterReadValue = failedReading.MeterReadValue
        };
        FailedMeterReading reading = await CreateAsync(entity);

        _logger.LogTrace("CreateMeterReadingAsync OUT ID: {FailedMeterReadingId}", reading.FailedMeterReadingId);
        return reading.FailedMeterReadingId;
    }
}
