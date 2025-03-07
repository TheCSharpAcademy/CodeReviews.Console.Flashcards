using Dapper;
using Flashcards.RyanW84;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

public class DataAccess
{
    IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    private string? ConnectionString;

    public DataAccess()
    {
        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    public bool ConfirmConnection() //Confirms the connection
    {
        try
        {
            Console.WriteLine("*_*_*_* Flashcards *_*_*_* ");
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                Console.Write(
                    $"\nConnection Status: {System.Data.ConnectionState.Open}\nPress any Key to continue:"
                );
                Console.ReadKey();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    internal void CreateTables()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string createStackTableSql =
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                    CREATE TABLE Stacks (
                        Id int IDENTITY(1,1) NOT NULL,
                        Name NVARCHAR(30) NOT NULL UNIQUE,
                        PRIMARY KEY (Id)
                    );";
            connection.Execute(createStackTableSql);

            string createFlashcardTableSql =
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                    CREATE TABLE Flashcards (
                        Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        Question NVARCHAR(30) NOT NULL,
                        Answer NVARCHAR(30) NOT NULL,
                        StackId int NOT NULL 
                            FOREIGN KEY 
                            REFERENCES Stacks(Id) 
                            ON DELETE CASCADE 
                            ON UPDATE CASCADE
                    );";
            connection.Execute(createFlashcardTableSql);
        }
    }

    internal void StudyArea() { }

    internal void AddStack()
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                Console.Clear();
                Stacks stacks = new();
                connection.Open();

                Console.WriteLine("Enter the name of the stack you want to add: ");

                string? stackName = Console.ReadLine();

                string addStackSql = @"INSERT INTO Stacks (Name) VALUES (@Name);";

                connection.Execute(addStackSql, new { Name = stackName });

                var dataAccess = new DataAccess();
                dataAccess.InsertRecord(stacks);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem in the adding section {ex.Message}");
        }
        UserInterface.StackMenu();
    }

    internal void InsertRecord(Stacks stacks)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string insertQuery =
                @"
INSERT INTO stacks (Name)
VALUES (@Name)";

            connection.Execute(insertQuery, new { stacks.Name });
        }
    }

    //internal void DeleteStack(int id)
    //{
    //    using (var connection = new SqlConnection(ConnectionString))
    //    {
    //        connection.Open();

    //        string deleteStackSql = @"DELETE FROM Stacks WHERE Id = @Id;";
    //        var affectedRows = connection.Execute(deleteStackSql, new { Id = id });
    //    }
    //}

    internal void UpdateStack() { }

    internal IEnumerable<Stacks> GetStacks()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string getStacksSQL = @"SELECT * FROM stacks;";

            var stacks = connection.Query<Stacks>(getStacksSQL);

            return stacks;
        }
    }

    internal void ViewStacks(IEnumerable<Stacks> stacks)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");

        foreach (var stack in stacks)
        {
            table.AddRow(stack.Id.ToString(), stack.Name.ToString());
        }

        AnsiConsole.Write(table);
        Console.Write("Press any key to continue: ");
        Console.ReadKey();
        UserInterface.isMenuRunning = true;
    }

    internal static void QueryAndDisplaySingleRecord(int id)
    {
        var dataAccess = new DataAccess();
        var stacks = dataAccess.GetStacks();

        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");

        var stack = stacks.Where(x => x.Id == id).Single();
        {
            table.AddRow(stack.Id.ToString(), stack.Name.ToString());
        }
        AnsiConsole.Write(table);
    }

    internal void DeleteStack()
    {
        var dataAccess = new DataAccess();

        var stacks = dataAccess.GetStacks();

        dataAccess.ViewStacks(stacks); // Changed to instance method call

        int id = GetNumber("Please type the id of the habit you want to delete: ");
        System.Console.Clear();
        QueryAndDisplaySingleRecord(id);
        System.Console.WriteLine();

        var responseMessage =
            id < 1
                ? $"\nRecord with the id {id} doesn't exist. Press any key to return to Main Menu"
                : "Record deleted successfully. Press any key to return to Main Menu";

        System.Console.Clear();
        System.Console.WriteLine(responseMessage);
        System.Console.ReadKey();

        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string deleteQuery = "DELETE FROM stacks WHERE Id = @Id";

            int rowsAffected = connection.Execute(deleteQuery, new { Id = id });

            Console.WriteLine($"Stack {id} Deleted");
        }
    }

    internal static int GetNumber(string message)
    {
        string numberInput = AnsiConsole.Ask<string>(message);

        if (numberInput == "0")
            UserInterface.MainMenu();

        var output = Validation.ValidateInt(numberInput, message);

        return output;
    }

    internal void AddFlashcard() { }

    internal void DeleteFlashcard() { }

    internal void UpdateFlashcard() { }

    internal void GetFlashcards() { }

    internal void ViewFlashcard() { }

    internal class Stacks
    {
        internal int Id { get; set; }
        internal string? Name { get; set; }
    }

    internal class Flashcards
    {
        internal int Id { get; set; }
        internal string? Question { get; set; }
        internal string? Answer { get; set; }
        internal int StackID { get; set; }
    }
}
