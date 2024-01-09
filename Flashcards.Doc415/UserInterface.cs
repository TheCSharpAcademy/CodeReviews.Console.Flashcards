using FlashCards.Doc415.Models;
using Spectre.Console;
using static FlashCards.Doc415.Enums;
namespace FlashCards.Doc415;

internal class UserInterface
{
    internal DataAccess dataAccess = new();

    public void InitializeMenu()
    {
        while (true)
        {
            AnsiConsole.Write(new FigletText("Flashcards").Color(Color.DarkMagenta));
            var selection = AnsiConsole.Prompt(new SelectionPrompt<MainMenuSelections>()
                .Title("Welcome, what would you like to do?")
                .AddChoices(
                    MainMenuSelections.ManageFlashcards,
                    MainMenuSelections.ManageStacks,
                    MainMenuSelections.StudyArea,
                    MainMenuSelections.Quit
                ));

            switch (selection)
            {
                case MainMenuSelections.ManageFlashcards:
                    ManageFlashcards();
                    Console.Clear();
                    break;
                case MainMenuSelections.ManageStacks:
                    ManageStacks();
                    Console.Clear();
                    break;
                case MainMenuSelections.StudyArea:
                    StudyAreaMenu();
                    Console.Clear();
                    break;
                case MainMenuSelections.Quit:
                    AnsiConsole.Write(new FigletText("Goodby!").Color(Color.Green3));
                    Environment.Exit(0);
                    break;
            }
        }
    }

    void ManageFlashcards()
    {
        Console.Clear();
        while (true)
        {
            AnsiConsole.Write(new FigletText("Flashcards").Color(Color.DarkMagenta));
            var selection = AnsiConsole.Prompt(new SelectionPrompt<FlashcardSelections>()
                 .Title("[Green]----Manage Flashcards----[/]\n What would you like to do?")
                 .AddChoices(
                     FlashcardSelections.ViewFlashcards,
                     FlashcardSelections.AddFlashcard,
                     FlashcardSelections.DeleteFlashcard,
                     FlashcardSelections.UpdateFlashcard,
                     FlashcardSelections.ReturnToMainMenu
                 ));

            switch (selection)
            {
                case FlashcardSelections.ViewFlashcards:
                    ViewFlashcards();
                    Console.Clear();
                    break;
                case FlashcardSelections.AddFlashcard:
                    AddFlashcard();
                    Console.Clear();
                    break;
                case FlashcardSelections.DeleteFlashcard:
                    DeleteFlashcard();
                    Console.Clear();
                    break;
                case FlashcardSelections.UpdateFlashcard:
                    UpdateFlashcard();
                    Console.Clear();
                    break;
                case FlashcardSelections.ReturnToMainMenu:
                    Console.Clear();
                    InitializeMenu();
                    break;
            }
        }
    }


    void ManageStacks()
    {
        Console.Clear();
        while (true)
        {
            AnsiConsole.Write(new FigletText("Flashcards").Color(Color.DarkMagenta));
            var selection = AnsiConsole.Prompt(new SelectionPrompt<StackSelections>()
                 .Title("[Green]----Manage Stacks----[/]\n What would you like to do?")
                 .AddChoices(
                     StackSelections.ViewStacks,
                     StackSelections.AddStack,
                     StackSelections.DeleteStack,
                     StackSelections.UpdateStack,
                     StackSelections.ReturnToMainMenu
                 ));

            switch (selection)
            {
                case StackSelections.ViewStacks:
                    ViewStacks();
                    Console.Clear();
                    break;
                case StackSelections.AddStack:
                    AddStack();
                    Console.Clear();
                    break;
                case StackSelections.DeleteStack:
                    DeleteStack();
                    Console.Clear();
                    break;
                case StackSelections.UpdateStack:
                    UpdateStack();
                    Console.Clear();
                    break;
                case StackSelections.ReturnToMainMenu:
                    Console.Clear();
                    InitializeMenu();
                    break;
            }
        }
    }

