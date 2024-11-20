using Ensek.Dto.Common;

namespace Ensek.DataAccess.Design;

public interface IMeterReadingWriter
{
    /// <summary>
    /// Returns the MeterReadingId
    /// </summary>
    Task<int> CreateMeterReadingAsync(MeterReadingDto meterReadingDto, DateTime readingDateTimeUTC);
}