using System.Configuration;
using System.Linq;

public class App
{
    private UserInput _userInput;
    private DatabaseManager _databaseManager;
    private StackRepository _stackRepo;
    private StackController _stackController;
    private FlashcardRepository _flashcardRepo;
    private FlashcardController _flashcardController;
    private List<Stack> stackList;

    public void Run()
    {
        _userInput = new UserInput();
        _databaseManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        _stackRepo = new StackRepository(_databaseManager);
        _stackController = new StackController(_stackRepo);
        _flashcardRepo = new FlashcardRepository(_databaseManager);
        _flashcardController = new FlashcardController(_flashcardRepo);

        _databaseManager.InitializeDB();

        while (true)
        {
            var mainMenuOption = _userInput.MainMenu();

            switch (mainMenuOption)
            {
                case MainMenuOptions.Stacks:
                    StackMenu();
                    break;
                case MainMenuOptions.Flashcards:
                    break;
                case MainMenuOptions.Study:
                    break;
                case MainMenuOptions.InsertTestData:
                    InsertTestData();
                    break;
                case MainMenuOptions.Exit:
                    Environment.Exit(0);
                    break;

            }
        }

    }

    public void StackMenu()
    {
        stackList = _stackController.GetStacks();

        if (stackList.Count == 0)
        {
            Stack stack;
            Console.WriteLine("No stacks to show. Press any key to continue to create a new stack.");
            Console.ReadKey(true);
            stack = _userInput.InsertStack(stackList);
            _stackController.CreateStack(stack.Name);
        }
        else
        {
            Stack stack;
            var option = _userInput.StacksManu();
            switch (option)
            {
                case StackOptions.Insert:
                    stack = _userInput.InsertStack(stackList);
                    _stackController.CreateStack(stack.Name);
                    break;
                case StackOptions.Select:
                    stack = _userInput.SelectStack(stackList);
                    ManageStack(stack);
                    break;
                case StackOptions.Exit:
                    Console.Clear();
                    break;
            }
        }
    }

    public void ManageStack(Stack stack)
    {
        stackList = _stackController.GetStacks();

        var option = _userInput.ManageStacksManu(stack);
        var currentCards = _flashcardController.GetFlashcardsByStack(stack);

        switch (option)
        {
            case ManageStackOption.ChangeStack:
                stack = _userInput.SelectStack(stackList);
                ManageStack(stack);
                break;
            case ManageStackOption.ViewCardsAll:
                ShowFlashcardsOption(currentCards.Flashcards);
                break;
            case ManageStackOption.ViewCardsAmount:
                List<Flashcard> flashCards = new List<Flashcard>(currentCards.Flashcards.Take(_userInput.FlashcardAmount(currentCards.Flashcards)));
                ShowFlashcardsOption(flashCards);
                break;
            case ManageStackOption.CreateCard:
                var flashcard = _userInput.CreateFlashcard(stack);
                _flashcardController.CreateFlashcard(flashcard);
                Console.WriteLine("Entry added");
                break;
            case ManageStackOption.EditCard:
                break;
            case ManageStackOption.DeleteCard:
                break;
            case ManageStackOption.DeleteStack:
                _stackController.DeleteStack(stack.Name);
                break;
            case ManageStackOption.Exit:
                Console.Clear();
                break;
        }
    }

    public void InsertTestData()
    {
        var numbers = _userInput.InsertTestData();
        List<string> randomStacks = new List<string> { "Introduction to C#", "Object-Oriented Programming", "Data Structures in C#", "Algorithms", "LINQ", "Entity Framework", "ASP.NET Core", "Blazor", "Xamarin", "Design Patterns", "SOLID Principles", "Unit Testing with xUnit", "Dependency Injection", "Multithreading and Asynchronous Programming", "RESTful Services with ASP.NET", "Microservices Architecture", "Azure DevOps", "Continuous Integration/Continuous Deployment (CI/CD)", "Docker for .NET Developers", "Kubernetes", "Security in .NET", "Performance Tuning", "C# 9 and Newer Features", "Code Refactoring", "Clean Code", "Version Control with Git", "Agile Development Practices", "Testing and Mocking", "Debugging Techniques", "Memory Management in C#" };
        Dictionary<string, string> randomFlashcards = new()
        {
            { "LINQ?", "Query tool" },
            { "ORM?", "Mapper" },
            { "ASP.NET Core?", "Web framework" },
            { "SOLID?", "Design rules" },
            { "Inheritance?", "Code reuse" },
            { "Polymorphism?", "Method override" },
            { "Encapsulation?", "Data hiding" },
            { "Abstraction?", "Simplification" },
            { "Interface?", "Contract" },
        };

        for (int i = 0; i < numbers.Item1; i++)
        {
            var stackName = randomStacks[Random.Shared.Next(0, randomStacks.Count)];
            randomStacks.Remove(stackName);
            _stackController.CreateStack(stackName);

            var list = _stackController.GetStacks();
            var currentStack = list.First(s => s.Name == stackName);

            for (int j = 0; j < numbers.Item2; j++)
            {
                Flashcard flashcard = new Flashcard();
                var randomCard = randomFlashcards.ElementAt(Random.Shared.Next(0, randomFlashcards.Count));
                flashcard.Question = randomCard.Key;
                flashcard.Answer = randomCard.Value;
                flashcard.StackId = currentStack.Id;
                _flashcardController.CreateFlashcard(flashcard);
            }

        }
    }

    private void ShowFlashcardsOption(List<Flashcard> flashcards)
    {
        if (flashcards.Count == 0)
        {
            Console.WriteLine("No cards to show for this stack. Press any key to continue");
            Console.ReadKey();
            return;
        }

        _userInput.ShowFlashcards(flashcards);
        Console.WriteLine("Press any key to go back to main menu.");
        Console.ReadKey();
    }
}

