using Flashcards.Models;
using Spectre.Console;

namespace Flashcards;

internal class ActionManager
{
    internal string ConnectionString { get; init; }
    internal FlashcardsDbContext DbContext;

    private bool runApplication;

    public Dictionary<string, Action> Actions { get; private set; }
    
    public ActionManager(string connectionString)
    {
        ConnectionString = connectionString;
        DbContext = new FlashcardsDbContext(connectionString);
        runApplication = true;
        Actions = new Dictionary<string, Action>
        {
            { "Add a Stack", () => {AddStack();} },
            { "Edit a Stack",EditStack },
            { "Delete a Stack", DeleteStack },
            { "Add a Flash Card", AddFlashCard },
            { "Edit a Flash Card", EditFlashCard },
            { "Delete a Flash Card", DeleteFlashCard },
            { "Start Study Session", StartStudySession },
            { "View Study Sessions by Stack", ViewStudySession },
            { "Average Score Yearly Report for one Stack", ViewYearlyReport },
            { "Average Score Yearly Report All Stacks", ViewYearlyAllStacksReport },
            { "Monthly Sessions Report All Stacks", ViewMonthlySessionsAllStacksReport },
            { "Exit", Exit },
            { "Back to Main Menu", Cancel }
        };
    }

    internal void RunApp()
    {
        AnsiConsole.Clear();
        while (runApplication)
        {
            var choice = Menu.GetMainMenuChoices();

            if (IsSubMenu(choice))
            {
                choice = Menu.GetSubMenu(choice);
            }

            if (Actions.ContainsKey(choice))
            {
                Actions[choice].Invoke();
            }
            else
            {
                AnsiConsole.WriteLine("Invalid choice, please try again.");
                VisualizationEngine.DisplayContinueMessage();
            }
        }
    }

    private void ViewMonthlySessionsAllStacksReport()
    {
        string? year = UserInputs.GetYearInput("Enter a Year in format (yyyy)");
        if (year == "0") return;

        IEnumerable<AllStackYearlyReport>? report = DbContext.GetAllStacksSessionsReportByYear(Convert.ToInt32(year));
        if (report != null)
        {
            VisualizationEngine.DisplayAllStacksYearlySessionReport(report, $"Total Sessions All Stacks Report for Year [green]{year}.[/]");
        }
        else
        {
            AnsiConsole.Markup($"[marron] No session found for the year {year}");
        }
        VisualizationEngine.DisplayContinueMessage();
    }

    private void ViewYearlyAllStacksReport()
    {
        string? year = UserInputs.GetYearInput("Enter a Year in format (yyyy)");
        if (year == "0") return;

        IEnumerable<AllStackYearlyReport>? report = DbContext.GetAllStacksReportByYear(Convert.ToInt32(year));
        if (report != null)
        {
            VisualizationEngine.DisplayAllStacksYearlySessionReport(report, $"Average Score All Stacks Report for Year [green]{year}.[/]");
        }
        else
        {
            AnsiConsole.Markup($"[marron] No session found for the year {year}");
        }
        VisualizationEngine.DisplayContinueMessage();
    }

    private void ViewYearlyReport()
    {
        string? stackName = Menu.GetStackMenu(DbContext.GetAllStacks());
        if (stackName == Menu.CancelOperation) return;

        string? year = UserInputs.GetYearInput("Enter a Year in format (yyyy)");
        if (year == "0") return;

        Stack? stack = DbContext.GetStackByName(stackName);
        StackYearlyReport report = DbContext.GetStackReportByYear(stack.Id, Convert.ToInt32(year));
        if (report != null)
        {
            VisualizationEngine.DisplayYearlySessionReport(report, $"Average Score Year [green]{year}[/] Report for Stack [blue]{stackName}[/]");
        }
        else
        {
            AnsiConsole.Markup($"[marron] No session found for Stack {stackName} in year {year}");
        }
        VisualizationEngine.DisplayContinueMessage();

    }

    private void StartStudySession()
    {
        string? stackName = Menu.GetStackMenu(DbContext.GetAllStacks());
        if (stackName != Menu.CancelOperation)
        {
            Stack? stack = DbContext.GetStackByName(stackName);
            StudyGameEngine game = new StudyGameEngine(DbContext, stack);
            game.StartGame();
        }
    }

    private void ViewStudySession()
    {
        string? stackName = Menu.GetStackMenu(DbContext.GetAllStacks());
        if (stackName != Menu.CancelOperation)
        {
            Stack? stack = DbContext.GetStackByName(stackName);
            IEnumerable<StudySession>? studies = DbContext.GetAllStudySessionsByStackId(stack.Id);
            VisualizationEngine.DisplayStudySessions(studies, stack.StackName);
            VisualizationEngine.DisplayContinueMessage();
        }
    }

