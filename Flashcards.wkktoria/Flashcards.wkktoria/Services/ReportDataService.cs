using System.Data;
using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.UserInteractions;
using Microsoft.Data.SqlClient;

namespace Flashcards.wkktoria.Services;

internal class ReportDataService
{
    private readonly SqlConnection _connection;
    private readonly string _databaseName;

    internal ReportDataService(string connectionString, string databaseName)
    {
        _connection = new SqlConnection(connectionString);
        _databaseName = databaseName;
    }

    internal bool Create(List<ReportDataDto> data)
    {
        var created = false;

        try
        {
            _connection.Open();

            foreach (var rd in data)
            {
                var query = $"""
                             USE {_databaseName};

                             INSERT INTO ReportData(StackId, StackName, SessionYear, SessionMonth, Score)
                                VALUES(@stackId, @name, @year, @month, @score)
                             """;
                var command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@stackId", rd.StackId);
                command.Parameters.AddWithValue("@name", rd.StackName);
                command.Parameters.AddWithValue("@year", rd.SessionYear);
                command.Parameters.AddWithValue("@month", rd.SessionMonth);
                command.Parameters.AddWithValue("@score", rd.Score);

                created = command.ExecuteNonQuery() > 0;
            }
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to create report data.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return created;
    }

    internal bool DeleteAll(List<ReportDataDto> data)
    {
        var deleted = false;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         DELETE FROM ReportData
                         """;
            var command = new SqlCommand(query, _connection);

            deleted = command.ExecuteNonQuery() > 0;
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to delete all report data.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return deleted;
    }

    internal int GetSessionsInMonth(int month, int year)
    {
        var sessions = 0;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         SELECT COUNT(Id) FROM ReportData WHERE SessionMonth = @month AND SessionYear = @year
                         """;
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@month", month);
            command.Parameters.AddWithValue("@year", year);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    sessions = reader.GetInt32(0);
        }
        catch (Exception ex)
        {
            UserOutput.ErrorMessage(ex.Message);
            UserOutput.ErrorMessage("Failed to get sessions in month.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return sessions;
    }

    internal int GetAverageScoreInMonth(int month, int year, string name)
    {
        var avg = 0;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         SELECT AVG(Score) FROM ReportData WHERE SessionMonth = @month AND SessionYear = @year AND StackName = @name
                         """;
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@month", month);
            command.Parameters.AddWithValue("@year", year);
            command.Parameters.AddWithValue("@name", name);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    try
                    {
                        avg = reader.GetInt32(0);
                    }
                    catch (Exception)
                    {
                        avg = 0;
                    }
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to get average score in month.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return avg;
    }
}