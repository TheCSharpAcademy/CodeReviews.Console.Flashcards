using System.Configuration;

var databaseName = ConfigurationManager.AppSettings.Get("DatabaseName");
var databasePassword = ConfigurationManager.AppSettings.Get("DatabasePassword");
var connectionString = $@"Server=localhost,1433;User Id=sa;Password={databasePassword};;TrustServerCertificate=true;";