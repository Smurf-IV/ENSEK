using Ensek.DataAccess.Extensions;
using Ensek.Database.Contexts.Context;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Ensek.DataAccess;

public abstract class BaseWriter<TContext, TTable> where TContext : BaseDbContext
{
    protected readonly TContext context;
    protected readonly ILogger<BaseWriter<TContext, TTable>> logger;

    protected BaseWriter(TContext context, ILogger<BaseWriter<TContext, TTable>> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="toCreate"></param>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <param name="token"></param>
    /// <remarks>
    /// Do not make this Async:
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbset-1.addrangeasync?view=efcore-2.1#Microsoft_EntityFrameworkCore_DbSet_1_AddRangeAsync__0___
    /// </remarks>
    public virtual async Task<TTable> CreateAsync(TTable toCreate, bool acceptAllChangesOnSuccess = true, CancellationToken token = default)
    {
        try
        {
            context.TruncateStringsBasedOnMaxLength(toCreate);
            // ReSharper disable once MethodHasAsyncOverload
            // ReSharper disable once MethodHasAsyncOverloadWithCancellation
            EntityEntry created = context.Add(toCreate);
            await context.SaveChangesAsync(acceptAllChangesOnSuccess, token);
            return (TTable)created.Entity;
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            logger.LogError(e, "Error creating entry {0}", toCreate?.ToString());
            return toCreate;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="toCreate"></param>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// Do not make this Async:
    /// https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbset-1.addrangeasync?view=efcore-2.1#Microsoft_EntityFrameworkCore_DbSet_1_AddRangeAsync__0___
    /// </remarks>
    public virtual async Task<bool> CreateBatchAsync(List<TTable> toCreate, bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default)
    {
        try
        {
            context.TruncateStringsBasedOnMaxLength(toCreate);
            // ReSharper disable once MethodHasAsyncOverload
            // ReSharper disable once MethodHasAsyncOverloadWithCancellation

            //As the type is generic calling 'AddRange' tries to add a single List<TTable> rather than a group of entries of TTable
            toCreate.ForEach(item =>
            {
                context.Add(item);
            });
            await context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            return true;
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            logger.LogError(e, @"Error creating entry {0}", toCreate?.ToString());
            return false;
        }
    }

    public virtual async Task<TTable> UpdateAsync(TTable toUpdate, bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default)
    {
        try
        {
            context.TruncateStringsBasedOnMaxLength(toUpdate);
            context.Update(toUpdate);
            await context.CommitAsync(acceptAllChangesOnSuccess, cancellationToken);
            return toUpdate;
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            logger.LogError(e, @"Error updating entry {0}", toUpdate?.ToString());
            return toUpdate;
        }
    }
}