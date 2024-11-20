using Ensek.Database.Contexts;
using Ensek.Database.Tables;

using Microsoft.EntityFrameworkCore;

namespace Ensek.DataAccess.Design;

/// <summary>
/// AccountId,MeterReadingDateTime,MeterReadValue
/// </summary>
internal class MeterReadingReader :IMeterReadingReader
{
    private readonly EnsekContext _ensekContext;

    public MeterReadingReader(EnsekContext ensekContext)
    {
        _ensekContext = ensekContext;
    }

    public async Task<IReadOnlyList<MeterReading>> GetAllMeterReadingsAsync(int accountId)
    {
        return (await _ensekContext.MeterReadings
                .Where(a => a.AccountId == accountId)
                .ToListAsync())
            .AsReadOnly();
    }
}