    void StudyAreaMenu()
    {
        Console.Clear();
        while (true)
        {
            AnsiConsole.Write(new FigletText("Flashcards").Color(Color.DarkMagenta));
            var selection = AnsiConsole.Prompt(new SelectionPrompt<StudyAreaSelections>()
                .Title("[Green]----Study Area----[/]\n What would you like to do?")
                .AddChoices(
                    StudyAreaSelections.StartNewStudy,
                    StudyAreaSelections.ViewStudies,
                    StudyAreaSelections.Statistics,
                    StudyAreaSelections.QuitToMainMenu
                ));

            switch (selection)
            {
                case StudyAreaSelections.StartNewStudy:
                    StartNewStudy();
                    Console.Clear();
                    break;
                case StudyAreaSelections.ViewStudies:
                    ViewStudies();
                    Console.Clear();
                    break;
                case StudyAreaSelections.Statistics:
                    Statistics();
                    Console.Clear();
                    break;
                case StudyAreaSelections.QuitToMainMenu:
                    Console.Clear();
                    InitializeMenu();
                    break;
            }
        }
    }

    void Statistics() 
    {
        var reports = dataAccess.GetReport();
        var stacks = dataAccess.GetAllStacks();
        Console.Write("Year".PadRight(8) + "Month".PadRight(8) + "Stac".PadRight(8) +"Studies".PadRight(8) + "Avarage score\n");
        foreach ( var report in reports)
        {
            var stackName = stacks.Single(s => s.Id == report.StackId).Name;
            Console.Write(report.Year.PadRight(8)+report.Month.PadRight(8)+stackName.PadRight(8)+report.StudyCount.ToString().PadRight(8)+report.Avarage);
        }
        Console.ReadLine();
    }

