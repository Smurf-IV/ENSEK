using Ensek.Database.Contexts;
using Ensek.Database.Tables;
using Ensek.Dto.Common;

using Microsoft.Extensions.Logging;

namespace Ensek.DataAccess.Design;

/// <summary>
/// AccountId,MeterReadingDateTime,MeterReadValue
/// </summary>
internal class MeterReadingWriter : BaseWriter<EnsekContext, MeterReading>, IMeterReadingWriter
{
    private readonly EnsekContext _ensekContext;
    private readonly ILogger<BaseWriter<EnsekContext, MeterReading>> _logger;

    public MeterReadingWriter(EnsekContext ensekContext,
        ILogger<BaseWriter<EnsekContext, MeterReading>> logger)
        : base(ensekContext, logger)
    {
        _ensekContext = ensekContext;
        _logger = logger;
    }

    public async Task<int> CreateMeterReadingAsync(MeterReadingDto readingDto, DateTime readingDateTimeUTC)
    {
        _logger.LogTrace("CreateMeterReadingAsync IN {AccountId} {ReadingDateTime} {MeterReadValue}", readingDto.AccountId, readingDto.MeterReadingDateTime, readingDto.MeterReadValue);
        var entity = new MeterReading
        {
            AccountId = readingDto.AccountId,
            ReadingDateTime = readingDto.MeterReadingDateTime!,
            ReadingDateTimeUTC = readingDateTimeUTC,
            MeterReadValue = (uint)readingDto.MeterReadValue!
        };
        MeterReading reading = await CreateAsync(entity);

        _logger.LogTrace("CreateMeterReadingAsync OUT ID: {MeterReadingId}", reading.MeterReadingId);
        return reading.MeterReadingId;
    }
}