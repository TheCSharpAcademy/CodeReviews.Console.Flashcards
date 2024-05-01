using Flashcards.Enums;
using Flashcards.Services;
using Flashcards.Application.Helpers;
using Flashcards.Database;
using Flashcards.DTO;
using Flashcards.DAO;
using Spectre.Console;

namespace Flashcards.Application;

public class AppStudySession
{
    private readonly StackDao _stackDao;
    private readonly FlashCardDao _flashCardDao;
    private readonly StudySessionDao _studySessionDao;
    private readonly InputHandler _inputHandler;
    private readonly ManageStacksHelper _manageStacksHelper;
    private readonly string _pageHeader = "Study Session";
    private bool _running;

    public AppStudySession(DatabaseContext databaseContext, InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
        _stackDao = new StackDao(databaseContext);
        _flashCardDao = new FlashCardDao(databaseContext);
        _studySessionDao = new StudySessionDao(databaseContext);
        _manageStacksHelper = new ManageStacksHelper(_stackDao, _flashCardDao, _inputHandler);

        _running = true;
    }

    public void Run()
    {
        while (_running)
        {
            AnsiConsole.Clear();
            Utilities.DisplayPageHeader(_pageHeader);
            PromptForSessionAction();
        }
    }

    private void PromptForSessionAction()
    {
        StudySessionMenuOption selectedOption = _inputHandler.PromptMenuSelection<StudySessionMenuOption>();
        ExecuteSelectedOption(selectedOption);
    }

    private void ExecuteSelectedOption(StudySessionMenuOption option)
    {
        switch (option)
        {
            case StudySessionMenuOption.Cancel:
                CloseSession();
                break;
            case StudySessionMenuOption.StartNewStudySession:
                HandleStartNewStudySessionSelection();
                break;
        }
    }

    private void CloseSession()
    {
        _running = false;
    }

    private void HandleStartNewStudySessionSelection()
    {
        StackDto selectedStack = _inputHandler.PromptForSelectionListStacks(_stackDao.GetAllStacks(), "Select a stack to study:");
        if (selectedStack == null)
        {
            _manageStacksHelper.HandleNoStacksFound();
            return;
        }

        IEnumerable<FlashCardDto> flashCards = _flashCardDao.GetAllFlashCardsByStackId(selectedStack);
        if (flashCards == null)
        {
            _manageStacksHelper.HandleNoFlashCardsFound();
            return;
        }

        StartStudySession(selectedStack, flashCards);
    }

    private void StartStudySession(StackDto stack, IEnumerable<FlashCardDto> flashCards)
    {
        int count = 0;
        int score = 0;

        do
        {
            FlashCardDto flashCard = flashCards.ElementAt(count);
            AnsiConsole.Clear();
            Utilities.DisplayPageHeader("Study Session");
            Utilities.DisplayFlashCardFront(flashCard);

            var response = _inputHandler.PromptForNonEmptyString("Enter your response:");
            
            if (Utilities.StringTrimLower(response) == Utilities.StringTrimLower(flashCard.Back!))
            {
                Utilities.DisplaySuccessMessage("Correct!");
                score++;
            }
            else
            {
                Utilities.DisplayWarningMessage("Incorrect!");
            }

            Utilities.DisplayInformationConsoleMessage($"Correct Answer: {flashCard.Back}");
            Utilities.DisplaySuccessMessage($"Score: {score}/{count + 1}");
            _inputHandler.PauseForContinueInput();
            count++;

        } while (count < flashCards.Count());

        HandleStudySessionEnd(stack, score);
    }

    private void HandleStudySessionEnd(StackDto stack, int score)
    {
        AnsiConsole.Clear();
        Utilities.DisplaySuccessMessage("Study session complete!");
        Utilities.DisplayInformationConsoleMessage($"Final Score: {score}");
        StudySessionDto studySession = new StudySessionDto(stack.StackID, score);

        try
        {
            _studySessionDao.InsertNewStudySession(studySession);
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error saving study session.", ex.Message);
        }

        _inputHandler.PauseForContinueInput();
    }

    private void HandleViewPreviousStudySessionsSelection()
    {
        AnsiConsole.Clear();
        int year = _inputHandler.PromptForPositiveInteger("Please enter a year to view study sessions for:");
        var data = _studySessionDao.GetStudySessionReportData(year);
        if (data == null || !data.Any())
        {
            AnsiConsole.WriteLine("No data available for the selected year.");
            _inputHandler.PauseForContinueInput();
            return;
        }

        DisplayStudySessionReport(data);
    }

    private void DisplayStudySessionReport(IEnumerable<dynamic> data)
    {
        AnsiConsole.Clear();
        Utilities.DisplayPageHeader("Study Session Report");
        var table = new Table();
        table.AddColumn("Stack Name");
        table.AddColumn("Jan");
        table.AddColumn("Feb");
        table.AddColumn("Mar");
        table.AddColumn("Apr");
        table.AddColumn("May");
        table.AddColumn("Jun");
        table.AddColumn("Jul");
        table.AddColumn("Aug");
        table.AddColumn("Sep");
        table.AddColumn("Oct");
        table.AddColumn("Nov");
        table.AddColumn("Dec");

        foreach (var row in data)
        {
            string stackName = row.StackName;
            string jan = row.Jan?.ToString() ?? "0";
            string feb = row.Feb?.ToString() ?? "0";
            string mar = row.Mar?.ToString() ?? "0";
            string apr = row.Apr?.ToString() ?? "0";
            string may = row.May?.ToString() ?? "0";
            string jun = row.Jun?.ToString() ?? "0";
            string jul = row.Jul?.ToString() ?? "0";
            string aug = row.Aug?.ToString() ?? "0";
            string sep = row.Sep?.ToString() ?? "0";
            string oct = row.Oct?.ToString() ?? "0";
            string nov = row.Nov?.ToString() ?? "0";
            string dec = row.Dec?.ToString() ?? "0";

            table.AddRow(
                stackName,
                jan,
                feb,
                mar,
                apr,
                may,
                jun,
                jul,
                aug,
                sep,
                oct,
                nov,
                dec
            );
        }

        AnsiConsole.Write(table);
        Utilities.PrintNewLines(2);
        _inputHandler.PauseForContinueInput();
    }
}
