using System.Text.RegularExpressions;

using Microsoft.Extensions.Logging;

// ReSharper disable UnusedMember.Global

namespace Ensek.DataAccess.Extensions;

/// <summary>
/// Taken from https://github.com/paul-lorica/efcore-truncatestrings-HasMaxLength/blob/master/src/EfCoreTruncateStringsBasedOnMaxLength/StringExtensions.cs
/// and then tidied up
/// </summary>
public static class StringExtensions
{
    public static string SanitizeToDigitsOnly(this string? value)
    {
        return (value == null) 
            ? string.Empty 
            : Regex.Replace(value, @"\D", string.Empty);
    }

    public static string Truncate(this string s, int length, ILogger? logger = null)
    {
        string result;

        if (string.IsNullOrEmpty(s)
            || (s.Length <= length)
           )
        {
            result = s;
        }
        else
        {
            if (length <= 4)
            {
                result = string.Empty;
            }
            else
            {
                int usable = length / 3;  // Keep the first 1/3rd
                result = $"{s.Substring(0, usable - 2)}...{s[(s.Length - usable * 2 + 2)..]}";

                logger?.LogWarning("Truncated string:{stringToTruncate}", s);
            }
        }

        return result;
    }
}