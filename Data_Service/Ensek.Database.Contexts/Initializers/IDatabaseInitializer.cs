using Microsoft.EntityFrameworkCore;

namespace Ensek.Database.Contexts.Initializers;

public interface IDatabaseInitializer
{
    void Initialize(DbContext dBContext);
}