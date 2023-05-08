using System.Data.SqlClient;

namespace FlashCardApp;

internal class GetData
{
    internal static List<StudySessionDto> sessionDtos;
    internal static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NeilTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    internal static string stackQuery = "SELECT StackId, StackName FROM Stacks ORDER BY StackId ASC";
    internal static string flashcardQuery = "SELECT * FROM Flashcards WHERE StackId=@StackId";

    internal static void SetStudySessionDto()
    {
        Console.Clear();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM StudySessions ORDER BY SessionId ASC";
            List<StudySessionDto> tableData = new List<StudySessionDto>();
            SqlDataReader reader = tableCmd.ExecuteReader();

            while (reader.Read())
            {
                var session = new StudySessionDto
                {
                    SessionId = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    StackId = reader.GetInt32(2),
                    Date = reader.GetDateTime(3),
                    Score = reader.GetInt32(4)
                };
                tableData.Add(session);
            }

            sessionDtos = tableData;
            connection.Close();
        }
    }
}
