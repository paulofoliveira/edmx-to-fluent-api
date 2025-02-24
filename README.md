# edmx-to-fluent-api
Migrating edmx to Fluent API syntax

The idea here is that user input with:

1. Source path of edmx file
2. Output path to generate fluent mapping files for Entity Framework.

Console Application is based on [articles](https://anton-dambrouski.medium.com/how-to-migrate-from-entity-framework-edmx-files-to-fluent-api-syntax-part-1-1-2-f53e31fd0b57) by Anton Dambrouski. This is part 1, for example. Some modifications from original code was added as:

- Set namespace for configuration classes;
- Examine assembly from classes comparing with edmx to find read only properties and map with `Ignore` method in Entity Framework.