    private string? AddStack()
    {
        string stackName = UserInputs.GetStringInput("Please enter a [bold blue]Stack Name[/]");
        if (stackName == null) return null;
        while (DbContext.GetStackByName(stackName) != null)
        {
            AnsiConsole.Markup($"The Stack [bold maroon]{stackName}[/] already exists in Database.\n");
            stackName = UserInputs.GetStringInput("Please enter a [bold blue]Stack Name[/]");
            if (stackName == null) return null;
        }
        int result = DbContext.AddStack(new StackDto { StackName = stackName });
        VisualizationEngine.ShowResultMessage(result, $"The Stack [bold green]{stackName}[/] is inserted");
        return stackName;
    }

    private void EditStack()
    {
        IEnumerable<Stack>? stacks = DbContext.GetAllStacks();
        var choice = Menu.GetStackMenu(stacks);
        if (choice != Menu.CancelOperation)
        {
            Stack stack = DbContext.GetStackByName(choice);
            var oldStackName = stack.StackName;
            stack.StackName = UserInputs.GetStringInput("Please enter a [bold blue]Stack Name[/]");
            if (stack.StackName != null)
            {
                var res = stacks.FirstOrDefault(s => s.Id != stack.Id && s.StackName == stack.StackName);
                while (res != null)
                {
                    AnsiConsole.Markup($"{stack.StackName} already exits\n");
                    VisualizationEngine.DisplayContinueMessage();
                    stack.StackName = UserInputs.GetStringInput("Please enter a [bold blue]Stack Name[/]");
                    if(stack.StackName == null)
                    {
                        EditStack();
                        return;
                    }
                    res = stacks.FirstOrDefault(s => s.Id != stack.Id && s.StackName == stack.StackName);
                }
                int result = DbContext.UpdateStackById(stack);
                VisualizationEngine.ShowResultMessage(result, $"The Stack [bold green]{choice}[/] is updated");
            }
            else
            {
                EditStack();
            }
        }
    }

    private void DeleteStack()
    {
        IEnumerable<Stack>? stacks = DbContext.GetAllStacks();
        var choice = Menu.GetStackMenu(stacks);
        if (choice != Menu.CancelOperation)
        {
            int result = DbContext.DeleteStackByStackName(choice);
            VisualizationEngine.ShowResultMessage(result, $"The Stack [bold green]{choice}[/] is deleted");
        }
    }

    private void AddFlashCard()
    {
        bool isNew = AnsiConsole.Confirm("Do you want to add the Flash Card to new Stack");
        string? stackName = GetNewOrExistingStack(isNew);

        if (stackName == null || stackName == Menu.CancelOperation) return;
        Stack? stack = DbContext.GetStackByName(stackName);

        (string? front, string? back) = UserInputs.GetFlashCardData(DbContext, stack);

        if (front == null || back == null)
        {
            if (isNew)
            {
                DbContext.DeleteStackByStackName(stackName);
            }
            return;
        }

        FlashcardDto flashcard = new FlashcardDto(stack.Id, front, back);
        int result = DbContext.AddFlashcard(flashcard);
        VisualizationEngine.ShowResultMessage(result, $"The Flashcard in [bold green]{stackName}[/] is inserted");
    }

    private void EditFlashCard()
    {
        string choice = Menu.GetStackMenu(DbContext.GetAllStacks());
        if (choice == Menu.CancelOperation) return;

        Stack stack = DbContext.GetStackByName(choice);
        IEnumerable<Flashcard>? flashcards = DbContext.GetAllFlashcardByStackId(stack.Id);
        string selectedCard = Menu.GetFlashCardsChoices(flashcards);

        if (selectedCard == Menu.CancelOperation)
        {
            EditFlashCard();
        }
        else
        {
            Flashcard flashcard = DbContext.GetFlashCardByStackIdAndFront(stack.Id, selectedCard);
            (flashcard.Front, flashcard.Back) = UserInputs.GetFlashCardData(DbContext, stack);

            if (flashcard.Front == null || flashcard.Back == null) return;

            int result = DbContext.UpdateFlashcard(flashcard);
            VisualizationEngine.ShowResultMessage(result, $"The Flashcard in [bold green]{flashcard.Front}[/] is updated");
        }
    }

    private void DeleteFlashCard()
    {
        string choice = Menu.GetStackMenu(DbContext.GetAllStacks());
        if (choice != Menu.CancelOperation)
        {
            Stack stack = DbContext.GetStackByName(choice);
            IEnumerable<Flashcard>? flashcards = DbContext.GetAllFlashcardByStackId(stack.Id);
            string question = Menu.GetFlashCardsChoices(flashcards);

            if (question == Menu.CancelOperation)
            {
                DeleteFlashCard();
            }
            else
            {
                int result = DbContext.DeleteFlashcardByFront(question, stack.Id);
                VisualizationEngine.ShowResultMessage(result, $"The Flashcard [bold green]{question}[/] is deleted");
            }
        }
    }

    private bool IsSubMenu(string choice) => Menu.MainMenus.ContainsKey(choice);

    private string? GetNewOrExistingStack(bool isNew) => isNew ? AddStack() : Menu.GetStackMenu(DbContext.GetAllStacks());

    private void Exit() => runApplication = false;

    private void Cancel() { }

}