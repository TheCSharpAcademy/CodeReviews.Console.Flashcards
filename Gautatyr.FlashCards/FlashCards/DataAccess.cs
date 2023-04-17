using System.Data;
using System.Data.SqlClient;
using static FlashCards.Helpers;
using FlashCards.Models;


namespace FlashCards;

public static class DataAccess
{
    static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
    static string dbFilePath = System.Configuration.ConfigurationManager.AppSettings.Get("DbFilePath");
    static string dbName = System.Configuration.ConfigurationManager.AppSettings.Get("DbName");

    public static void InitializeDatabase()
    {
        CreateDatabase();
        CreateTables();
    }

    public static void InsertStack(string theme)
    {
        theme = SafeTextSql(theme);

        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"IF NOT EXISTS (SELECT * FROM Stacks
            WHERE theme='{theme}')
            INSERT INTO Stacks(Theme) VALUES('{theme}')";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            sqlCommand.ExecuteNonQuery();
            Console.WriteLine($"\nThe stack {theme} has been created successfully !\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public static void InsertCard(string stackName, string question, string answer)
    {
        question = SafeTextSql(question);
        answer = SafeTextSql(answer);

        SqlConnection connection = new SqlConnection(connectionString);

        int stackID = GetStackId(stackName);

        string sqlString =
            $@"INSERT INTO Cards(Question, Answer, StackID)
            VALUES('{question}', '{answer}', {stackID})";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            sqlCommand.ExecuteNonQuery();
            Console.WriteLine($"\nCard added successfully !\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public static void CreateStudySession(int stackId, string score)
    {
        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"INSERT INTO StudySessions(Date, Score, StackID)
            VALUES('{DateTime.Now}', '{score}', {stackId})";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            sqlCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public static List<Stack> GetStacks()
    {
        List<Stack> stacks = new List<Stack>();

        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"SELECT * FROM Stacks";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                stacks.Add(new Stack
                {
                    Id = reader.GetInt32(0),
                    Theme = reader.GetString(1)
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        return stacks;
    }

    public static StackCardsDto GetStack(int stackId)
    {
        StackCardsDto stackCards = new StackCardsDto();

        List<CardDto> cardsDTO = new List<CardDto>();

        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"SELECT * FROM Cards WHERE StackID={stackId}";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                cardsDTO.Add(new CardDto
                {
                    Id = reader.GetInt32(0),
                    Question = reader.GetString(1),
                    Answer = reader.GetString(2),
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        stackCards.Theme = GetStackTheme(stackId);
        stackCards.CardsDto = cardsDTO;

        return stackCards;
    }

    public static CardNoId GetCard(int stackId, int cardId)
    {
        CardNoId card = new CardNoId();

        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"SELECT * FROM Cards WHERE StackID={stackId} AND CardId={cardId}";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                card.Question = reader.GetString(1);
                card.Answer = reader.GetString(2);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        return card;
    }

    public static List<StudySessionDto> GetStudySessions()
    {
        List<StudySessionDto> studySessionDTOs = new List<StudySessionDto>();

        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"SELECT * FROM StudySessions";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                studySessionDTOs.Add(new StudySessionDto
                {
                    Date = DateTime.Parse(reader.GetString(1)),
                    Score = reader.GetString(2),
                    StackTheme = GetStackTheme(reader.GetInt32(3))
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        return studySessionDTOs;
    }

    public static int GetNumberOfCards(int stackId)
    {
        int numberOfCards = 0;

        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"SELECT COUNT (*) FROM Cards WHERE stackId={stackId}";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            numberOfCards = Convert.ToInt32(sqlCommand.ExecuteScalar());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        return numberOfCards;
    }

    public static void UpdateStackTheme(int stackId, string stackNewTheme)
    {
        SqlConnection connection = new SqlConnection(connectionString);

        string stackNewThemeCleaned = SafeTextSql(stackNewTheme);

        string sqlString =
            $@"UPDATE Stacks SET Theme='{stackNewThemeCleaned}' WHERE StackId={stackId}";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            sqlCommand.ExecuteNonQuery();
            Console.WriteLine($"\nThe stack theme has been updated successfully !\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public static void UpdateCardQuestion(int cardId, int stackId, string newQuestion)
    {
        stackId = StackIdToRealId(stackId);
        cardId = CardIdToRealId(cardId, stackId);

        SqlConnection connection = new SqlConnection(connectionString);

        newQuestion = SafeTextSql(newQuestion);

        string sqlString =
            $@"UPDATE Cards SET Question='{newQuestion}' WHERE CardId={cardId}";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            sqlCommand.ExecuteNonQuery();
            Console.WriteLine($"\nThe card's question has been updated successfully !\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public static void UpdateCardAnswer(int cardId, int stackId, string newAnswer)
    {
        stackId = StackIdToRealId(stackId);
        cardId = CardIdToRealId(cardId, stackId);

        SqlConnection connection = new SqlConnection(connectionString);

        newAnswer = SafeTextSql(newAnswer);

        string sqlString =
            $@"UPDATE Cards SET Answer = '{newAnswer}' WHERE CardId={cardId}";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            sqlCommand.ExecuteNonQuery();
            Console.WriteLine($"\nThe card's answer has been updated successfully !\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public static void DeleteCard(int cardId, int stackId)
    {
        stackId = StackIdToRealId(stackId);
        cardId = CardIdToRealId(cardId, stackId);

        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"DELETE FROM Cards WHERE CardId={cardId}";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            sqlCommand.ExecuteNonQuery();
            Console.WriteLine($"\nThe card has been Deleted !\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public static int GetStackId(string stackTheme)
    {
        stackTheme = SafeTextSql(stackTheme);

        int stackId = 0;

        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"SELECT StackID FROM Stacks
            WHERE theme='{stackTheme}'";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                stackId = reader.GetInt32(0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        return stackId;
    }

    public static string GetStackTheme(int stackId)
    {
        stackId = StackIdToRealId(stackId);

        string stackTheme = "";

        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"SELECT Theme FROM Stacks
            WHERE StackID='{stackId}'";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                stackTheme = reader.GetString(0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        return stackTheme;
    }

    public static void DeleteStack(int stackId)
    {
        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"DELETE FROM Stacks WHERE StackId={stackId}";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            sqlCommand.ExecuteNonQuery();
            Console.WriteLine($"\nThe stack has been Deleted !\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public static bool CardExists(int cardId, int stackId)
    {
        bool cardExists = false;
        stackId = StackIdToRealId(stackId);
        cardId = CardIdToRealId(cardId, stackId);

        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"SELECT COUNT (*) FROM Cards WHERE cardID={cardId} AND stackId={stackId}";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            int cardCount = Convert.ToInt32(sqlCommand.ExecuteScalar());

            if (cardCount > 0) cardExists = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        return cardExists;
    }

    public static bool StackExists(int stackId)
    {
        bool stackExists = false;

        SqlConnection connection = new SqlConnection(connectionString);

        int id = StackIdToRealId(stackId);

        string sqlString =
            $@"SELECT COUNT (*) FROM Stacks WHERE stackId={id}";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            int stackCount = Convert.ToInt32(sqlCommand.ExecuteScalar());

            if (stackCount > 0) stackExists = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        return stackExists;
    }

    private static void CreateDatabase()
    {
        SqlConnection connection = new SqlConnection(connectionString);

        string sqlString =
            $@"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{dbName}')
            BEGIN
                CREATE DATABASE {dbName} ON PRIMARY
                (NAME = {dbName}_Data, FILENAME = '{dbFilePath}{dbName}.mdf',
                SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)
                LOG ON (NAME = {dbName}_log,
                FILENAME = '{dbFilePath}{dbName}.ldf',
                SIZE = 1MB, MAXSIZE = 5MB, FILEGROWTH = 10%)
            END";

        SqlCommand sqlCommand = new SqlCommand(sqlString, connection);

        try
        {
            connection.Open();
            sqlCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    private static void CreateTables()
    {
        SqlConnection connection = new SqlConnection(connectionString);

        string createStacksTable =
            $@"IF NOT EXISTS (SELECT * FROM sysobjects
            WHERE name='Stacks' and xtype='U')
            CREATE TABLE Stacks (
                StackID int NOT NULL IDENTITY PRIMARY KEY,
                Theme nvarchar(50) NOT NULL)";

        SqlCommand sqlCommandStacks = new SqlCommand(createStacksTable, connection);

        string createCardsTable =
            $@"IF NOT EXISTS (SELECT * FROM sysobjects
            WHERE name='Cards' and xtype='U')
            CREATE TABLE Cards (
                CardID int NOT NULL IDENTITY PRIMARY KEY,
                Question Text NOT NULL,
                Answer Text NOT NULL,
                StackID int NOT NULL,
                CONSTRAINT FK_StackCard FOREIGN KEY (StackID)
                REFERENCES Stacks(StackID)
                ON DELETE CASCADE
                ON UPDATE CASCADE)";

        SqlCommand sqlCommandCards = new SqlCommand(createCardsTable, connection);

        string createStudySessionsTable =
            $@"IF NOT EXISTS (SELECT * FROM sysobjects
            WHERE name='StudySessions' and xtype='U')
            CREATE TABLE StudySessions (
                StudySessionsID int NOT NULL IDENTITY PRIMARY KEY,
                Date Text NOT NULL,
                Score Text NOT NULL,
                StackID int NOT NULL,
                CONSTRAINT FK_StackStudySession FOREIGN KEY (StackID)
                REFERENCES Stacks(StackID)
                ON DELETE CASCADE
                ON UPDATE CASCADE)";

        SqlCommand sqlCommandStudySessions = new SqlCommand(createStudySessionsTable, connection);

        try
        {
            connection.Open();

            sqlCommandStacks.ExecuteNonQuery();
            sqlCommandCards.ExecuteNonQuery();
            sqlCommandStudySessions.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}\n\nPress Enter to continue.");
            Console.ReadLine();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public static int StackIdToRealId(int id)
    {
        int realId = 0;
        int sequenceId = 1;

        List<Stack> stacks = GetStacks();

        foreach(var stack in stacks)
        {
            if (sequenceId == id)
            {
                realId = stack.Id;
            }

            sequenceId++;
        }

        return realId;
    }

    public static int CardIdToRealId(int cardId, int stackId)
    {
        int cardRealId = 0;
        int sequenceId = 1;

        List<CardDto> cards = GetStack(stackId).CardsDto;

        foreach (var card in cards)
        {
            if (sequenceId == cardId)
            {
                cardRealId = card.Id;
            }

            sequenceId++;
        }

        return cardRealId;
    }
}
