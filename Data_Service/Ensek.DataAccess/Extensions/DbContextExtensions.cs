using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
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
    public static void TruncateStringsBasedOnMaxLength<T>([NotNull] this DbContext context, T entityObject, ILogger? logger = null)
    {
        var entityTypes = context.Model.GetEntityTypes();
        var properties = entityTypes.First(e => e.Name == entityObject.GetType().FullName)
            .GetProperties()
            .ToDictionary(p => p.Name, p => p.GetMaxLength());

        foreach (PropertyInfo propertyInfo in entityObject.GetType()
                     .GetProperties()
                     .Where(p => p.PropertyType == typeof(string))
                )
        {
            var value = (string?)propertyInfo.GetValue(entityObject);

            if (value == null)
            {
                continue;
            }

            var maxLength = properties[propertyInfo.Name];

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
    public static void TruncateStringsBasedOnMaxLength<T>([NotNull] this DbContext context, List<T> entityObjects, ILogger? logger = null)
    {
        var entityTypes = context.Model.GetEntityTypes();
        T entityObject = entityObjects.First();
        var properties = entityTypes.First(e => e.Name == entityObject.GetType().FullName)
            .GetProperties()
            .ToDictionary(p => p.Name, p => p.GetMaxLength());

        foreach (var propertyInfo in entityObject.GetType()
                     .GetProperties()
                     .Where(p => p.PropertyType == typeof(string))
                )
        {
            entityObjects.AsParallel().ForAll(thisEntityObject =>
            {
                var value = (string?)propertyInfo.GetValue(thisEntityObject);

                if (value == null)
                {
                    return;
                }

                var maxLength = properties[propertyInfo.Name];

                if (maxLength.HasValue)
                {
                    propertyInfo.SetValue(thisEntityObject, value.Truncate(maxLength.Value, logger));
                }
            });
        }
    }
}