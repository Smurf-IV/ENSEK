# Create Migration

Create a migration by navigating to the root of the `Ensek\Data_Service\Ensek.Database.Migrations` project and executing the following (for each context you need):

``` dos
dotnet tool update dotnet-ef -g --ignore-failed-sources
```
Latest version is `9.0.0`

## Migrations 
- The `NewMigration` can be replaced with a meaningful name, as it will be appended to the classes that are generated.
- Note: DO NOT put `Context` at the end of the `meaningful Name`!

``` dos

dotnet ef migrations add ProjectModel --verbose --context Ensek.Database.Contexts.EnsekContext --startup-project ..\Ensek.Database.Builder

```
