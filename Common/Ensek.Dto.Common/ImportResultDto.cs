namespace Ensek.Dto.Common;

public record ImportResultDto
{
    public uint SuccessLines { get; private set; }
    public uint FailedLines { get; private set; }
    public string? ExceptionMessage { get; private set; }
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