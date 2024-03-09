using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using flashcards.Fennikko.Models;
using Spectre.Console;

namespace flashcards.Fennikko;

public class DatabaseController
{
    public static readonly string? InitialConnection = ConfigurationManager.AppSettings.Get("initialConnectionString");

    public static string? ConnectionString = ConfigurationManager.AppSettings.Get("connectionString");

    public static void DatabaseCreation()
    {
        using var initialConnection = new SqlConnection(InitialConnection);
        var testQuery = "SELECT database_id FROM sys.databases WHERE name = 'Flashcards'";
        var testDatabaseExists = initialConnection.Query(testQuery);
        if (testDatabaseExists.Any()) return;
        var databaseCreation = """
                               CREATE DATABASE Flashcards ON PRIMARY
                               (NAME = Flashcards,
                               FILENAME = 'C:\temp\Flashcards.mdf',
                               SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)
                               LOG ON (NAME = MyDatabase_Log,
                               FILENAME = 'C:\temp\FlashCardsLog.ldf',
                               SIZE = 1MB,
                               MAXSIZE = 5MB,
                               FILEGROWTH = 10%)
                               """;
        initialConnection.Execute(databaseCreation);
        using var connection = new SqlConnection(ConnectionString);
        connection.Execute(
            """
            IF OBJECT_ID(N'stacks', N'U') IS NULL
            CREATE TABLE stacks (
                StackId int IDENTITY(1,1) PRIMARY KEY,
                StackName VARCHAR(255) NOT NULL,
                UNIQUE (StackName)
                )
            """);
        connection.Execute(
            """
            IF OBJECT_ID(N'flash_cards', N'U') IS NULL
            CREATE TABLE flash_cards (
            FlashcardId int IDENTITY(1,1) PRIMARY KEY,
            FlashcardIndex int NOT NULL,
            CardFront VARCHAR(255) NOT NULL,
            CardBack VARCHAR(255) NOT NULL,
            StackId int NOT NULL,
            UNIQUE (CardFront),
            CONSTRAINT FK_flash_cards_stacks FOREIGN KEY (StackId)
            REFERENCES stacks (StackId)
            ON DELETE CASCADE
            )
            """);
        connection.Execute(
            """
            IF OBJECT_ID(N'study_sessions', N'U') IS NULL
            CREATE TABLE study_sessions (
            StudyId int IDENTITY(1,1) PRIMARY KEY,
            SessionDate DateTime NOT NULL,
            SessionScore int NOT NULL,
            StackId int NOT NULL,
            CONSTRAINT FK_study_sessions_stacks FOREIGN KEY (StackId)
            REFERENCES stacks (StackId)
            ON DELETE CASCADE
            )
            """);
    }

    public static int CreateStack()
    {
        AnsiConsole.Clear();
        var stackName = AnsiConsole.Prompt(
            new TextPrompt<string>("Please enter a [green]stack name[/]: ")
                .PromptStyle("blue")
                .AllowEmpty());
        if(stackName == "0") UserInput.GetUserInput();
        while (string.IsNullOrWhiteSpace(stackName))
        {
            stackName = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter a [green]stack name[/]: ")
                    .PromptStyle("blue")
                    .AllowEmpty());
            if(stackName == "0") UserInput.GetUserInput();
        }

