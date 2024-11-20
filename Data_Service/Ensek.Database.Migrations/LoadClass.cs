namespace Ensek.Database.Migrations;

/// <summary>
/// Simple class to ensure that the migrations dll goes where migrations are called from
/// </summary>
public static class LoadClass
{
    /// <summary>
    /// The idea is to prevent optimisation from removing the dll reference
    /// </summary>
    public static bool KeepMigrationsDll => true;
}