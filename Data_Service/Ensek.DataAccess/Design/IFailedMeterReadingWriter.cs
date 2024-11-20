using Ensek.Dto.Common;

namespace Ensek.DataAccess.Design;

public interface IFailedMeterReadingWriter
{
    /// <summary>
    /// Returns the MeterReadingId
    /// </summary>
    Task<int> CreateFailedMeterReadingAsync(long tsId, MeterReadingDto failedReading);
}