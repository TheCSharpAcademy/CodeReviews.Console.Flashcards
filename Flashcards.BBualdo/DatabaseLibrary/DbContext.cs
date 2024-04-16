using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System.Configuration;
using System.Data;

namespace DatabaseLibrary;

public class DbContext
{
  private string _connectionString { get; set; }
  public StacksDataAccess StacksAccess { get; set; }
  public FlashcardsDataAccess FlashcardsAccess { get; set; }
  public SessionsDataAccess SessionAccess { get; set; }

  public DbContext()
  {
    _connectionString = ConfigurationManager.AppSettings.Get("MasterConnectionString")!;
    AnsiConsole.Markup("[blue]Loading...[/]");

    CreateDatabase();
    CreateTables();
    SeedStacksData();
    SeedFlashcardsData();

    StacksAccess = new(_connectionString);
    FlashcardsAccess = new(_connectionString);
    SessionAccess = new(_connectionString);
  }

  private void CreateDatabase()
  {
    using SqlConnection connection = new(_connectionString);

    string sql = "IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'flashcards') CREATE DATABASE flashcards";

    connection.Execute(sql);

    _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
  }

  public void CreateTables()
  {
    using SqlConnection connection = new(_connectionString);

    string sql = @"IF NOT EXISTS(SELECT * FROM sys.tables WHERE schema_id=SCHEMA_ID('dbo') AND name='stacks') CREATE TABLE stacks(
                  stack_id INT IDENTITY(1, 1) PRIMARY KEY,
                  name VARCHAR(40) UNIQUE NOT NULL);

                  IF NOT EXISTS(SELECT * FROM sys.tables WHERE schema_id=SCHEMA_ID('dbo') AND name='flashcards') CREATE TABLE flashcards(
                  flashcard_id INT IDENTITY(1, 1) PRIMARY KEY,
                  display_id INT NOT NULL,
                  question VARCHAR(200) NOT NULL,
                  answer VARCHAR(200) NOT NULL,
                  stack_id INT REFERENCES stacks(stack_id) ON DELETE CASCADE);

                  IF NOT EXISTS(SELECT * FROM sys.tables WHERE schema_id=SCHEMA_ID('dbo') AND name='sessions') CREATE TABLE sessions(
                  session_id INT IDENTITY(1, 1) PRIMARY KEY,
                  date DATE NOT NULL,
                  score INT NOT NULL,
                  stack_id INT REFERENCES stacks(stack_id) ON DELETE CASCADE)";

    connection.Execute(sql);
  }

  private void SeedStacksData()
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    string countSql = "SELECT COUNT(*) FROM stacks";

    int numberOfStacks = connection.ExecuteScalar<int>(countSql);

    if (numberOfStacks > 0) return;

    string[] defaultStacks = ["Polish", "German", "Spanish"];

    foreach (string stack in defaultStacks)
    {
      string sql = "INSERT INTO stacks(name) VALUES(@Name)";
      connection.Execute(sql, new { Name = stack });
    }
  }

  private void SeedFlashcardsData()
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    string countSql = "SELECT COUNT(*) FROM flashcards";

    int numberOfFlashcards = connection.ExecuteScalar<int>(countSql);

    if (numberOfFlashcards != 0) return;

    string selectIdSql = "SELECT stack_id FROM stacks";

    List<int> stackIds = new List<int>();

    using IDataReader reader = connection.ExecuteReader(selectIdSql);

    while (reader.Read())
    {
      int stackId = reader.GetInt32(0);
      stackIds.Add(stackId);
    }

    reader.Close();

    foreach (int stackId in stackIds)
    {
      int displayId = 0;
      Dictionary<string, string> QandA = GetDictionaryByStackId(stackId);

      foreach (KeyValuePair<string, string> keyValuePair in QandA)
      {
        displayId++;

        string sql = "INSERT INTO flashcards(question, answer, stack_id, display_id) VALUES(@Question, @Answer, @StackId, @DisplayId)";

        connection.Execute(sql, new { Question = keyValuePair.Key, Answer = keyValuePair.Value, StackId = stackId, DisplayId = displayId });
      }
    }
  }

  private Dictionary<string, string> GetDictionaryByStackId(int stackId)
  {
    switch (stackId)
    {
      case 1:
        return new Dictionary<string, string>()
          {
            {"kot", "cat"},
            {"drzewo", "tree"},
            {"rzeka", "river"},
            {"książka", "book"},
            {"słońce", "sun"},
            {"samochód", "car"},
            {"dom", "house"},
            {"kwiat", "flower"},
            {"ulica", "street"},
            {"ptak", "bird"},
            {"chmura", "cloud"},
            {"stół", "table"},
            {"krzesło", "chair"},
            {"szkoła", "school"},
            {"miasto", "city"},
            {"butelka", "bottle"},
            {"jabłko", "apple"},
            {"ogień", "fire"},
            {"woda", "water"},
            {"ryba", "fish"},
            {"las", "forest"},
            {"góra", "mountain"},
            {"dziecko", "child"},
            {"księżyc", "moon"},
            {"zegar", "clock"}
          };
      case 2:
        return new Dictionary<string, string>()
          {
            {"Hund", "dog"},
            {"Baum", "tree"},
            {"Fluss", "river"},
            {"Buch", "book"},
            {"Sonne", "sun"},
            {"Auto", "car"},
            {"Haus", "house"},
            {"Blume", "flower"},
            {"Straße", "street"},
            {"Vogel", "bird"},
            {"Wolke", "cloud"},
            {"Tisch", "table"},
            {"Stuhl", "chair"},
            {"Schule", "school"},
            {"Stadt", "city"},
            {"Flasche", "bottle"},
            {"Apfel", "apple"},
            {"Feuer", "fire"},
            {"Wasser", "water"},
            {"Fisch", "fish"},
            {"Wald", "forest"},
            {"Berg", "mountain"},
            {"Kind", "child"},
            {"Mond", "moon"},
            {"Uhr", "clock"}
          };
      case 3:
        return new Dictionary<string, string>()
          {
            {"perro", "dog"},
            {"árbol", "tree"},
            {"río", "river"},
            {"libro", "book"},
            {"sol", "sun"},
            {"coche", "car"},
            {"casa", "house"},
            {"flor", "flower"},
            {"calle", "street"},
            {"pájaro", "bird"},
            {"nube", "cloud"},
            {"mesa", "table"},
            {"silla", "chair"},
            {"escuela", "school"},
            {"ciudad", "city"},
            {"botella", "bottle"},
            {"manzana", "apple"},
            {"fuego", "fire"},
            {"agua", "water"},
            {"pez", "fish"},
            {"bosque", "forest"},
            {"montaña", "mountain"},
            {"niño", "child"},
            {"luna", "moon"},
            {"reloj", "clock"}
          };
      default:
        return new Dictionary<string, string>();
    }
  }
}