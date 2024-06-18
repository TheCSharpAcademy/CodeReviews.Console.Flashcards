using System.Configuration;

public class App
{
    private UserInput _userInput;
    private DatabaseManager _databaseManager;
    private StackRepository _stackRepo;
    private StackController _stackController;
    private FlashcardRepository _flashcardRepo;
    private FlashcardController _flashcardController;
    private StudyRepository _studyRepo;
    private StudyController _studyController;
    private List<Stack> stackList;

    public void Run()
    {
        _userInput = new UserInput();
        _databaseManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        _stackRepo = new StackRepository(_databaseManager);
        _stackController = new StackController(_stackRepo);
        _flashcardRepo = new FlashcardRepository(_databaseManager);
        _flashcardController = new FlashcardController(_flashcardRepo);
        _studyRepo = new StudyRepository(_databaseManager);
        _studyController = new StudyController(_studyRepo);

        _databaseManager.InitializeDB();

        while (true)
        {
            var mainMenuOption = _userInput.MainMenu();
            stackList = _stackController.GetStacks();

            switch (mainMenuOption)
            {
                case MainMenuOptions.Stacks:
                    StackMenu();
                    break;
                case MainMenuOptions.Flashcards:
                    ManageFlashcardMenu();
                    break;
                case MainMenuOptions.Study:
                    ManageStudySession();
                    break;
                case MainMenuOptions.InsertTestData:
                    InsertTestData();
                    break;
                case MainMenuOptions.Reports:
                    ManageStudyReport();
                    break;
                case MainMenuOptions.Exit:
                    Environment.Exit(0);
                    break;
            }
        }

    }

    public void StackMenu()
    {
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
                Console.WriteLine("Press any key to go back to main menu.");
                Console.ReadKey(true);
                break;
            case ManageStackOption.ViewCardsAmount:
                List<Flashcard> flashCards = new List<Flashcard>(currentCards.Flashcards.Take(_userInput.FlashcardAmount(currentCards.Flashcards)));
                ShowFlashcardsOption(flashCards);
                Console.WriteLine("Press any key to go back to main menu.");
                Console.ReadKey(true);
                break;
            case ManageStackOption.CreateCard:
                var flashcard = _userInput.CreateFlashcard(stack);
                _flashcardController.CreateFlashcard(flashcard);
                Console.WriteLine("Entry added");
                break;
            case ManageStackOption.DeleteStack:
                _stackController.DeleteStack(stack.Name);
                break;
            case ManageStackOption.Exit:
                Console.Clear();
                break;
        }
    }

    public void ManageFlashcardMenu()
    {
        var option = _userInput.ViewFlashCardsMainMenu();

        switch (option)
        {
            case FlashcardsMenuOptions.ViewFlashcardByStack:

                if (stackList.Count == 0)
                {
                    Console.WriteLine("No stacks to show. Press any key to return.");
                    Console.ReadKey(true);
                }
                else
                {
                    var stack = _userInput.SelectStack(stackList);
                    var currentCards = _flashcardController.GetFlashcardsByStack(stack);
                    if (currentCards.Flashcards.Count == 0)
                    {
                        Console.WriteLine("No flash cards connected to this stack. Press any key to return to main menu.");
                        Console.ReadKey(true);
                        break;
                    }
                    ShowFlashcardsOption(currentCards.Flashcards);
                    var flashcard = _userInput.GetFlashcard(currentCards.Flashcards);
                    ManagerFlashcard(flashcard);
                }
                break;
            case FlashcardsMenuOptions.Exit:
                Console.Clear();
                break;
        }
    }

    public void ManagerFlashcard(Flashcard flashcard)
    {
        var option = _userInput.ManageeFlashcard(flashcard);

        switch (option)
        {
            case FlashcardsManageOptions.Delete:
                _flashcardController.DeleteFlashcard(flashcard.Id);
                break;
            case FlashcardsManageOptions.Edit:
                var newCard = _userInput.UpdateCard(flashcard);
                _flashcardController.UpdateFlashcard(newCard, flashcard);
                break;
            case FlashcardsManageOptions.Exit:
                Console.Clear();
                break;
        }
    }

    public void ManageStudySession()
    {
        var options = _userInput.StudySessionMenu();

        switch (options)
        {
            case StudySessionOptions.SelectStack:
                var stack = _userInput.SelectStack(stackList);
                var flashcards = _flashcardController.GetFlashcardsByStack(stack);

                if (flashcards.Flashcards.Count > 0)
                {
                    var studySession = _userInput.NewStudySession(flashcards);
                    _studyController.AddStudy(studySession);
                }
                else
                {
                    Console.WriteLine("Cannot study, there are no flashcards in this stack. Press any key to continue.");
                    Console.ReadKey(true);
                }
                break;
            case StudySessionOptions.PreviousSessions:
                var studySessions = _studyController.GetStudySessions(stackList);
                _userInput.ViewPreviousStudySessions(studySessions);
                Console.WriteLine("Press any key to go back to main menu");
                Console.ReadKey(true);

                break;
            case StudySessionOptions.Exit:
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
            var randomStacksCount = randomStacks.Count;

            if (randomStacksCount == 0) return;
            
            var stackName = randomStacks[Random.Shared.Next(0, randomStacksCount)];
            randomStacks.Remove(stackName);

            if (!_stackController.GetStacks().Any(n => n.Name.ToLower() == stackName.ToLower()))
            {
                _stackController.CreateStack(stackName);
            }
            else
            {
                continue;
            }
            

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

    public void ManageStudyReport()
    {
        var year = _userInput.GetReportYear();
        var reports = _studyController.GetMonthlyReports(year, stackList);
        _userInput.ViewReportByYear(reports);

        Console.WriteLine("\nPress any key to go back to main menu.");
        Console.ReadKey(true);
    }

    private void ShowFlashcardsOption(List<Flashcard> flashcards)
    {
        if (flashcards.Count == 0)
        {
            Console.WriteLine("No cards to show for this stack. Press any key to continue");
            Console.ReadKey(true);
            return;
        }

        _userInput.ShowFlashcards(flashcards);
    }
}

