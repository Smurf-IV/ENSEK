using System.Threading;
using System.Threading.Tasks;

using Ensek.Database.Contexts.Initializers;

using Microsoft.EntityFrameworkCore;

namespace Ensek.Database.Contexts.Context;

public abstract class BaseDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions options, IDatabaseInitializer? initializer = null)
        : base(options)
    {
        // https://www.codeproject.com/Tips/775607/How-to-fix-LocalDB-Requested-Login-failed
        // But use (localdb)\MSSQLLocalDB as the server name
        initializer?.Initialize(this);
    }

    public async Task<int> CommitAsync(bool acceptAllChangesOnSuccess = false, CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    // Use this in UnitTests if you must
    public int Commit()
    {
        return SaveChanges();
    }
}