using System.Collections;
using Dapper;
using Flashcards.RyanW84;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

public class DataAccess
{
    internal string tableNameChosen = "";

    IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    private string ConnectionString;

    public IEnumerable TableChosen { get; private set; }

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
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE question = 'Stacks')
                    CREATE TABLE Stacks (
                        Id int IDENTITY(1,1) NOT NULL,
                        Name NVARCHAR(30) NOT NULL UNIQUE,
                        PRIMARY KEY (Id)
                    );";
            connection.Execute(createStackTableSql);

            string createFlashcardTableSql =
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE question = 'Flashcards')
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

                Console.WriteLine("Enter the question of the Stack you wish to add:");

                string stackName = Console.ReadLine();

                string addStackSql = @"INSERT INTO Stacks (Name) VALUES (@Name);";

                connection.Execute(addStackSql, new { Name = stackName });
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
        tableNameChosen = GetTable();

        if (tableNameChosen == "Stacks")
        {
            var stackRecords = GetRecords().Cast<Stacks>();

            var Id = GetNumber("\nPlease type the id of the stack you want to update: ");
            System.Console.Clear();

            var recordSelected = stackRecords.SingleOrDefault(x => x.Id == Id);
            if (recordSelected == null)
            {
                System.Console.WriteLine("Record not found. Choose a valid record");
                System.Console.ReadKey();
                UpdateStack();
            }
            QueryAndDisplaySingleRecord(Id);

            string name = "";
            bool updateName = AnsiConsole.Confirm("\nUpdate Stack name?");
            if (updateName)
            {
                name = AnsiConsole.Ask<string>("New Name: ");
                while (string.IsNullOrEmpty(name))
                {
                    name = AnsiConsole.Ask<string>(
                        $"{tableNameChosen} field: Name can't be empty. Try again:"
                    );
                }
            }

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery =
                    @$"
                UPDATE {tableNameChosen} SET Name =@question WHERE Id =@Id";

                connection.Execute(updateQuery, new { name, Id });
                Console.WriteLine("Record Updated");
            }
        }
        else if (tableNameChosen == "Flashcards")
        {
            var flashcardRecords = GetRecords().Cast<Flashcards>();

            var Id = GetNumber("\nPlease type the id of the Flashcard you want to update: ");
            System.Console.Clear();

            var recordSelected = flashcardRecords.SingleOrDefault(x => x.Id == Id);
            if (recordSelected == null)
            {
                System.Console.WriteLine("Record not found. Choose a valid record");
                System.Console.ReadKey();
                UpdateStack();
            }
            QueryAndDisplaySingleRecord(Id);

            string question = "";
            bool updateQuestion = AnsiConsole.Confirm("\nUpdate question?");
            if (updateQuestion)
            {
                question = AnsiConsole.Ask<string>("Update question: ");
                while (string.IsNullOrEmpty(question))
                {
                    question = AnsiConsole.Ask<string>(
                        $"{tableNameChosen} field: Question can't be empty. Try again:"
                    );
                }
            }

            string answer = "";
            bool updateAnswer = AnsiConsole.Confirm("\nUpdate Answer?");
            if (updateQuestion)
            {
                question = AnsiConsole.Ask<string>("Update Answer: ");
                while (string.IsNullOrEmpty(question))
                {
                    question = AnsiConsole.Ask<string>(
                        $"{tableNameChosen} field: Answer can't be empty. Try again:"
                    );
                }
            }

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string updateQuery =
                    @$"
                UPDATE {tableNameChosen} SET Question =@question WHERE Id =@Id";

                connection.Execute(updateQuery, new { question, Id });
                Console.WriteLine("Record Updated");
            }
        }
    }

    internal IEnumerable GetRecords()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            tableNameChosen = GetTable();

            string getRecordsSQL = @$"SELECT * FROM {tableNameChosen};";

            var stacks = connection.Query<Stacks>(getRecordsSQL);
            var flashcards = connection.Query<Flashcards>(getRecordsSQL);

            if (tableNameChosen == "Stacks")
            {
                ViewRecords((IEnumerable<IEnumerable>)stacks);
                return stacks;
            }
            else if (tableNameChosen == "Flashcards")
            {
                ViewRecords((IEnumerable<IEnumerable>)flashcards);
                return flashcards;
            }
            return TableChosen;
        }
    }

    internal string GetTable()
    {
        var tableNameChosen = "";
        var tableName = AnsiConsole.Prompt(
            new TextPrompt<bool>("Which table do you wish to choose?")
                .AddChoice(true)
                .AddChoice(false)
                .WithConverter(choice => choice ? "Stacks" : "Flashcards")
        );

        tableNameChosen = tableName == true ? "Stacks" : "Flashcards";

        return tableNameChosen;
    }

    internal void ViewRecords(IEnumerable<IEnumerable> TableChosen)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");

        foreach (var record in TableChosen)
        {
            table.AddRow(record.Id.ToString(), record.Name.ToString());
        }

        AnsiConsole.Write(table);
    }

    internal static void QueryAndDisplaySingleRecord(int id)
    {
        var dataAccess = new DataAccess();
        var stacks = dataAccess.GetRecords().Cast<Stacks>();

        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");

        var stack = stacks.Where(x => x.Id == id).Single();
        {
            table.AddRow(stack.Id.ToString(), stack.Name.ToString());
        }
        AnsiConsole.Write(table);
    }

    internal void DeleteEntry()
    {
        var tablename = AnsiConsole.Prompt(
            new TextPrompt<bool>("Which table do you wish to choose?")
                .AddChoice(true)
                .AddChoice(false)
                .WithConverter(choice => choice ? "Stacks" : "Flashcards")
        );
        Console.WriteLine();
        var stacks = GetRecords();

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
        using (var connection = new SqlConnection(ConnectionString))
        {
            Console.Clear();
            Flashcards flashcard = new();
            connection.Open();

            GetRecords();

            int StackChoice = GetNumber(
                "Enter the ID of the Stack you wish to add this Flashcard to"
            );

            var question = AnsiConsole.Ask<string>(
                "Write the question to be asked: (char limit 30)"
            );
            while (string.IsNullOrEmpty(question))
            {
                question = AnsiConsole.Ask<string>("Question can't be empty, try again!");
            }

            var answer = AnsiConsole.Ask<string>("Write the answer to the Question");
            while (string.IsNullOrEmpty(question))
            {
                question = AnsiConsole.Ask<string>("Answer can't be empty, try again!");
            }

            string addFlashcardSql = @"INSERT INTO Flashcards (Question) VALUES (@question);";

            connection.Execute(addFlashcardSql, new { Question = question });
        }
    }

    internal void DeleteFlashcard()
    {
        var flashcards = GetRecords();

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
