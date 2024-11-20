namespace Ensek.Dto.Common;

public record ImportResultDto
{
    public uint SuccessLines { get; private set; }
    public uint FailedLines { get; private set; }

    public void AddError(MeterReadingDto line, string failedToRetrieveAccount)
    {
        FailedLines++;
    }

    public void AddSuccess() => SuccessLines++;

}