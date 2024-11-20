using Ensek.Database.Contexts.Initializers;

using Microsoft.EntityFrameworkCore;

namespace Ensek.Database.Migrations.Initializers;

/// <summary>
/// Relational capable DB's can use migrate.
/// </summary>
public class Migrate : IDatabaseInitializer
{
    public void Initialize(DbContext dBContext)
    {
        dBContext.Database.Migrate();
    }
}