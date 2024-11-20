namespace Ensek.Dto.Common;

public record MeterReadingDto
{
    public int AccountId { get; set; }

    /// <summary>
    /// When was it Created, As a string so that the client does not need to convert every time to display
    /// </summary>
    public string? ReadingDateTime { get; set; }

    public int? MeterReadValue { get; set; }
}