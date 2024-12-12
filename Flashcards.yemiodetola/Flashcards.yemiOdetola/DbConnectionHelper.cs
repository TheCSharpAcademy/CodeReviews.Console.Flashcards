using System.Configuration;

public static class DbConnectionHelper
{
  public static string GetConnectionString()
  {
    string? connectionString = ConfigurationManager.AppSettings["ConnectionString"];
    if (string.IsNullOrEmpty(connectionString))
    {
      throw new InvalidOperationException("Database connection string is invalid. Please check and retry again.");
    }
    return connectionString;
  }
}
