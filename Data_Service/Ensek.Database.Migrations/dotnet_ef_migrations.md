# Create Migration

Create a migration by navigating to the root of the `Ensek\Data_Service\Ensek.Database.Migrations` project and executing the following (for each context you need):

``` dos
dotnet tool update dotnet-ef -g --ignore-failed-sources
```
Latest version is 6.0.2

## Migrations 
- The `NewMigration` can be replaced with a meaningful name, as it will be appended to the classes that are generated.
- Note: DO NOT put `Context` at the end of the `meaningful Name`!

``` dos

dotnet ef migrations add ProjectModel --verbose --context LR.Ocelot.DataService.Database.Contexts.ProjectModelContext --startup-project ..\Database.Builder


dotnet ef migrations add WorkFlow --verbose --context LR.Ocelot.DataService.Database.Contexts.WorkFlowContext --startup-project ..\Database.Builder


dotnet ef migrations add OcxModel --verbose --context LR.Ocelot.DataService.Database.Contexts.OcxModelContext --startup-project ..\Database.Builder


dotnet ef migrations add TransverseSections --verbose --context LR.Ocelot.DataService.Database.Contexts.TransverseSectionsContext --startup-project ..\Database.Builder

```
