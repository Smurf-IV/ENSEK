using Ensek.Database.Contexts;
using Ensek.Database.Tables;

using Microsoft.EntityFrameworkCore;

namespace Ensek.DataAccess.Design;

/// <summary>
/// AccountId,MeterReadingDateTime,MeterReadValue
/// </summary>
internal class FailedMeterReadingReader : IFailedMeterReadingReader
{
    private readonly EnsekContext _ensekContext;

    public FailedMeterReadingReader(EnsekContext ensekContext)
    {
        _ensekContext = ensekContext;
    }

    public async Task<IReadOnlyList<FailedMeterReading>> GetAllFailedMeterReadingsAsync(int accountId)
    {
        return (await _ensekContext.FailedMeterReadings
                .Where(a => a.AccountId == accountId)
                .ToListAsync())
            .AsReadOnly();
    }
}