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
        var stacks = _stackDao.GetAllStacks();
        if (stacks == null || !stacks.Any())
        {
            Utilities.DisplayWarningMessage("No stacks found.");
            _inputHandler.PauseForContinueInput();
            return;
        }

        StackDto selectedStack = _inputHandler.PromptForSelectionListStacks(stacks, "Select a stack to study:");
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
}
