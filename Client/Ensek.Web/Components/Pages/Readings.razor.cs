using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

using Ensek.Dto.Common;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Ensek.Web.Components.Pages;

public partial class Readings : ComponentBase
{
    [Inject] private IUploadService UploadService { get; set; }

    private IBrowserFile? _file;
    private ImportResultDto? _progress;
    private bool _hasErrors;
    private string? _error;

    private bool _hasFileName;
    private string _inputFileId = Guid.NewGuid().ToString();

    public Readings(IUploadService uploadService)
    {
        UploadService = uploadService;
    }

    protected void HandleFileNameChange(InputFileChangeEventArgs e)
    {
        CheckFileNameState(true);
        _progress = null;
        (bool success, string result) = IsValidCsvFile(e.File);
        if (success)
        {
            _file = e.File;
        }
        else
        {
            _hasErrors = !success;
            _error = result;
        }
        CheckFileNameState();
    }

    private void CheckFileNameState(bool makeNull = false)
    {
        if (makeNull)
        {
            _file = null;
            _hasErrors = false;
        }

        _hasFileName = !string.IsNullOrWhiteSpace(_file?.Name);
        StateHasChanged();
    }

    private void ResetState()
    {
        _inputFileId = Guid.NewGuid().ToString();
        CheckFileNameState(true);
    }

    protected (bool, string) IsValidCsvFile(IBrowserFile browserFile)
    {
        string[] allowedExtensions = [".csv"];
        string extension = Path.GetExtension(browserFile.Name);
        if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            return (false, "Non Csv Error");
        }

        const long maxSizeInBytes = 2 * 1024 * 1024;
        return browserFile.Size > maxSizeInBytes 
            ? (false, "Max File Size > 2Mb") 
            : (true, string.Empty);
    }

    private async Task<ImportResultDto?> UploadAsync(string apiUrlRoute, IBrowserFile file)
    {
        using var content = new MultipartFormDataContent 
            { 
                { new StreamContent(file.OpenReadStream()), "file", file.Name } 
            };

        HttpResponseMessage response = await UploadService.PostAsync(apiUrlRoute, content);
        switch (response.StatusCode)
        {
            case System.Net.HttpStatusCode.OK:
                {
                    var jsonSerializerOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    };
                    return await response.Content.ReadFromJsonAsync<ImportResultDto>(jsonSerializerOptions);
                }
            // TODO: Add handler for Unauthorised
            case HttpStatusCode.Continue:
            case HttpStatusCode.SwitchingProtocols:
            case HttpStatusCode.Processing:
            case HttpStatusCode.EarlyHints:
            case HttpStatusCode.Created:
            case HttpStatusCode.Accepted:
            case HttpStatusCode.NonAuthoritativeInformation:
            case HttpStatusCode.NoContent:
            case HttpStatusCode.ResetContent:
            case HttpStatusCode.PartialContent:
            case HttpStatusCode.MultiStatus:
            case HttpStatusCode.AlreadyReported:
            case HttpStatusCode.IMUsed:
            case HttpStatusCode.Ambiguous:
            case HttpStatusCode.Moved:
            case HttpStatusCode.Found:
            case HttpStatusCode.RedirectMethod:
            case HttpStatusCode.NotModified:
            case HttpStatusCode.UseProxy:
            case HttpStatusCode.Unused:
            case HttpStatusCode.RedirectKeepVerb:
            case HttpStatusCode.PermanentRedirect:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.PaymentRequired:
            case HttpStatusCode.Forbidden:
            case HttpStatusCode.NotFound:
            case HttpStatusCode.MethodNotAllowed:
            case HttpStatusCode.NotAcceptable:
            case HttpStatusCode.ProxyAuthenticationRequired:
            case HttpStatusCode.RequestTimeout:
            case HttpStatusCode.Conflict:
            case HttpStatusCode.Gone:
            case HttpStatusCode.LengthRequired:
            case HttpStatusCode.PreconditionFailed:
            case HttpStatusCode.RequestEntityTooLarge:
            case HttpStatusCode.RequestUriTooLong:
            case HttpStatusCode.UnsupportedMediaType:
            case HttpStatusCode.RequestedRangeNotSatisfiable:
            case HttpStatusCode.ExpectationFailed:
            case HttpStatusCode.MisdirectedRequest:
            case HttpStatusCode.UnprocessableEntity:
            case HttpStatusCode.Locked:
            case HttpStatusCode.FailedDependency:
            case HttpStatusCode.UpgradeRequired:
            case HttpStatusCode.PreconditionRequired:
            case HttpStatusCode.TooManyRequests:
            case HttpStatusCode.RequestHeaderFieldsTooLarge:
            case HttpStatusCode.UnavailableForLegalReasons:
            case HttpStatusCode.InternalServerError:
            case HttpStatusCode.NotImplemented:
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.ServiceUnavailable:
            case HttpStatusCode.GatewayTimeout:
            case HttpStatusCode.HttpVersionNotSupported:
            case HttpStatusCode.VariantAlsoNegotiates:
            case HttpStatusCode.InsufficientStorage:
            case HttpStatusCode.LoopDetected:
            case HttpStatusCode.NotExtended:
            case HttpStatusCode.NetworkAuthenticationRequired:
            default:
            {
                var readAsStringAsync = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(string.IsNullOrWhiteSpace(readAsStringAsync)?response.ReasonPhrase: readAsStringAsync);
            }
        }
    }

    protected async Task UploadFileAsync()
    {
        if (_file is not null)
        {
            try
            {
                // TODO: Use a BusyDialog 
                ImportResultDto? result = await UploadAsync("api/MeterReading/Import", _file!);

                if (result != null)
                {
                    _inputFileId = Guid.NewGuid().ToString();
                    _progress = result;
                }

                ResetState();
            }
            catch (InvalidOperationException ex)
            {
                _hasErrors = true;
                _error = ex.Message;
            }
        }
        else
        {
            _hasErrors = true;
            _error = "Select File Error";
        }
        StateHasChanged();
    }
}