    void StartNewStudy() 
    {
        int stackId = ChooseStack("Select the stack to practice.");
        var flashcardsInStack = dataAccess.GetStackFlashcards(stackId);
        if (flashcardsInStack.Count() < 1)
        {
            Console.WriteLine("There are no flashcards in selected stack. Press Enter to return study are menu.");
            StudyAreaMenu();
        }
        flashcardsInStack = Shuffle(flashcardsInStack);
        decimal totalQuestions = flashcardsInStack.Count();
        int score = 0;

        foreach (var flashcard in flashcardsInStack)
        {
            string answer = AnsiConsole.Ask<string>($"Question: {flashcard.Question}").ToLower();
            if (answer == flashcard.Answer)
            {
                score++;
                Console.WriteLine("Correct!");
            }
            else
            {
                Console.WriteLine("Wrong answer!");
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        score = Convert.ToInt32(score / totalQuestions * 100);
        Console.WriteLine($"Total Score: {score}");

        dataAccess.AddStudy(stackId, score);

        bool studyAgain = AnsiConsole.Confirm("Would you like to study another stack?");
        if (studyAgain) { StartNewStudy(); }
    }

    void ViewStudies()
    {
        var stacks = dataAccess.GetAllStacks();
        var studies = dataAccess.GetAllStudies();
        Console.WriteLine("Date".PadRight(13) + "Score".PadRight(6) + "Stack");
        foreach (var study in studies)
        {
            var stackName = stacks.Single(s => s.Id == study.StackId).Name;
            DateTime date = DateTime.Parse(study.Date);
            string studyDate = date.ToString("dd-MM-yyyy");
            Console.WriteLine(studyDate.PadRight(13) + study.Score.ToString().PadRight(6) + stackName);
        }
        Console.ReadLine();
    }

    IEnumerable<T> Shuffle<T>(IEnumerable<T> inputList)
    {
        var output = inputList.ToArray();
        Random random = new();
        for (int i = 0; i < output.Length; i++)
        {
            int r = random.Next(i, output.Length);
            var temp = output[i];
            output[i] = output[r];
            output[r] = temp;
        }
        return output.ToList();
    }
    void ViewFlashcards()
    {
        var flashcards = dataAccess.GetAllFlashcards();
        DisplayFlashcards(flashcards);
    }

    void DisplayFlashcards(IEnumerable<Flashcard> flashcards)
    {
        var stacks = dataAccess.GetAllStacks();  // I dont want to query the db for each flashcard to get the name of stack it belongs to
        string header = string.Format("[deeppink1_1]{0}[/][indianred_1]{1}{2}{3}[/]\n", "Id".PadRight(10), "Question".PadRight(30), "Answer".PadRight(30), "Inside stack");
        AnsiConsole.Markup(header);
        int index = 1;
        foreach (var card in flashcards)
        {
            var stackName = stacks.Single(s => s.Id == card.StackId).Name;
            Console.WriteLine(string.Format("{0}{1}{2}{3}",
                                                          index.ToString().PadRight(10),
                                                          card.Question.PadRight(30),
                                                          card.Answer.PadRight(30),
                                                          stackName
                                                          ));
            index++;
        }
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    void UpdateFlashcard()
    {
        int stack = ChooseStack("Select the stack you want to update a flashcard.");
        var flashcardsInStack = dataAccess.GetStackFlashcards(stack);
        var cardlist = flashcardsInStack.Select(x => x.Question).ToArray();

        var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                            .Title("Select flashcard to update: ")
                                            .AddChoices(cardlist));
        var flashcardId = flashcardsInStack.Single(x => x.Question == selection).Id;

        string newQuestion = AnsiConsole.Ask<string>("Enter Question: ");
        string newAnswer = AnsiConsole.Ask<string>("Enter Answer: ");

        int stackId = ChooseStack("Select the stack you want to insert the flashcard.");
        try
        {
            dataAccess.UpdateFlashcard(flashcardId, newQuestion, newAnswer, stackId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error while updating the flashcard: {ex.Message}");
            Console.ReadLine();
        }
    }

    void AddFlashcard()
    {
        Flashcard flashcard = new();

        flashcard.StackId = ChooseStack("Choose stack to add flashcard");

        flashcard.Question = AnsiConsole.Ask<string>("Enter Question: ");
        while (string.IsNullOrEmpty(flashcard.Question))
        {
            flashcard.Question = AnsiConsole.Ask<string>("Question can't be empty. Try again");
        }

        flashcard.Answer = AnsiConsole.Ask<string>("Enter Answer: ").ToLower();
        while (string.IsNullOrEmpty(flashcard.Answer))
        {
            flashcard.Answer = AnsiConsole.Ask<string>("Answer can't be empty. Try again");
        }

        dataAccess.AddFlashcard(flashcard);
    }

    int ChooseStack(string promptMessage)
    {
        var stacks = dataAccess.GetAllStacks();
        var stackNames = stacks.Select(x => x.Name).ToArray();

        if (stackNames.Length < 1)
        {
            Console.Write("There is no Stack.");
        }

        var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                            .Title(promptMessage)
                                            .AddChoices(stackNames));
        var stackId = stacks.Single(x => x.Name == selection).Id;
        return stackId;
    }

    void DeleteFlashcard()
    {
        int stack = ChooseStack("Select the stack you want to delete a flashcard.");
        var flashcardsInStack = dataAccess.GetStackFlashcards(stack);
        var cardlist = flashcardsInStack.Select(x => x.Question).ToArray();


        var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                            .Title("Select flashcard to delete: ")
                                            .AddChoices(cardlist));
        var flashcardId = flashcardsInStack.Single(x => x.Question == selection).Id;

        if (AnsiConsole.Confirm("Are you sure?", false))
        {
            try
            {
                dataAccess.DeleteFlashcard(flashcardId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error deleting flashcard: {ex.Message}");
            }
        }
    }

    void ViewStacks()
    {
        var stacks = dataAccess.GetAllStacks();
        AnsiConsole.Markup("[deeppink1_1]Id[/]     [indianred_1]Stack name[/]\n-----------------\n");
        int index = 1;
        foreach (var stack in stacks)
        {
            Console.WriteLine(string.Format("{1} {0}", stack.Name, index.ToString().PadRight(6)));
            index++;
        }
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    void UpdateStack()
    {
        int stackId = ChooseStack("Choose stack to update.");
        string newName = AnsiConsole.Ask<string>("Enter a new name for stack: ");
        dataAccess.UpdateStack(stackId, newName);
    }

    void AddStack()
    {
        CardStack stack = new CardStack();
        stack.Name = AnsiConsole.Ask<string>("Enter Stack name:");

        while (string.IsNullOrEmpty(stack.Name))
        {
            stack.Name = AnsiConsole.Ask<string>("Stack name can't be empty. Try again:");
        }
        DataAccess dataAccess = new DataAccess();
        dataAccess.AddStack(stack);
    }
    void DeleteStack()
    {
        int stackId = ChooseStack("Choose stack to delete.");
        if (AnsiConsole.Confirm("Are you sure?", false))
        {
            try
            {
                dataAccess.DeleteStack(stackId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error deleting stack: {ex.Message}");
            }
        }
    }
}

