using System.Configuration;
using System.Data.SQLite;
using CodingTracker.Controllers;
using CodingTracker.Helpers;
using Dapper;

namespace CodingTracker.Data;

public class DataBaseService
{
    private readonly string DatabasePath;
    private readonly CrudController crudController;

    public DataBaseService()
    {
        DatabasePath = ConfigurationManager.ConnectionStrings["DatabasePath"].ConnectionString;
        using (var connection = GetConnection())
        {
            CreateCodingTable(connection); // Ensure the table is created on service initialization.
            InsertSeedData(connection);
            CreateGoalTable(connection);
        }
    }

    public SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(DatabasePath);
    }

    private void CreateCodingTable(SQLiteConnection connection)
    {
        var query = @"
                CREATE TABLE IF NOT EXISTS CodingTracker (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL,
                    Duration TEXT NOT NULL
                )";

        connection.Execute(query);
    }

    private void CreateGoalTable(SQLiteConnection connection)
    {
        var query = @"
                    CREATE TABLE IF NOT EXISTS Goals (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartDate TEXT NOT NULL,
                        DateToComplete TEXT NOT NULL,
                        Hours TEXT NOT NULL
                    )";
        connection.Execute(query);
    }

    private void InsertSeedData(SQLiteConnection connection)
    {
        // Check if there are any records in CodingTracker table
        var countQuery = "SELECT COUNT(1) FROM CodingTracker;";
        int recordCount = connection.ExecuteScalar<int>(countQuery);

        if (recordCount == 0)
        {
            for (int i = 0; i < 10; i++)
            {
                DateTime startTime = Utilities.GetRandomStartDateTime();
                DateTime endTime = Utilities.GetRandomEndDateTime(startTime);

                while (!DateValidator.IsValidEndDate(startTime, endTime))
                {
                    endTime = Utilities.GetRandomEndDateTime(startTime);
                }

                string duration = Utilities.CalculateDuration(startTime, endTime);
                var query = @"
                    INSERT INTO CodingTracker (StartTime, EndTime, Duration)
                    VALUES 
                        (@StartTime, @EndTime, @Duration)
                    ";
                var parameters = new { StartTime = startTime, EndTime = endTime, Duration = duration };
                connection.Execute(query, parameters);
            }
        }
    }
}