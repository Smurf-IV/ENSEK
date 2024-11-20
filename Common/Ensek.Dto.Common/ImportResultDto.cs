namespace Ensek.Dto.Common;

public record ImportResultDto
{
    public uint SuccessLines { get; set; }
    public uint FailedLines { get; set; }
    public string? ExceptionMessage { get; set; }
    public long TsId { get; set; }

    public void AddError(MeterReadingDto? line, string failedToRetrieveAccount)
    {
        FailedLines++;
        if (line is null)
        {
            ExceptionMessage = failedToRetrieveAccount;
        }
    }

    public void AddSuccess() => SuccessLines++;

}