using System.Data;
using System.Data.SqlClient;

namespace Flashcards.UgniusFalze;
public class Driver
{
    public SqlConnection SqlConn { get; }

    public Driver(string connectionString)
    {
        SqlConn = new SqlConnection(connectionString);
    }
    
    public Dictionary<int, Models.Flashcards> GetFlashCards(int stackId)
    {
        SqlCommand sqlCommand = SqlConn.CreateCommand();
        sqlCommand.CommandText =
            "SELECT FlashcardId, Front, Back, StackId FROM dbo.Flashcards WHERE StackId = @sid";
        sqlCommand.Parameters.Add("@sid", SqlDbType.Int).Value = stackId;
        Dictionary<int, Models.Flashcards> flashcardsList = new Dictionary<int, Models.Flashcards>();
        SqlConn.Open();
        sqlCommand.Prepare();
        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
        {
            while (dataReader.Read())
            {
                
                flashcardsList.Add(dataReader.GetInt32(0), new Models.Flashcards(stackId, dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2)));
            }
        }
        SqlConn.Close();
        return flashcardsList;
    }
    
    public Dictionary<int, Models.Stacks> GetStacks()
    {
        SqlCommand sqlCommand = SqlConn.CreateCommand();
        sqlCommand.CommandText =
            "SELECT StackId, StackName FROM dbo.Stack ORDER BY StackId";
        Dictionary<int, Models.Stacks> stacksList = new Dictionary<int, Models.Stacks>();
        SqlConn.Open();
        sqlCommand.Prepare();
        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
        {
            while (dataReader.Read()){
                stacksList.Add(dataReader.GetInt32(0), new Models.Stacks(dataReader.GetInt32(0), dataReader.GetString(1)));
            }
        }

        SqlConn.Close();
        return stacksList;
    }

    public List<Models.StudySessionDto> GetStudySessions()
    {
        SqlCommand sqlCommand = SqlConn.CreateCommand();
        sqlCommand.CommandText =
            "SELECT sess.Date, sess.Score, stack.StackName " +
            "FROM dbo.StudySessions AS sess " +
            "JOIN dbo.Stack AS stack ON sess.StackId = Stack.StackId " +
            "ORDER BY StudySessionId";
        List<Models.StudySessionDto> sessionList = new List<Models.StudySessionDto>();
        SqlConn.Open();
        sqlCommand.Prepare();
        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
        {
            while (dataReader.Read()){
                sessionList.Add(new Models.StudySessionDto(dataReader.GetDateTime(0), dataReader.GetInt32(1), dataReader.GetString(2)));
            }
        }

        SqlConn.Close();
        return sessionList;
    }
}