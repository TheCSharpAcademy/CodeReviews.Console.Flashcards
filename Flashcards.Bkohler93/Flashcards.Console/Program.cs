using System.Configuration;
using Database;
using Flashcards;
using Models;

var dbConnString = ConfigurationManager.AppSettings
    .Get("sqlConnectionString") ?? throw new ArgumentNullException("missing 'sqliteConnString' in App.config");


DatabaseService.MigrateUp(dbConnString);

var db = new DbContext(dbConnString);

var stackDto = new UpdateStackDto
{
    Id = 4,
    Name = "Germannn"
};

await db.UpdateStackAsync(stackDto);

var app = new App(db);

app.Run();