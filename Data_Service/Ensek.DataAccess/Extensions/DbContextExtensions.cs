using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedMember.Global

namespace Ensek.DataAccess.Extensions;

/// <summary>
/// Taken from
/// https://github.com/paul-lorica/efcore-truncatestrings-HasMaxLength/blob/master/src/EfCoreTruncateStringsBasedOnMaxLength/DbContextExtensions.cs
/// And tidied up
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Modifies in place the strings that are too long
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="entityObject"></param>
    /// <param name="logger"></param>
    public static void TruncateStringsBasedOnMaxLength<T>([NotNull] this DbContext context, [NotNull] T entityObject, ILogger? logger = null)
    {
        IEnumerable<IEntityType> entityTypes = context.Model.GetEntityTypes();
        Dictionary<string, int?> properties = entityTypes.First(e => e.Name == entityObject.GetType().FullName)!
            .GetProperties()
            .ToDictionary(p => p.Name, p => p.GetMaxLength()!);

        foreach (PropertyInfo propertyInfo in entityObject.GetType()
                     .GetProperties()
                     .Where(p => p.PropertyType == typeof(string))
                )
        {
            string? value = (string?)propertyInfo.GetValue(entityObject);

            if (value == null)
            {
                continue;
            }

            int? maxLength = properties[propertyInfo.Name];

            if (maxLength.HasValue)
            {
                propertyInfo.SetValue(entityObject, value.Truncate(maxLength.Value, logger));
            }
        }
    }

    /// <summary>
    /// Modifies in place the strings that are too long
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="entityObjects"></param>
    /// <param name="logger"></param>
    public static void TruncateStringsBasedOnMaxLength<T>(this DbContext context, List<T> entityObjects, ILogger? logger = null)
    {
        IEnumerable<IEntityType> entityTypes = context.Model.GetEntityTypes();
        T entityObject = entityObjects.First()!;
        Dictionary<string, int?> properties = entityTypes.First(e => e.Name == entityObject.GetType().FullName)
            .GetProperties()
            .ToDictionary(p => p.Name, p => p.GetMaxLength());

        foreach (PropertyInfo propertyInfo in entityObject.GetType()
                     .GetProperties()
                     .Where(p => p.PropertyType == typeof(string))
                )
        {
            entityObjects.AsParallel().ForAll(thisEntityObject =>
            {
                string? value = (string?)propertyInfo.GetValue(thisEntityObject);

                if (value == null)
                {
                    return;
                }

                int? maxLength = properties[propertyInfo.Name];

                if (maxLength.HasValue)
                {
                    propertyInfo.SetValue(thisEntityObject, value.Truncate(maxLength.Value, logger));
                }
            });
        }
    }
}