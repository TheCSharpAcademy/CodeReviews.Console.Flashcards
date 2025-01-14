using Dapper;
using Microsoft.Data.SqlClient;

internal class SessionCreate
{
    internal static void AddNewSession(Stack stack, DateTime date, int score)
    {
        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@StackId", stack.StackId);
            parameters.Add("@StackName", stack.StackName);
            parameters.Add("@SessionDate", date);
            parameters.Add("@SessionScore", score);
            connection.Execute(@"
                INSERT INTO Session (StackId, StackName, SessionDate, SessionScore)
                VALUES (@StackId, @StackName, @SessionDate, @SessionScore)", parameters);
        }
        catch (SqlException ex)
        {
            DisplayErrorHelpers.SqlError(ex);
        }
        catch (Exception ex)
        {
            DisplayErrorHelpers.GeneralError(ex);
        }
    }
}
