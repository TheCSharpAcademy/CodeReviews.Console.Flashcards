using System.Configuration;

var dbConnString = ConfigurationManager.AppSettings
    .Get("sqlConnectionString") ?? throw new ArgumentNullException("missing 'sqliteConnString' in App.config");

Database.DatabaseService.MigrateUp(dbConnString);

var app = new App(dbConnString);

app.Run();