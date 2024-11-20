using Ensek.Database.Tables;

namespace Ensek.DataAccess.Design;

internal interface IFailedMeterReadingReader
{
    Task<IReadOnlyList<FailedMeterReading>> GetAllFailedMeterReadingsAsync(int accountId);
}