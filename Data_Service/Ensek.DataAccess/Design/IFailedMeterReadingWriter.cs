using Ensek.Dto.Common;

namespace Ensek.DataAccess.Design;

public interface IFailedMeterReadingWriter
{
    /// <summary>
    /// Returns the MeterReadingId
    /// </summary>
    Task<int> CreateFailedMeterReadingAsync(ulong tsId, MeterReadingDto failedReading);
}