namespace Ensek.Database.Contexts;

public interface IConstants
{
    static readonly string MIGRATIONS_ASSEMBLY = @"Ensek.Database.Migrations";
    static readonly string CONTEXTS_ASSEMBLY = @"Ensek.Database.Contexts";
    static readonly string TEST_LOCAL_CONNECTION_STRING = @"Server=(localdb)\MSSQLLocalDB;Database=EnsekReadingsTestDb;Trusted_Connection=True;";
    static readonly string STANDARD_LOCAL_CONNECTION_STRING = @"Server=(localdb)\MSSQLLocalDB;Database=EnsekReadingsDb;Trusted_Connection=True";
    static readonly string DB_CONFIG_SECTION = @"Database";
}