using System.Configuration;
using Database;
using Flashcards;
using Models;

var dbConnString = ConfigurationManager.AppSettings
    .Get("sqlConnectionString") ?? throw new ArgumentNullException("missing 'sqliteConnString' in App.config");


DatabaseService.MigrateUp(dbConnString);

var db = new DbContext(dbConnString);

var stackDto = new CreateStackDto() {
    Name = "German",
    Flashcards = [
        new () {Front = "Hallo", Back = "Hello"},
        new () {Front = "Tschuss", Back = "Bye"},
        new () {Front = "Toilette", Back = "Bathroom"}
    ],
};

await db.CreateStackAsync(stackDto);

var app = new App(db);

app.Run();