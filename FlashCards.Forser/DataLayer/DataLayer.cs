using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace FlashCards.Forser;

public class DataLayer
{
    internal string? DatabaseConnection { get; }
    public DataLayer() 
    { 
        DatabaseConnection = GetConnectionStringFromSettings();
    }
    private string? GetConnectionStringFromSettings()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        return config.GetConnectionString("MSSQL");
    }
    public int NewStackEntry(Stack stack)
    {
        int rows = 0;
        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"INSERT Stacks (Name) VALUES ('{stack.Name}')";

                connection.Open();
                rows = cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message.ToString());
        }

        return rows;
    }
    public int ReturnNumberOfStacks()
    {
        int stackCount = 0;
        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT Count('Name') FROM Stacks";

                connection.Open();
                stackCount = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

        return stackCount;
    }
    public int ReturnNumberOfFlashCards()
    {
        int flashCardCount = 0;
        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT Count('Front') From FlashCards";

                connection.Open();
                flashCardCount = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message.ToString());
        }

        return flashCardCount;
    }
    public List<FlashCard> ReturnFlashCardsFromStackId(int stackId)
    {
        List<FlashCard> flashCards = new List<FlashCard>();
        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT Id, Front, Back From FlashCards WHERE StackId = {stackId}";

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    flashCards.Add(new FlashCard
                    {
                        CardId = Convert.ToInt32(reader[0]),
                        Front = reader[1].ToString(),
                        Back = reader[2].ToString()
                    });
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message.ToString());
        }
        return flashCards;
    }
    public List<Stack> FetchAllStacks()
    {
        List<Stack> listOfStacks = new List<Stack>();

        using (SqlConnection connection = new SqlConnection(DatabaseConnection))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT * FROM Stacks";

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                listOfStacks.Add(new Stack
                {
                    StackId = Convert.ToInt32(reader[0]),
                    Name = reader[1].ToString()
                });
            }

            connection.Close();

            return listOfStacks;
        }
    }
    internal bool CheckStackId(int stackId)
    {
        bool stackValid = false;
        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT Count('Name') FROM Stacks WHERE Id = {stackId}";

                connection.Open();
                stackValid = Convert.ToBoolean(cmd.ExecuteScalar());
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

        return stackValid;
    }
    internal bool DeleteStackById(int stackId)
    {
        bool stackDeleted = false;
        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"DELETE FROM Stacks WHERE Id = {stackId}";

                connection.Open();
                stackDeleted = Convert.ToBoolean(cmd.ExecuteNonQuery());
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

        return stackDeleted;
    }
    internal bool NewFlashCard(FlashCard flashCard)
    {
        bool isSaved = false;

        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection)) 
            { 
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"INSERT FlashCards (Front, Back, StackId) VALUES " +
                    $"('{flashCard.Front}', '{flashCard.Back}', '{flashCard.StackId}')";

                connection.Open();
                if (cmd.ExecuteNonQuery() > 0) 
                {
                    isSaved = true;
                }
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message.ToString());
        }

        return isSaved;
    }
    internal bool CheckCardId(int cardId)
    {
        bool cardValid = false;

        try
        {
            using(SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT Count('Front') FROM FlashCards WHERE Id = {cardId}";

                connection.Open();
                cardValid = Convert.ToBoolean(cmd.ExecuteScalar());
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

        return cardValid;
    }
    internal bool DeleteCardById(int cardId)
    {
        bool cardDeleted = false;

        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"DELETE FROM FlashCards WHERE Id = {cardId}";

                connection.Open();
                cardDeleted = Convert.ToBoolean(cmd.ExecuteNonQuery());
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message.ToString());
        }

        return cardDeleted;
    }
    internal FlashCard GetFlashCardById(int cardId)
    {
        FlashCard flashCard = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT * FROM FlashCards WHERE Id = {cardId}";

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    flashCard = new()
                    {
                        CardId = Convert.ToInt32(reader[0]),
                        Front = reader[1].ToString(),
                        Back = reader[2].ToString()
                    };
                }
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(ex.Message.ToString());
        }

        return flashCard;
    }
    internal bool UpdateFlashCardById(FlashCard updatedCard)
    {
        bool cardUpdated = false;

        try
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"UPDATE FlashCards SET Front = '{updatedCard.Front}', Back = '{updatedCard.Back}'" +
                    $"WHERE Id = {updatedCard.CardId}";

                connection.Open();
                cardUpdated = Convert.ToBoolean(cmd.ExecuteNonQuery());
                connection.Close();
            }
        }
        catch(Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message.ToString());
        }

        return cardUpdated;
    }
}