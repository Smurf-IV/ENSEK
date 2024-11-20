namespace Ensek.Database.Migrations
{
    public enum DbStartup
    {
        Migrate,
        Create,
        None
    }

    public class DatabaseOptions
    {
        public string ConnectionString { get; set; }
        public DbStartup StartupAction { get; set; } = DbStartup.None;
    }
}