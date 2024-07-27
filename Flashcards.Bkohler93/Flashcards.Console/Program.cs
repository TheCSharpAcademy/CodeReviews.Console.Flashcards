using System.Configuration;
using Flashcards.Database;
using Flashcards;

var dbConnString = ConfigurationManager.AppSettings
    .Get("sqlConnectionString") ?? throw new ArgumentNullException("missing 'sqliteConnString' in App.config");


DatabaseService.MigrateUp(dbConnString);

var db = new DbContext(dbConnString);

var app = new App(db);

await app.Run();