﻿namespace Ensek.Database.Tables;

public class FailedMeterReading
{
    public int FailedMeterReadingId { get; set; }

    /// <summary>
    /// Used for tracking back to the Logging
    /// </summary>
    public long TsIdentifier { get; set; }

    public int? AccountId { get; set; }

    /// <summary>
    /// When was it Created, As a string so that the client does not need to convert every time to display
    /// </summary>
    public string? ReadingDateTime { get; set; }

    public int? MeterReadValue { get; set; }
}