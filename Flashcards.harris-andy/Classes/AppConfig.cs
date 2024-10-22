using System.Configuration;

namespace Flashcards.harris_andy;

public class AppConfig
{
    public static string ConnectionString => ConfigurationManager.ConnectionStrings["connectionString"]?.ConnectionString ?? throw new Exception("Connection string not found.");
    public static string dbPath = ConfigurationManager.AppSettings["DB-Path"] ?? "./";
}