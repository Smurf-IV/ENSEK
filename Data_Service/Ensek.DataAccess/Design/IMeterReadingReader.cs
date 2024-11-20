using Ensek.Database.Tables;

namespace Ensek.DataAccess.Design;

public interface IMeterReadingReader
{
    Task<IReadOnlyList<MeterReading>> GetAllMeterReadingsAsync(int accountId);
    Task<MeterReading?> GetLastReadAsync(int accountId);
}