using System.Configuration;
using Database;
using Flashcards;
using Models;

var dbConnString = ConfigurationManager.AppSettings
    .Get("sqlConnectionString") ?? throw new ArgumentNullException("missing 'sqliteConnString' in App.config");


DatabaseService.MigrateUp(dbConnString);

var db = new DbContext(dbConnString);

await db.DeleteStackAsync(1);

var app = new App(db);

app.Run();