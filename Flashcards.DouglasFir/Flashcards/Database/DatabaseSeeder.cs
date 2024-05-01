using Flashcards.Models;
using Flashcards.DAO;
using System.Data;
using Spectre.Console;
using Flashcards.Services;

namespace Flashcards.Database;

public class DatabaseSeeder
{
    private readonly DatabaseSeederDao _databaseSeederDao;
    private readonly InputHandler _inputHandler;

    public DatabaseSeeder(DatabaseContext databaseContext, InputHandler inputHandler)
    {
        _databaseSeederDao = new DatabaseSeederDao(databaseContext);
        _inputHandler = inputHandler;
    }

    public void SeedDatabase()
    {
        var stackNameSOLID = "SOLID";
        var stackNameDesignPatterns = "DesignPatterns";
        var stackNameProgrammingBasics = "ProgrammingBasics";

        var stacks = new List<Stack>
        {
            new Stack(stackNameSOLID),
            new Stack(stackNameDesignPatterns),
            new Stack(stackNameProgrammingBasics)
        };

        // Flashcards for each stack are created with StackID's of 0, which will be updated to the correct ID when inserted into the database
        var flashCards = new Dictionary<string, List<FlashCard>>
        {
            {
                stackNameSOLID, new List<FlashCard>
                {
                    new FlashCard("What does the S in SOLID stand for?", "Single Responsibility Principle", 0),
                    new FlashCard("What does the O in SOLID stand for?", "Open/Closed Principle", 0),
                    new FlashCard("What does the L in SOLID stand for?", "Liskov Substitution Principle", 0),
                    new FlashCard("What does the I in SOLID stand for?", "Interface Segregation Principle", 0),
                    new FlashCard("What does the D in SOLID stand for?", "Dependency Inversion Principle", 0)
                }
            },
            {
                stackNameDesignPatterns, new List<FlashCard>
                {
                    new FlashCard("Which design pattern provides a way to access the elements of an aggregate object sequentially without exposing its underlying representation?", "Iterator Pattern", 0),
                    new FlashCard("What pattern defines a family of algorithms, encapsulates each one, and makes them interchangeable?", "Strategy Pattern", 0),
                    new FlashCard("Which pattern is used to reduce the complexity of creating complex objects?", "Builder Pattern", 0),
                    new FlashCard("Which pattern helps to hide the complexity of a system by providing a simplified interface?", "Facade Pattern", 0),
                    new FlashCard("Which pattern creates a duplicate of an existing object?", "Prototype Pattern", 0)
                }
            },
            {
               stackNameProgrammingBasics, new List<FlashCard>
                {
                    new FlashCard("What is the keyword used to define a variable in Python?", "var", 0),
                    new FlashCard("What does '&&' signify in most programming languages?", "Logical AND", 0),
                    new FlashCard("What keyword is used to define a function in Python?", "def", 0),
                    new FlashCard("What does 'null' represent in programming?", "No value or a null reference", 0),
                    new FlashCard("What is the purpose of the 'return' keyword in programming?", "To exit a function and optionally return a value", 0)
                }
            }
        };

        InsertSeedData(stacks, flashCards);
    }

    private void InsertSeedData(List<Stack> stacks, Dictionary<string, List<FlashCard>> flashCards)
    {
        AnsiConsole.WriteLine("Starting database seeding...");

        try
        {
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .Start("Processing...", ctx =>
                {
                    _databaseSeederDao.InsertStacksAndFlashCards(stacks, flashCards);
                });

            Utilities.DisplaySuccessMessage("Database seeded successfully!");
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error seeding database. No data inserted.", ex.Message);
        }
        finally
        {
            _inputHandler.PauseForContinueInput();
        }
    }
}
