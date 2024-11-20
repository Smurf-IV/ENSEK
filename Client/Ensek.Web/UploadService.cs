using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ensek.Web;

public class UploadService(HttpClient httpClient) : IUploadService
{
    public Task<HttpResponseMessage> PostAsync(string apiUrlRoute, HttpContent content)
    {
        var uri = new Uri(apiUrlRoute, UriKind.RelativeOrAbsolute);
        return httpClient.PostAsync(uri, content);
    }
}