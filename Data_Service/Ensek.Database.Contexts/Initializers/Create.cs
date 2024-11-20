using Ensek.Database.Contexts.Initializers;

using Microsoft.EntityFrameworkCore;

namespace Ensek.Database.Migrations.Initializers;

/// <summary>
/// An in-memory DB, for example, needs to be created. Migrations won't work for it.
/// </summary>
public class Create : IDatabaseInitializer
{
    public void Initialize(DbContext dBContext)
    {
        dBContext.Database.EnsureCreated();
    }
}