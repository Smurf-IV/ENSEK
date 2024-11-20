using Ensek.Database.Tables;

namespace Ensek.DataAccess.Design;

internal interface IMeterReadingReader
{
    Task<IReadOnlyList<MeterReading>> GetAllMeterReadingsAsync(int accountId);
}