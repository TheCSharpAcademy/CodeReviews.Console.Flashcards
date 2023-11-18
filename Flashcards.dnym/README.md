# Flashcards

~~Note that this solution uses an SQL Server project. The application assumes
that the database and all tables exist, which requires the SQL Server
project to be "published" beforehand. The template in `PublishLocations`
may be adjusted, as should the `App.config` XML file in the main project.~~

Adjust the `App.config` file: enter a `PreparationConnectionString` config
key if the database and tables need to be created, and set `AddSampleData`
to `"true"` to add some stacks and flashcards.
