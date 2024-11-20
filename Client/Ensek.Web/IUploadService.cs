using System.Net.Http;
using System.Threading.Tasks;

namespace Ensek.Web;

public interface IUploadService
{
#pragma warning disable CA1054
    Task<HttpResponseMessage> PostAsync(string apiUrlRoute, HttpContent content);
#pragma warning restore CA1054
}