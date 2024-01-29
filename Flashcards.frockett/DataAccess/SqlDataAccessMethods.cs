using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Library.Models;

namespace DataAccess;

public class SqlDataAccessMethods : IDataAccess
{
    private string? initConnectionString;
    private string? dbConnString;
    private string? dbCreationScript;
    private string? tableCreationScript;
    private string? sampleData;

    public SqlDataAccessMethods()
    {
        initConnectionString = GetConnectionStringFromSettings("MasterConnString");
        dbConnString = GetConnectionStringFromSettings("FlashcardsConnString");
        dbCreationScript = File.ReadAllText(@"..\DataAccess\Databaseinit.txt");
        tableCreationScript = File.ReadAllText(@"..\DataAccess\CreateTables.txt");
        sampleData = File.ReadAllText(@"..\DataAccess\SampleData.txt");
    }

    public void InitDatabase()
    {
        using (SqlConnection connection = new SqlConnection(initConnectionString))
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = dbCreationScript;
            command.ExecuteNonQuery();
            connection.Close();
        }

        using(SqlConnection connection1 = new SqlConnection(dbConnString))
        {
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandText = tableCreationScript;
            command1.ExecuteNonQuery();
            
            command1 = connection1.CreateCommand();
            command1.CommandText = sampleData;
            command1.ExecuteNonQuery();
            connection1.Close();
        }
    }
    private string? GetConnectionStringFromSettings(string desiredString)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
        IConfiguration configuration = builder.Build();
        return configuration.GetConnectionString(desiredString);
    }
    public void InsertCard(CardModel flashcard)
    {
        using (SqlConnection connection = new SqlConnection(dbConnString))
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO cards(StackId, Question, Answer)
                                    VALUES ({flashcard.StackId}, '{flashcard.Question}', '{flashcard.Answer}')";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void DeleteCardById(int cardId)
    {
        using (SqlConnection connection = new SqlConnection(dbConnString))
        {
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"DELETE FROM cards WHERE id = {cardId}";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void InsertStack(StackModel stack)
    {
        using (SqlConnection connection = new SqlConnection(dbConnString))
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO stacks(name)
                                    VALUES ('{stack.Name}')";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void DeleteStackById(int stackId)
    {
        DeleteCardsBeforeStack(stackId);
        DeleteRelatedStudySessions(stackId);

        using (SqlConnection connection = new SqlConnection(dbConnString))
        {
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"DELETE FROM stacks WHERE id = {stackId}";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public int CheckForStackContents(int stackId)
    {
        using (SqlConnection connection = new SqlConnection(dbConnString))
        {
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"SELECT COUNT(*) FROM cards WHERE StackId = {stackId}";
            int rows = (int)command.ExecuteScalar();
            connection.Close();
            return rows;
        }
    }

    private void DeleteCardsBeforeStack(int stackId)
    {
        using (SqlConnection connection = new SqlConnection(dbConnString))
        {
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"DELETE FROM cards WHERE StackId = {stackId}";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    private void DeleteRelatedStudySessions(int stackId)
    {
        Console.WriteLine("deleted related study sessions");
        using (SqlConnection connection = new SqlConnection(dbConnString))
        {
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"DELETE FROM StudySessions WHERE StackId = {stackId}";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<StackModel> GetListOfStacks()
    {
        List<StackModel> listOfStacks = new List<StackModel>();

        using (SqlConnection connetion = new SqlConnection(dbConnString))
        {
            connetion.Open();
            SqlCommand command = connetion.CreateCommand();
            command.CommandText = "SELECT * FROM Stacks";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                listOfStacks.Add(new StackModel()
                {
                    Id = Convert.ToInt32(reader[0]),
                    Name = reader[1].ToString()
                });
            }

            connetion.Close();
            return listOfStacks;
        }
    }

    public StackModel GetStackById(int stackId)
    {
        StackModel stackToReturn = new StackModel();

        using (SqlConnection connetion = new SqlConnection(dbConnString))
        {
            connetion.Open();
            using SqlCommand command = connetion.CreateCommand();
            command.CommandText = $@"SELECT * FROM Stacks WHERE Id = {stackId}";
            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    stackToReturn.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    stackToReturn.Name = reader.GetString(reader.GetOrdinal("Name"));
                }
            }
        }
        return stackToReturn;
    }

    public List<CardModel> GetCardsByStackId(int stackId)
    {
        List<CardModel> flashcards = new List<CardModel>();
        using (SqlConnection connection = new SqlConnection(dbConnString))
        {
            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM cards WHERE StackId = {stackId}";

                using SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        flashcards.Add(new CardModel()
                        {
                            Id= reader.GetInt32(reader.GetOrdinal("Id")),
                            StackId = reader.GetInt32(reader.GetOrdinal("StackId")),
                            Question = reader.GetString(reader.GetOrdinal("Question")),
                            Answer = reader.GetString(reader.GetOrdinal("Answer"))
                        });
                    }
                }
            }
            connection.Close();
            return flashcards;
        }

    }

    public int InsertStudySession(StudySessionModel studySession)
    {
        using (SqlConnection connection = new SqlConnection(dbConnString))
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO StudySessions(StackId, Stack_name, SessionDateTime, Score)
                                    VALUES ({studySession.StackId}, '{studySession.StackName}', '{studySession.SessionDateTime.ToString("yyyy-MM-dd HH:mm")}', {studySession.Score})";
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            return rowsAffected;
        }
    }

    public List<StudySessionModel> GetStudySessions()
    {
        List<StudySessionModel> studySessions = new List<StudySessionModel>();

        using (SqlConnection connection = new SqlConnection(dbConnString))
        {
            connection.Open();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"Select * FROM StudySessions";

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    studySessions.Add(new
                        StudySessionModel()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        StackId = reader.GetInt32(reader.GetOrdinal("StackId")),
                        StackName = reader.GetString(reader.GetOrdinal("Stack_name")),
                        SessionDateTime = reader.GetDateTime(reader.GetOrdinal("SessionDateTime")),
                        Score = reader.GetInt32(reader.GetOrdinal("Score"))
                    });
                }
            }
            connection.Close();
            return studySessions;
        }
    }
}
