using System.Collections;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using Spectre;
using Spectre.Console;
using Spectre.Console.Cli;

class DBController
{
    public static SqlConnection ConnectDB()
    {
        string?connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");
        SqlConnection connection  =  new SqlConnection(connectionString);
        connection.ConnectionString  = connectionString;
        return  connection;
    }
    public static void CreateTables(SqlConnection connection)
    {
        String  createFlashCardsTable = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                        WHERE TABLE_SCHEMA = 'dbo'
                                        AND TABLE_NAME = 'FlashCards')
                                        BEGIN
                                        CREATE TABLE FlashCards (
                                        ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                        Name varchar(255),
                                        Definition varchar(255),
                                        StackId int)
                                        END;
                                        ";
        string createStacksTable = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                    WHERE TABLE_SCHEMA = 'dbo'
                                    AND TABLE_NAME = 'Stacks')
                                    CREATE TABLE Stacks (
                                    ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                    Name varchar(255));";
        string createSessionsTable = @"IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                    WHERE TABLE_SCHEMA = 'dbo'
                                    AND TABLE_NAME = 'Sessions'))
                                    CREATE TABLE Sessions (
                                    ID Int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                    DateTime varchar(255),
                                    StackId int,
                                    Score varchar(255));";
        using (connection)
        {
            connection.Open();
            SqlCommand command = new SqlCommand(createFlashCardsTable,  connection);
            command.ExecuteNonQuery();
            command.CommandText = createStacksTable;
            command.ExecuteNonQuery();
            command.CommandText = createSessionsTable;
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    
    public static void UpdateFlashCard(SqlConnection connection, int Id, string Name, string Definition, int StackID)
    {
        string sqlString = @"UPDATE FlashCards SET Name = @Name, Definition = @Definition, StackID = @StackID
                            WHERE ID = @Id;";
        connection.Open();
        SqlCommand command = new SqlCommand(sqlString, connection);
        using (command)
        {
            command.Parameters.AddWithValue("Id", Id);
            command.Parameters.AddWithValue("Name", Name);
            command.Parameters.AddWithValue("Definition", Definition);
            command.Parameters.AddWithValue("StackID", StackID);
            command.ExecuteNonQuery();
        }
        connection.Close();
    }
    public static void UpdateStack()
    {

    }
    public static void DeleteFlashCard(SqlConnection connection, int ID)
    {
        string sqlString = @"DELETE FROM FlashCards WHERE ID = @ID;";
        connection.Open();
        using (SqlCommand command = new SqlCommand(sqlString,connection) )
        {
            command.Parameters.AddWithValue("ID", ID);
            command.ExecuteNonQuery();
        }
        connection.Close();
    }
    public static void DeleteFlashCardStacks(SqlConnection connection, int id)
    {
        string  sqlString = @"DELETE FROM FlashCards
                            WHERE StackId = @Id;";
        connection.Open();
        SqlCommand command = new SqlCommand(sqlString, connection);
        using (command)
        {
            command.Parameters.AddWithValue("Id", id);
            command.ExecuteNonQuery();
        }
            connection.Close();
            return;
    }
    public static void DeleteStack(SqlConnection connection, string Name)
    {
        //Get stack ID
        int stackId = QueryStackID(connection,Name);
        //Delete all flashcards with the stackID
        DeleteFlashCardStacks(connection,stackId);
        //Delete sessions with this stack
        DeleteSessionStacks(connection,stackId);
        
        //Delete the stack from stack table
        string  sqlString = @"DELETE FROM Stacks
                            WHERE ID = @Id;";
        connection.Open();
        SqlCommand command = new SqlCommand(sqlString, connection);
        using (command)
        {
            command.Parameters.AddWithValue("Id", stackId);
            command.ExecuteNonQuery();
        }
            connection.Close();
            return;
    }    
    public static int InsertFlashCard(SqlConnection connection, string Name, string Definition, int StackID)
    {
        string  sqlString = @"INSERT INTO FlashCards (Name, Definition, StackId) 
                            VALUES (@Name, @Definition, @StackId); SELECT SCOPE_IDENTITY() AS INT";
        connection.Open();
        SqlCommand command = new SqlCommand(sqlString, connection);
        using (command)
        {
            command.Parameters.AddWithValue("Name", Name);
            command.Parameters.AddWithValue("Definition", Definition);
            command.Parameters.AddWithValue("StackId", StackID);
            
            int output = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            return output;
        }
        
    }
    public static int InsertStack(SqlConnection connection, string Name)
    {
        String sqlString = @"INSERT INTO Stacks (Name)
                            VALUES (@Name); SELECT SCOPE_IDENTITY() AS INT";
        connection.Open();
        SqlCommand command  =  new SqlCommand(sqlString,  connection);
        using (command)
        {
            command.Parameters.AddWithValue("Name",Name);
            int output = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            return output;
        }
    }
    public static List<String> QueryStacks(SqlConnection Connection)
    {
        List<String> stacks = [];
        string stackString = @"SELECT Name FROM Stacks";
        Connection.Open();
        SqlCommand command = new SqlCommand(stackString, Connection);
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                stacks.Add(reader[0].ToString().ToUpper().Trim());
            }
        }
        Connection.Close();
        return stacks;
    }
     public static List<FlashCardModel> GetFlashCardsInStack(SqlConnection connection, string stackName)
    {
        List<FlashCardModel> stack = new List<FlashCardModel>();
                string sqlString = @"SELECT FlashCards.ID, FlashCards.Name, FlashCards.Definition, Stacks.ID
                            FROM FlashCards
                             LEFT JOIN Stacks ON FlashCards.StackID = Stacks.ID 
                             WHERE Stacks.Name = @Stack;";
        connection.Open();
        using(SqlCommand command = new SqlCommand(sqlString, connection))
        {
            command.Parameters.AddWithValue("Stack", stackName);
            SqlDataReader reader = command.ExecuteReader();
            int flashCardPosition = 0;
            while(reader.Read())
            {
                flashCardPosition +=1;
                FlashCardModel flashCard = new FlashCardModel();
                flashCard.Id = Convert.ToInt32(reader[0]);
                flashCard.Position = flashCardPosition;
                flashCard.Name = reader[1].ToString();
                flashCard.Definition = reader[2].ToString();
                flashCard.StackId = Convert.ToInt32(reader[3]);
                flashCard.StackName = stackName;
                stack.Add(flashCard);
                
            }
            flashCardPosition = 0;
        }
        connection.Close();
        return stack;
    }

    public static List<StackModel> GetStacks(SqlConnection connection)
    {
    List<StackModel> Stacks = new List<StackModel>();
    List<string> StacksList =  QueryStacks(connection);

        foreach (string st in StacksList)
        {
            StackModel stack = new StackModel();
            //ID, Name, FlashCard
            stack.Name = st;
            stack.Id = QueryStackID(connection, st);
            stack.FlashCards = GetFlashCardsInStack(connection, st);
            Stacks.Add(stack);
        }

    return Stacks;
    }

    public static int QueryStackID(SqlConnection connection, string Name)
    {
        string sqlString = @"SELECT ID FROM Stacks WHERE Name = @Name;";
        SqlCommand command = new SqlCommand(sqlString, connection);
        command.Parameters.AddWithValue("Name", Name);
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader() )
        {
            while (reader.Read())
            {
                int output = (int)reader[0];
                connection.Close();
                return output;
            }
        }
        connection.Close();
        return 0;
    }
    public static void ViewStacks(List<StackModel> stacks)
    {
        Console.Clear();
        Table table = new Table();
        table.AddColumn("Stack");
        foreach (StackModel stack in stacks)
        {
            table.AddRow(stack.Name);
        }
        AnsiConsole.Write(table);
    }
    
    //revise this method
    public static List<FlashCardDto> ViewFlashcardsInStack(SqlConnection Connection, string Stack)
    {

        Table table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Definition");
        table.AddColumn("Stack Name");
        List<FlashCardDto> flashCards = new List<FlashCardDto>();
        List<StackModel> Stacks = GetStacks(Connection);
        foreach(StackModel stack in Stacks)
        {
            if(stack.Name.ToUpper().Trim() == Stack.ToUpper().Trim())
            {
                int position = 0;
                foreach (FlashCardModel flashCard in stack.FlashCards)
                {
                    table.AddRow([flashCard.Position.ToString(), flashCard.Name, flashCard.Definition, stack.Name]);
                    FlashCardDto flashCardDto = new FlashCardDto();
                    position +=1;
                    flashCardDto.Position = position;
                    flashCardDto.Id = flashCard.Id;
                    flashCardDto.Name = flashCard.Name;
                    flashCardDto.Definition = flashCard.Definition;
                    flashCardDto.StackName = flashCard.StackName;
                    flashCards.Add(flashCardDto);
                }
            }
        }
        
        AnsiConsole.Write(table);
        return flashCards;
    }
    public static List<FlashCardDto> ViewAllFlashCards(SqlConnection Connection)
    {
        Table table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Stack Name");
        table.AddColumn("Name");
        table.AddColumn("Definition");

        List<FlashCardDto> flashCards = new List<FlashCardDto>();
        List<StackModel> Stacks = GetStacks(Connection);
        int position = 0;
        foreach(StackModel stack in Stacks)
        {

                foreach (FlashCardModel flashCard in stack.FlashCards)
                {
                    
                    position +=1;
                    table.AddRow([position.ToString(),stack.Name, flashCard.Name, flashCard.Definition]);
                    FlashCardDto flashCardDto = new FlashCardDto();
                    flashCardDto.Position = position;
                    flashCardDto.Id = flashCard.Id;
                    flashCardDto.StackName = flashCard.StackName;
                    flashCardDto.Name = flashCard.Name;
                    flashCardDto.Definition = flashCard.Definition;
                    flashCards.Add(flashCardDto);
                }
            
        }
        //Connection.Close();
        AnsiConsole.Write(table);
        return flashCards;
    }
    public static void XFlashcardsInStack(SqlConnection connection, string Stack, int cardNumber)
    {
        Random rand = new Random();
        int i =0;
        string idList = "";
        int flashcardCount = countFlashCards(connection,Stack);
        for (i = 1; i<=cardNumber;i++)
        {
            int j = rand.Next(1,flashcardCount+1);
            idList = idList +  j.ToString();
            if (i < cardNumber )
            {
                idList +=", ";
            }
        }
        idList = $"({idList})"; 
        string sqlString = @$"SELECT FlashCards.ID, FlashCards.Name, FlashCards.Definition FROM Flashcards LEFT JOIN Stacks ON FlashCards.StackID = Stacks.ID WHERE Stacks.Name = @Stack AND Flashcards.ID IN {idList};";
        SqlCommand command = new SqlCommand(sqlString,connection);
        connection.Open();
        command.Parameters.AddWithValue("Stack", Stack);
        Table table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Definition");
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while(reader.Read())
            {
                table.AddRow([reader[0].ToString(), reader[1].ToString(), reader[2].ToString()]);
            }
        }
        connection.Close();
        AnsiConsole.Write(table);
    }
    public static int countFlashCards(SqlConnection connection, string Stack)
    {
        string sqlString = @"SELECT Count(FlashCards.ID) FROM Flashcards LEFT JOIN STACKS ON FlashCards.StackID = Stacks.ID WHERE Stacks.Name = @Stack;";
        SqlCommand command = new SqlCommand(sqlString, connection);
        connection.Open();
        command.Parameters.AddWithValue("Stack", Stack);
        using(SqlDataReader reader = command.ExecuteReader())
        {
            while(reader.Read())
            {
                try
                {
                    int count = int.Parse(reader[0].ToString());
                    connection.Close();
                    return count;
                }
                catch
                {
                    connection.Close();
                    return 0;
                }
            }
            connection.Close();
            return 0;
        }
        
    }
    public static int InsertSesion(SqlConnection connection, SessionModel session)
    {
        string  sqlString = @"INSERT INTO Sessions (DateTime, StackId, Score, FlashCardIds) 
                            VALUES (@datetime, @stackId, @score, @flashCardsIds); SELECT SCOPE_IDENTITY() AS INT";
        
        string datetime = session.Date;
        int stackId = session.StackId;
        string score = session.Score;
        string flashCardIds="";
        foreach (FlashCardModel card in session.FlashCards)
        {
            flashCardIds += $"{card.Id},";
        }
        flashCardIds = flashCardIds.Remove(flashCardIds.Length-1);
        
        connection.Open();
        SqlCommand command = new SqlCommand(sqlString, connection);
        using (command)
        {
            command.Parameters.AddWithValue("datetime", datetime);
            command.Parameters.AddWithValue("stackId", stackId);
            command.Parameters.AddWithValue("score", score);
            command.Parameters.AddWithValue("flashCardsIds", flashCardIds);
            
            var output = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            return output;
        }
    }
  
    public static List<SessionModel> QuerySession(SqlConnection connection)

    {
        List<SessionModel> sessions = new List<SessionModel>();
        
        List<FlashCardModel> cards = new List<FlashCardModel>();
        FlashCardModel card = new FlashCardModel();
        List<StackModel> stacks = DBController.GetStacks(connection);

        string sqlString = @"SELECT DateTime, StackId, Score, FlashCardIds
                            FROM Sessions;";
        connection.Open();
        
        using(SqlCommand command = new SqlCommand(sqlString, connection))
        {
            SqlDataReader reader = command.ExecuteReader();
            
            while(reader.Read())
            {
                SessionModel session = new SessionModel();
                session.Date = reader[0].ToString();
                session.StackId = Convert.ToInt32(reader[1]);
                session.Score= reader[2].ToString();
                List<FlashCardModel> stack = stacks.Where(x => x.Id == session.StackId).First().FlashCards;
                foreach (string number in reader[3].ToString().Split(','))
                {
                    cards.Add(stack.Where(x => x.Id == Convert.ToInt32(number)).First());
                }
                session.FlashCards = cards;
                sessions.Add(session);
            }
        }
        connection.Close();
        return sessions;
    }
    //view all study sessions
    public static void ViewAllSessions(SqlConnection connection, List<SessionModel> models)
    {
        Table table = new Table();
        table.AddColumn("ID");
        table.AddColumn("DateTime");
        table.AddColumn("Stack");
        table.AddColumn("Score");
        List<StackModel> Stacks = GetStacks(connection);
        foreach(SessionModel model in models)
        {
        string stackName = Stacks.Where(x => x.Id == model.StackId).First().Name;
        table.AddRow([model.Id.ToString(), model.Date, stackName, model.Score.ToString()]);
        }
        AnsiConsole.Write(table);
    }

    public static void DeleteSessionStacks(SqlConnection connection, int id)
    {
        string  sqlString = @"DELETE FROM Sessions
                            WHERE StackId = @Id;";
        connection.Open();
        SqlCommand command = new SqlCommand(sqlString, connection);
        using (command)
        {
            command.Parameters.AddWithValue("Id", id);
            command.ExecuteNonQuery();
        }
            connection.Close();
            return;
    }
}