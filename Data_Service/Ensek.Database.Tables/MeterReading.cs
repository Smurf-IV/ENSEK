using System.Diagnostics.CodeAnalysis;

namespace Ensek.Database.Tables;

/// <summary>
/// AccountId,MeterReadingDateTime,MeterReadValue
/// </summary>
public class MeterReading
{
    public int MeterReadingId { get; set; }

    public int AccountId { get; set; }

    /// <summary>
    /// When was it Created, As a string so that the client does not need to convert every time to display
    /// </summary>
    [NotNull, DisallowNull]
    public string ReadingDateTime { get; set; }

    /// <summary>
    /// Secondary index (with AccountId) to ensure values are only added once
    /// </summary>
    [NotNull, DisallowNull]
    public DateTime ReadingDateTimeUTC { get; set; }

    public uint MeterReadValue { get; set; }
}