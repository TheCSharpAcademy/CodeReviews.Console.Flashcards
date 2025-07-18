namespace DotNETConsole.Flashcards.Controllers;

using Database;
using DTO;
using Dapper;
using Views;

public class StudySessionController
{

    private DbContext _dbContext = new DbContext();
    private UserViews _userViews = new UserViews();

    public List<SessionDto> GetAllSession()
    {
        List<SessionDto> sessions = new List<SessionDto>();

        using (var connection = _dbContext.DBConnection())
        {
            string query = @"SELECT sl.Score, sl.LogDate, s.Name AS Stack
                                             FROM studylogs sl
                                             JOIN stacks s ON sl.STACK_ID = s.ID";
            sessions = connection.Query<SessionDto>(query).ToList();
        }
        return sessions;
    }

    public int AddSession(int stackId)
    {
        int res = 0;
        using (var connection = _dbContext.DBConnection())
        {
            string query = @"INSERT INTO studylogs (STACK_ID) VALUES (@StackId);
                            SELECT CAST(SCOPE_IDENTITY() AS int);";
            var queryParams = new { StackId = stackId };
            try
            {
                res = connection.QuerySingle<int>(query, queryParams);
                _userViews.Tost($"Session created successfully.", "success");
            }
            catch (Exception ex)
            {
                _userViews.Tost($"An error occurred: {ex.Message}", "error");
            }
        }
        return res;
    }

    public void UpdateScore(int sessionID, int score)
    {
        using (var connection = _dbContext.DBConnection())
        {
            string query = @"UPDATE studylogs SET Score=@Score WHERE ID=@SessionId";
            var queryAttributes = new { Score = score, SessionId = sessionID };
            try
            {
                connection.Execute(query, queryAttributes);
                _userViews.Tost($"[blue bold]Your Score is {score}.[/]");
            }
            catch (Exception ex)
            {
                _userViews.Tost($"Failed to update Score: {ex.Message}", "error");
            }
        }
    }
}