        using var connection = new SqlConnection(ConnectionString);
        var command = "INSERT INTO stacks (StackName) VALUES (@StackName)";
        var stack = new Stacks { StackName = stackName };
        var stackCreation = connection.Execute(command, stack);
        AnsiConsole.MarkupLine($"[green]{stackCreation}[/] stack added. Press any key to continue.");
        Console.ReadKey();
        var getStackIdCommand = $"SELECT StackId from stacks WHERE StackName = '{stackName}'";
        var stackIdQuery = connection.Query<Stacks>(getStackIdCommand);
        var stackIdArray = stackIdQuery.Select(id => id.StackId).ToArray();
        var stackId = stackIdArray[0];
        return stackId;
    }

    public static void CreateFlashcard()
    {
        AnsiConsole.Clear();
        var newStackQuestion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Will this be for a new [blue]stack[/]?")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Yes", "No"
                }));
        if (newStackQuestion == "Yes")
        {
            var stackId = CreateStack();
            AnsiConsole.Clear();
            var flashCardIndex = 1;
            using var connection = new SqlConnection(ConnectionString);
            var cardFront = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter the flash card [green]question[/] or press 0 to return to main menu: ")
                    .PromptStyle("blue")
                    .AllowEmpty());
            if (cardFront == "0") UserInput.GetUserInput();
            while (string.IsNullOrWhiteSpace(cardFront))
            {
                cardFront = AnsiConsole.Prompt(
                    new TextPrompt<string>("[red]Empty value not allowed.[/]Please enter the flash card [green]question[/] or press 0 to return to main menu: ")
                        .PromptStyle("blue")
                        .AllowEmpty());
                if (cardFront == "0") UserInput.GetUserInput();
            }
            AnsiConsole.Clear();
            var cardBack = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter the flash card [green]answer[/] or press 0 to return to main menu: ")
                    .PromptStyle("blue")
                    .AllowEmpty());
            if (cardBack == "0") UserInput.GetUserInput();
            while (string.IsNullOrWhiteSpace(cardBack))
            {
                cardBack = AnsiConsole.Prompt(
                    new TextPrompt<string>("[red]Empty value not allowed.[/]Please enter the flash card [green]answer[/] or press 0 to return to main menu: ")
                        .PromptStyle("blue")
                        .AllowEmpty());
                if (cardBack == "0") UserInput.GetUserInput();
            }

            var cardCreationCommand =
                "INSERT INTO flash_cards (FlashcardIndex,CardFront,CardBack,StackId) VALUES (@FlashcardIndex,@CardFront,@CardBack,@StackId)";
            var flashcard = new Flashcards { FlashcardIndex = flashCardIndex,CardFront = cardFront,CardBack = cardBack,StackId = stackId};
            var cardCreation = connection.Execute(cardCreationCommand, flashcard);
            AnsiConsole.Write(new Markup($"[green]{cardCreation}[/] flashcard added. Press any key to continue."));
            Console.ReadKey();
        }
        else
        {
            var stackId = GetStacks("to get ID");
            using var connection = new SqlConnection(ConnectionString);
            var getFlashcardsCommand = $"SELECT FlashcardIndex FROM flash_cards WHERE StackId = '{stackId}'";
            var getFlashcards = connection.Query<Flashcards>(getFlashcardsCommand);
            var flashcardIndexes = getFlashcards.Select(flashcard => flashcard.FlashcardIndex).ToList();
            var flashcardIndex = flashcardIndexes.AsQueryable().LastOrDefault() + 1;

            var cardFront = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter the flash card [green]question[/] or press 0 to return to main menu: ")
                    .PromptStyle("blue")
                    .AllowEmpty());
            if (cardFront == "0") UserInput.GetUserInput();
            while (string.IsNullOrWhiteSpace(cardFront))
            {
                cardFront = AnsiConsole.Prompt(
                    new TextPrompt<string>("[red]Empty value not allowed.[/]Please enter the flash card [green]question[/] or press 0 to return to main menu: ")
                        .PromptStyle("blue")
                        .AllowEmpty());
                if (cardFront == "0") UserInput.GetUserInput();
            }
            AnsiConsole.Clear();
            var cardBack = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter the flash card [green]answer[/] or press 0 to return to main menu: ")
                    .PromptStyle("blue")
                    .AllowEmpty());
            if (cardBack == "0") UserInput.GetUserInput();
            while (string.IsNullOrWhiteSpace(cardBack))
            {
                cardBack = AnsiConsole.Prompt(
                    new TextPrompt<string>("[red]Empty value not allowed.[/]Please enter the flash card [green]answer[/] or press 0 to return to main menu: ")
                        .PromptStyle("blue")
                        .AllowEmpty());
                if (cardBack == "0") UserInput.GetUserInput();
            }

            var cardCreationCommand =
                "INSERT INTO flash_cards (FlashcardIndex,CardFront,CardBack,StackId) VALUES (@FlashcardIndex,@CardFront,@CardBack,@StackId)";
            var flashcard = new Flashcards { FlashcardIndex = flashcardIndex, CardFront = cardFront, CardBack = cardBack, StackId = stackId };
            var cardCreation = connection.Execute(cardCreationCommand, flashcard);
            AnsiConsole.Write(new Markup($"[green]{cardCreation}[/] flashcard added. Press any key to continue."));
            Console.ReadKey();
        }
    }

    public static void DeleteStack()
    {
        AnsiConsole.Clear();
        using var connection = new SqlConnection(ConnectionString);
        var selectStack = GetStacks("to delete");
        var deleteCommand = $"DELETE FROM stacks WHERE StackId = '{selectStack}'";
        var deleteStack = connection.Execute(deleteCommand);
        AnsiConsole.Write(new Markup($"[green]{deleteStack}[/] stack deleted. Press any key to continue."));
        Console.ReadKey();
    }

    public static void DeleteFlashcard()
    {
        AnsiConsole.Clear();
        var stackId = GetStacks("Where your flash card resides");
        var flashcardId = GetFlashcards("to delete",stackId);
        using var connection = new SqlConnection(ConnectionString);
        var deleteCommand = $"DELETE from flash_cards WHERE FlashcardId = '{flashcardId}'";
        var flashcardIndexCommand = $"SELECT FlashcardIndex from flash_cards WHERE FlashcardId = '{flashcardId}'";
        var flashcardQuery = connection.Query<Flashcards>(flashcardIndexCommand);
        var flashcardIndexIdList = flashcardQuery.Select(flashcard => flashcard.FlashcardIndex).ToList();
        var flashcardIndexId = flashcardIndexIdList[0];
        var deleteFlashcard = connection.Execute(deleteCommand);
        var flashcardIdIndexUpdateCommand =
            $"UPDATE flash_cards SET FlashcardIndex = FlashcardIndex - 1 WHERE FlashcardIndex > {flashcardIndexId} AND StackId = '{stackId}'";
        var updateFlashcardIndexes = connection.Execute(flashcardIdIndexUpdateCommand);
        AnsiConsole.MarkupLine($"[green]{deleteFlashcard}[/] flashcard deleted.");
        AnsiConsole.MarkupLine($"[green]{updateFlashcardIndexes}[/] flashcard indexes updated. Press any key to continue.");
        Console.ReadKey();
    }

    public static int GetStacks(string function)
    {
        using var connection = new SqlConnection(ConnectionString);
        var getStacksCommand = "SELECT * FROM stacks";
        var stacks = connection.Query<Stacks>(getStacksCommand);
        var stackList = stacks.Select(stack => stack.StackName).ToList();
        var selectStack = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Select a [blue]Stack[/] {function}")
                .PageSize(10)
                .AddChoices(stackList));
        var getStackIdCommand = $"SELECT StackId FROM stacks WHERE StackName = '{selectStack}'";
        var stackIdQuery = connection.Query<Stacks>(getStackIdCommand);
        var stackIdList = stackIdQuery.Select(stack => stack.StackId).ToList();
        var stackId = stackIdList[0];

        return stackId;
    }

    public static int GetFlashcards(string function, int stackId)
    {
        using var connection = new SqlConnection(ConnectionString);
        var getFlashcardsCommand = $"SELECT * FROM flash_cards WHERE StackId = '{stackId}'";
        var flashcards = connection.Query<Flashcards>(getFlashcardsCommand);
        var flashcardList = flashcards.Select(flashcard => flashcard.CardFront).ToList();
        var selectFlashcard = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Select a [blue]flashcard[/] {function}")
                .PageSize(10)
                .AddChoices(flashcardList));
        var getFlashcardIdCommand =
            $"SELECT FlashcardId FROM flash_cards WHERE CONVERT (VARCHAR, CardFront) = '{selectFlashcard}'";
        var flashcardIdQuery = connection.Query<Flashcards>(getFlashcardIdCommand);
        var flashcardIdList = flashcardIdQuery.Select(flashcard => flashcard.FlashcardId).ToList();
        var flashcardId = flashcardIdList[0];
        return flashcardId;
    }

    public static List<FlashcardDto> GetAllFlashcards(int stackId)
    {
        using var connection = new SqlConnection(ConnectionString);
        var getFlashcardsCommand = $"SELECT * FROM flash_cards WHERE StackId = '{stackId}'";
        var flashcards = connection.Query<Flashcards>(getFlashcardsCommand);
        var flashcardList = flashcards.ToList();
        var flashcardDtoList = FlashcardController.MapToDto(flashcardList);
        return flashcardDtoList;
    }
}