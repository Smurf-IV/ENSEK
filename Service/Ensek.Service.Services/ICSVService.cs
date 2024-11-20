using System.Collections.Generic;
using System.IO;

namespace Ensek.Service.Services;

public interface ICSVService
{
    IAsyncEnumerable<T> ReadCSVAsync<T>(Stream file);
}