using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Ensek.Dto.Common;

namespace Ensek.Service.Services;

public interface IMeterReadingService
{
    Task<ImportResultDto> ImportAsync(long tsId, Stream file, CancellationToken cancellationToken = default);
}