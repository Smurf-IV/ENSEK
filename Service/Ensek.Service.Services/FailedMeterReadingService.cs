using System.Collections.Generic;
using System.Threading.Tasks;

using Ensek.Dto.Common;

namespace Ensek.Service.Services;

public class FailedMeterReadingService : IFailedMeterReadingService
{
    public Task<IReadOnlyList<FailedMeterReadingDto>> GetFailedMeterReadingsAsync(long tsId, long lookupTsId)
    {
        throw new System.NotImplementedException();
    }
}