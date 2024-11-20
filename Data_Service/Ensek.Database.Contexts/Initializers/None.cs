using Ensek.Database.Contexts.Initializers;

using Microsoft.EntityFrameworkCore;

namespace Ensek.Database.Migrations.Initializers;

/// <summary>
/// For non-development DBs we won't want to update automatically.
/// </summary>
public class None : IDatabaseInitializer
{
    public void Initialize(DbContext dBContext)
    {
    }
}