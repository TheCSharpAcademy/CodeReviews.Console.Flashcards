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

    private string ConnectionString;

    public DataAccess()
    {
        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    public bool ConfirmConnection()
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

                string stackName = Console.ReadLine();

                string addStackSql = @"INSERT INTO Stacks (Name) VALUES (@Name);";

                connection.Execute(addStackSql, new { Name = stackName });

                //var dataAccess = new DataAccess();
                //dataAccess.InsertStack(stacks);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem in the adding section {ex.Message}");
        }
        UserInterface.StackMenu();
    }

    internal void UpdateStack()
    {
        //var dataAccess = new DataAccess();

        var stackRecords = GetStacks();

        var Id = GetNumber("\nPlease type the id of the habit you want to update: ");
        System.Console.Clear();

        var stackSelected = stackRecords.SingleOrDefault(x => x.Id == Id);
        if (stackSelected == null)
        {
            System.Console.WriteLine("Record not found. Choose a valid record");
            System.Console.ReadKey();
            UpdateStack();
        }
        QueryAndDisplaySingleRecord(Id);

        var name = AnsiConsole.Ask<string>("What do you want to update the Stack Name to? ");
        while (string.IsNullOrEmpty(name))
        {
            name = AnsiConsole.Ask<string>("Name can't be empty. Try again:");
        }

        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string updateQuery =
                @"
                UPDATE stacks SET Name =@name WHERE Id =@Id";

            connection.Execute(updateQuery, new { name, Id });
            Console.WriteLine("Record Updated");

            string name = ""; // adding more spectre integration
            bool updateName = AnsiConsole.Confirm("\nUpdate name?");
            if (updateName)
            {
                name = AnsiConsole.Ask<string>("Habit's new name:");
                while (string.IsNullOrEmpty(name))
                {
                    name = AnsiConsole.Ask<string>("Habit's name can't be empty. Try again:");
                }
            }
        }
    }

    internal IEnumerable<Stacks> GetStacks()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string getStacksSQL = @"SELECT * FROM stacks;";

            var stacks = connection.Query<Stacks>(getStacksSQL);

            ViewStacks(stacks);

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
        var stacks = GetStacks();

        int id = GetNumber("Please type the ID of the Stack you want to delete: ");
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

            var tableName = "stacks";

            string deleteQuery = $@"DELETE FROM {tableName} WHERE Id = @Id";

            int rowsAffected = connection.Execute(deleteQuery, new { Id = id });

            Console.WriteLine($"Rows affected: {rowsAffected} from {tableName}");

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

    internal void AddFlashcard()
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                Console.Clear();
                Flashcards flashcard = new();
                connection.Open();

                Console.WriteLine("Enter the name of the stack you want to add: ");

                string question = Console.ReadLine();

                string addFlashcardSql = @"INSERT INTO Flashcards (Question) VALUES (@question);";

                connection.Execute(addFlashcardSql, new { Question = question });

                var dataAccess = new DataAccess();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem in the adding section {ex.Message}");
        }
        UserInterface.StackMenu();
    }

    internal void DeleteFlashcard()
    {
        var stacks = GetStacks();

        int id = GetNumber("Please type the id of the Flashcard you want to delete: ");
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

    internal void UpdateFlashcard() { }

    internal void GetFlashcards() { }

    internal void ViewFlashcard() { }

    internal class Stacks
    {
        internal int Id { get; set; }
        internal string Name { get; set; }
    }

    internal class Flashcards
    {
        internal int Id { get; set; }
        internal string Question { get; set; }
        internal string Answer { get; set; }
        internal int StackID { get; set; }
    }
}
