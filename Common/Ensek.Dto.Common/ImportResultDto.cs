namespace Ensek.Dto.Common;

public record ImportResultDto
{
    public uint SuccessLines { get; set; }
    public uint FailedLines { get; set; }

    public void AddError(MeterReadingDto line, string failedToRetrieveAccount)
    {
        FailedLines++;
    }

    public void AddSuccess() => SuccessLines++;

}