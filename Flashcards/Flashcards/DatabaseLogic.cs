using Flashcards.Data;
using System.Data.SqlClient;

namespace Flashcards;

public class DatabaseLogic
{
    string connectionString = "Data Source=JOAOSIILVA1993;Initial Catalog=FlashcardsDB;Integrated Security=True";

    public void InsertStack(string stackName)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();
            string query = $"INSERT INTO StacksTable (StacksName) VALUES (@StackName);";
            var command = conn.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@StackName", stackName);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }

    public void DeleteStack(string stackName)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            FlashCardCleanup(stackName);
            StudySessionCleanup(stackName);

            var tableCmd = conn.CreateCommand();
            string query = $"DELETE FROM StacksTable WHERE StacksName LIKE @StackName;";
            var command = conn.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@StackName", stackName);
            command.ExecuteNonQuery();

            conn.Close();
        }
    }

    void FlashCardCleanup(string stackName)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();
            string query = $"DELETE FROM FlashcardsTable WHERE StacksName LIKE @StackName;";
            var command = conn.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@StackName", stackName);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }

    void StudySessionCleanup(string stackName)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();
            string query = $"DELETE FROM StudyTable WHERE StacksName LIKE @StackName;";
            var command = conn.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@StackName", stackName);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }

    public List<string> CreateStacksList()
    {
        List<string> stacksList = new List<string>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();
            string query = "SELECT StacksName FROM StacksTable";
            var command = conn.CreateCommand();
            command.CommandText = query;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string StacksName = reader.GetString(0);
                stacksList.Add(StacksName);
            };         
        }
        return stacksList;
    }

    public int GetStackID(string stackName)
    {
        int stackID = 0;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();
            string query = "SELECT dID FROM StacksTable WHERE CAST(StacksName AS NVARCHAR(MAX)) = @StackName";
            var command = conn.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@StackName", stackName);
            var result = command.ExecuteScalar();

            if (result != null)
            {
                if (int.TryParse(result.ToString(), out stackID))
                {
                    // Retrieved the ID.
                }
            }
        }
        return stackID;
    }

    public void InsertFlashcard(int fcID, string front, string back, string stacksName, int sID)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();
            string query = $"INSERT INTO FlashcardsTable (fcID, front, back, stacksName, sID) VALUES (@fcID, @front, @back, @stacksName, @sID);";
            var command = conn.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@fcID", fcID);
            command.Parameters.AddWithValue("@front", front);
            command.Parameters.AddWithValue("@back", back);
            command.Parameters.AddWithValue("@stacksName", stacksName);
            command.Parameters.AddWithValue("@sID", sID);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }

    public void DeleteFlashcard(int fcID)
    {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var tableCmd = conn.CreateCommand();
                string query = $"DELETE FROM FlashcardsTable WHERE fcID = @fcID;";
                var command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@fcID", fcID);
                command.ExecuteNonQuery();

                UpdateFlashcardTableID();

                conn.Close();
            }
    }

    public void UpdateFlashcard(int fcID, string front, string back)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = $@"UPDATE FlashcardsTable
                            SET Front = @front, Back = @back
                            Where FcID = @fcID;";
            var command = conn.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@fcID", fcID);
            command.Parameters.AddWithValue("@front", front);
            command.Parameters.AddWithValue("@back", back); ;
            int countRows = command.ExecuteNonQuery();

            UpdateFlashcardTableID();
            conn.Close();
        }
    }

    public List<FlashcardsModel> CreateFlashCardsList()
    {
        List<FlashcardsModel> flashcardsList = new List<FlashcardsModel>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();
            string query = "SELECT * FROM FlashcardsTable";
            var command = conn.CreateCommand();
            command.CommandText = query;
            SqlDataReader reader = command.ExecuteReader();

            UpdateFlashcardTableID();

            while (reader.Read())
            {
                int FcID = reader.GetInt32(0);
                string Front = reader.GetString(1);
                string Back = reader.GetString(2);
                string StackName = reader.GetString(3);
                flashcardsList.Add(new FlashcardsModel(FcID, Front, Back, StackName));
            };
            conn.Close();
        }
        return flashcardsList;
    }

    public List<StudySession> CreateStudySessionsList()
    {
        List<StudySession>  studySessionsList = new List<StudySession>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();
            string query = "SELECT * FROM StudyTable";
            var command = conn.CreateCommand();
            command.CommandText = query;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string StacksName = reader.GetString(1);
                string Date = reader.GetString(2);
                int Score = reader.GetInt32(3);

                studySessionsList.Add(new StudySession(StacksName, Date, Score));
            };
            conn.Close();
        }
        return studySessionsList;
    }

    public void UpdateFlashcardTableID()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();
            string query = $@"
                    WITH NumberedRows AS 
                    (SELECT *, ROW_NUMBER() OVER (ORDER BY fcID) AS RowNum
                    FROM FlashcardsTable)
                    UPDATE NumberedRows
                    SET fcID = RowNum;";
            var command = conn.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
            conn.Close();
        }
    }

    public void InsertStudySession(string stackName, string date, int score)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var tableCmd = conn.CreateCommand();
            string query = $"INSERT INTO StudyTable (StacksName, Date, Score) VALUES (@stackName, @date, @score);";
            var command = conn.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@stackName", stackName);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@score", score);
            command.ExecuteNonQuery();

            conn.Close();
        }
    }
}