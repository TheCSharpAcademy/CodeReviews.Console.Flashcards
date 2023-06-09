using Microsoft.Extensions.Configuration;

public class DbBase
{
    protected string? ConnectionString { get; }

    protected DbBase()
    {
        ConnectionString = GetConnectionStringFromSettings();
    }
    
    private string? GetConnectionStringFromSettings()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        return configuration.GetConnectionString("MSSql");
    }

    protected readonly Dictionary<string, string> SqlCommands = new()
    {
        ["createStackTable"] = "IF OBJECT_ID('dbo.Stacks', 'U') IS NULL " +
                                "CREATE TABLE Stacks " +
                                "(Id INT PRIMARY KEY, " +
                                "Name NVARCHAR(50) NOT NULL, " +
                                "Description NVARCHAR(200));",
        ["createFlashcardsTable"] = "IF OBJECT_ID('dbo.FlashCards', 'U') IS NULL " +
                                    "CREATE TABLE FlashCards " +
                                    "(" +
                                    "Id INT PRIMARY KEY, " +
                                    "Name NVARCHAR(100) NOT NULL, " +
                                    "Content NVARCHAR(Max)," +
                                    "StackId INT NOT NULL," +
                                    "FOREIGN KEY (StackId) REFERENCES Stacks(Id)" +
                                    ");"
    };
}