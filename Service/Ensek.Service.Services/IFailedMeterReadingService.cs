using System.Collections.Generic;
using System.Threading.Tasks;

using Ensek.Dto.Common;

namespace Ensek.Service.Services;

public interface IFailedMeterReadingService
{
    Task<IReadOnlyList<FailedMeterReadingDto>> GetFailedMeterReadingsAsync(long tsId, long lookupTsId);

}