using FlashCardsLibrary.Models;
using Microsoft.Data.SqlClient;

namespace FlashCardsLibrary;

public class SQLDataAccess
{
    private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FlashCardsDB";

    //congifuration manager not working for some reason return null.
    //private readonly string connectionString = ConfigurationManager.ConnectionStrings["flashcardsDB"].ConnectionString;
    public List<StackModel> LoadStacks()
    {
        string sql = "select * from dbo.Stacks;";
        List<StackModel> output = new List<StackModel>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string stackName = reader.GetString(1);
                        output.Add(new StackModel { Id = id, StackName = stackName });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while searching stacks!");
            }
            return output;
        }
    }

    public bool CheckStackName(string stackName)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "select StackName from dbo.Stacks where StackName = @StackName;";
            bool output = false;

            string retrievedStackName = "";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@StackName", stackName);
            try
            {
                connection.Open();
                retrievedStackName = (string)cmd.ExecuteScalar();
                if (retrievedStackName == stackName)
                {
                    output = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while searching stack name {stackName}");
            }
            return output;
        }

    }

    public List<FlashCardModel> LoadFlashCards(string stackName)
    {
        string sql = @"select f.id, Front, Back, StackId
                        from dbo.FlashCards f
                        inner join dbo.Stacks s on s.id = f.StackId
                        where StackName = @StackName";

        List<FlashCardModel> output = new();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@StackName", stackName);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string front = reader.GetString(1);
                        string back = reader.GetString(2);
                        int stackId = reader.GetInt32(3);
                        output.Add(new FlashCardModel { Id = id, Front = front, Back = back, StackId = stackId });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get FlashCards");
            }
            return output;
        }
    }

    private int GetStackId(string stackName)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = @"select id from Stacks where StackName = @stackname;";
            int stackId = 0;
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@StackName", stackName);
            try
            {
                connection.Open();
                stackId = (int)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"stack id from {stackName} could not be found");
            }
            return stackId;
        }
    }

    public int CheckFlashCardId(int id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = @"select id from dbo.FlashCards where id = @id";
            int stackId = 0;
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                connection.Open();
                stackId = (int)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                Console.WriteLine($" flashcard id was not found");
            }
            return stackId;
        }
    }

    public void InsertFlashCard(string stackName, string front, string back)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            int stackId = GetStackId(stackName);
            if (stackId != 0)
            {
                string sql = @"insert into FlashCards (Front, Back, StackId) values (@Front, @Back, @StackId);";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@Front", front);
                cmd.Parameters.AddWithValue("@Back", back);
                cmd.Parameters.AddWithValue("@StackId", stackId);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Record could not be inserted");
                }
            }
            else
            {
                Console.WriteLine("Record not inserted into stack stack Id not found");
            }
        }
    }
    public void UpdateFlashCardRecord(int id, string front, string back)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            if (CheckFlashCardId(id) > 0)
            {
                string sql = @"update FlashCards set Front = @Front, Back = @Back where id = @id ;";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@Front", front);
                cmd.Parameters.AddWithValue("@Back", back);
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Record could not be inserted");
                }
            }
            else
            {
                Console.WriteLine("Record could not be updated id not found");
            }
        }
    }
    public void DeleteFlashCardRecord(int id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            if (CheckFlashCardId(id) > 0)
            {
                string sql = @"delete from FlashCards where id = @id ;";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Record could not be deleted");
                }
            }
            else
            {
                Console.WriteLine("Record could not be deleted Id not found");
            }
        }
    }

    public void CreateStudySession(string stackName, DateTime date, int score)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            int stackId = GetStackId(stackName);
            if (stackId != 0)
            {
                string sql = @"insert into StudySessions (Date, Score, StackId) values (@Date, @Score, @StackId);";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Score", score);
                cmd.Parameters.AddWithValue("@StackId", stackId);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Record could not be inserted");
                }
            }
            else
            {
                Console.WriteLine("Study session could not be created stack Id not found");
            }
        }
    }

    public List<StudySessionModel> GetAllStudySessions()
    {
        using (SqlConnection connection = new(connectionString))
        {
            List<StudySessionModel> output = new();
            string sql = @"select x.id, Date, Score, StackId, s.StackName
                            from dbo.StudySessions x
                            inner join dbo.Stacks s on s.id = x.StackId";

            SqlCommand cmd = new(sql, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime date = reader.GetDateTime(1);
                        int score = reader.GetInt32(2);
                        int stackId = reader.GetInt32(3);
                        string stackName = reader.GetString(4);
                        output.Add(new StudySessionModel { Id = id, Date = date, Score = score, StackId = stackId, StackName = stackName });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Study sessions could not be retrieved");
            }
            return output;
        }

    }

    public void CreateStack(string stackName)
    {
        using (SqlConnection connection = new(connectionString))
        {
            string sql = @"insert into Stacks (StackName) values (@StackName);";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@StackName", stackName);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Stack could not be crated");
            }
        }

    }

    public void DeleteStack(string stackName)
    {
        using (SqlConnection connection = new(connectionString))
        {
            string sql = @"delete from Stacks where StackName = @StackName;";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@StackName", stackName);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("\nStack has been deleted\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Stack {stackName} could not be deleted");
            }
        }
    }

}



