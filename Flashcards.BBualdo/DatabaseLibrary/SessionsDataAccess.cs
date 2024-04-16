using Dapper;
using DatabaseLibrary.Models;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace DatabaseLibrary;

public class SessionsDataAccess
{
  private string _connectionString { get; set; }

  public SessionsDataAccess(string connectionString)
  {
    _connectionString = connectionString;
  }

  public List<StudySession> GetStudySessionsList()
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    string sql = "SELECT * FROM sessions";

    List<StudySession> sessions = connection.Query<StudySession>(sql).ToList();

    return sessions;
  }

  public bool GetAllStudySessions(List<StudySession> sessions)
  {
    if (sessions.Count == 0)
    {
      AnsiConsole.Markup("[red]Sessions list is empty. [/]");
      return false;
    }

    List<StudySessionDTO> sessionsDTO = new List<StudySessionDTO>();

    foreach (StudySession session in sessions)
    {
      string stackName = GetStackNameForStudySession(session.Stack_Id);

      sessionsDTO.Add(session.ToStudySessionDTO(stackName));
    }

    sessionsDTO = sessionsDTO.OrderBy(session => session.Id).ToList();

    ConsoleEngine.ShowStudySessionsTable(sessionsDTO);
    return true;
  }

  public bool InsertSession(StudySession session)
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    string sql = "INSERT INTO sessions(date, score, stack_id) VALUES(@Date, @Score, @StackId)";

    int rowsAffected = connection.Execute(sql, new { Date = session.Date, Score = session.Score, StackId = session.Stack_Id });

    if (rowsAffected == 0)
    {
      AnsiConsole.Markup("[red]Inserting Failed![/]");
      return false;
    }

    AnsiConsole.Markup("[green]Session saved in sessions list![/]");
    return true;
  }

  private string GetStackNameForStudySession(int? stackId)
  {
    using SqlConnection connection = new SqlConnection(_connectionString);

    string sql = "SELECT name FROM stacks WHERE stack_id=@StackId";

    string stackName = connection.ExecuteScalar<string>(sql, new { StackId = stackId })!;

    return stackName;
  }
}