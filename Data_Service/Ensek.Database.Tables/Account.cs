using System.Diagnostics.CodeAnalysis;

namespace Ensek.Database.Tables;

/// <summary>
/// AccountId,FirstName,LastName
/// </summary>
/// <remarks>
/// `FirstName` || `LastName` || (`FirstName` && `LastName`) Will NOT BE a unique key !
/// </remarks>
public class Account
{
    /// <summary>
    /// External Foreign Key / Index
    /// </summary>
    public int AccountId { get; set; }

    /// <summary>
    /// HasMaxLength(128)
    /// </summary>
    [NotNull, DisallowNull]
    public string FirstName { get; set; }

    /// <summary>
    /// HasMaxLength(128)
    /// </summary>
    [NotNull, DisallowNull]
    public string LastName { get; set; }

    public ICollection<MeterReading> MeterReadings { get; set; }

}