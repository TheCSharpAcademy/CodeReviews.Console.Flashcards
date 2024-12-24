using Flashcards.Controllers;
using Flashcards.Database;
using Flashcards.Utilities;
using Flashcards.Views;
using Flashcards.Models;
using Flashcards.Mappers;
using Spectre.Console;

namespace Flashcards.Coordinators;

class AppCoordinator
{
    private readonly MenuHandler _menuHandler;
    private readonly DatabaseManager _databaseManager;
    private readonly StacksControllers _stacksController;
    private readonly ListManager _listManager;
    private readonly StackMapper _stackMapper;
    private readonly FlashcardMapper _flashcardMapper;
    private readonly StudySessionMapper _studySessionMapper;
    private readonly FlashcardsController _flashcardsController;
    private readonly StudySessionController _studyController;

    public AppCoordinator(MenuHandler menuHandler, DatabaseManager databaseManager, StacksControllers stacksController, ListManager listManager, StackMapper stackMapper, FlashcardMapper flashcardMapper, StudySessionMapper studySessionMapper, FlashcardsController flashcardsController, StudySessionController studyController)
    {
        _menuHandler = menuHandler;
        _databaseManager = databaseManager;
        _stacksController = stacksController;
        _listManager = listManager;
        _stackMapper = stackMapper;
        _flashcardMapper = flashcardMapper;
        _studySessionMapper = studySessionMapper;
        _flashcardsController = flashcardsController;
        _studyController = studyController;
    }

    internal void Start()
    {
        _databaseManager.CreateDatabase();
        _databaseManager.CreateStacksTable();
        _databaseManager.CreateFlashcardsTable();
        _databaseManager.CreateStudySessionsTable();

        bool closeApp = false;

        while (!closeApp)
        {
            var userSelection = _menuHandler.ShowMenu();
            switch (userSelection)
            {
                case "viewAllStacks":
                    ViewAllStacks(_databaseManager.GetAllStacksFromDB());
                    break;
                case "createAStack":
                    CreateStack();
                    break;
                case "updateAStack":
                    UpdateAStack();
                    break;
                case "deleteAStack":
                    DeleteAStack();
                    break;
                case "viewAllFlashcards":
                    ViewAllFlashcards(_databaseManager.GetAllFlashcardsFromDB());
                    break;
                case "createAFlashcard":
                    CreateFlashcard();
                    break;
                case "updateAFlashcard":
                    UpdateFlashcard();
                    break;
                case "deleteAFlashcard":
                    DeleteAFlashcard();
                    break;
                case "studySession":
                    StudySession();
                    break;
                case "viewStudySessionData":
                    ViewStudySessionData();
                    break;
                case "viewStudySessionSummary":
                    ViewStudySessionSummary();
                    break;
                case "closeApp":
                    closeApp = true;
                    break;
            }
        }
    }

    internal void ViewAllStacks(List<Stack> stack)
    {
        if (stack.Any())
        {
            _listManager.stacksDTO.Clear();
            var displayCount = 0;

            foreach (var record in stack)
            {
                displayCount++;
                var mappedDto = _stackMapper.MapStackToDTO(record, displayCount);
                _listManager.stacksDTO.Add(mappedDto);
            }
            _listManager.PrintRecords();
            _menuHandler.WaitForUserInput();
        }
        else
        {
            AnsiConsole.WriteLine("No stacks available.");
            _menuHandler.WaitForUserInput();
        }
    }

    internal void CreateStack()
    {
        var stackName = _stacksController.GetStackName("Please enter name for new stack or enter 0 to return to main menu");
        _menuHandler.ReturnToMainMenu(stackName);
        _databaseManager.CreateStack(stackName);
    }

    internal void ViewAllFlashcards(List<FlashcardsModel> flashcard)
    {
        if (flashcard.Any())
        {
            _listManager.flashcardsDTOs.Clear();
            var displayCount = 0;

            foreach (var record in flashcard)
            {
                displayCount++;
                string? stackName = _databaseManager.FindMatchingStackNameRecord(record.StackId);
                var mappedDto = _flashcardMapper.MapFlashcardToDTO(displayCount, record, record, stackName);
                _listManager.flashcardsDTOs.Add(mappedDto);
            }
            _listManager.PrintFlashcardRecords();
            _menuHandler.WaitForUserInput();
        }
        else
        {
            AnsiConsole.WriteLine("No stacks available.");
            _menuHandler.WaitForUserInput();
        }
    }

    internal void DeleteAStack()
    {
        var allStacks = _databaseManager.GetAllStacksFromDB();
        ViewAllStacks(allStacks);
        string? stackName = _stacksController.GetStackName("Please enter a stack name you wish to delete or enter 0 to return to main menu.");
        _menuHandler.ReturnToMainMenu(stackName);
        var recordExist = _listManager.CheckForExistingStackRecord(stackName);
        if (recordExist)
        {
            _databaseManager.DeleteAStackFromDB(stackName);
            AnsiConsole.MarkupLine($"Record with name of {stackName} deleted successfully");
        }
        else
        {
            AnsiConsole.WriteLine("Stack name entered does not exist. Returning to main menu");
            _menuHandler.WaitForUserInput();
        }
    }

    internal void UpdateAStack()
    {
        var allStacks = _databaseManager.GetAllStacksFromDB();
        ViewAllStacks(allStacks);

        string? stackName = _stacksController.GetStackName("Please enter stack name to update, or enter 0 to return to main menu");
        _menuHandler.ReturnToMainMenu(stackName);
        var recordExist = _listManager.CheckForExistingStackRecord(stackName);
        if (recordExist)
        {
            string? updatedStackName = _stacksController.GetStackName("Please enter new name for stack.");

            _databaseManager.UpdateAStackFromDB(stackName, updatedStackName);
        }
        else
        {
            AnsiConsole.WriteLine("Stack name entered does not exist. Returning to main menu.");
            _menuHandler.WaitForUserInput();
        }

    }

    internal void CreateFlashcard()
    {
        var loadedStacks = _databaseManager.GetAllStacksFromDB();
        ViewAllStacks(loadedStacks);

        var stackId = _flashcardsController.GetFlashcardStackId();
        if (stackId == 0) return;

        var front = _flashcardsController.GetFlashcardFront();
        _menuHandler.ReturnToMainMenu(front);
        var back = _flashcardsController.GetFlashcardBack();

        string? matchingStackName;
        foreach (var stack in _listManager.stacksDTO)
        {
            if (stackId == stack.DisplayId)
            {
                matchingStackName = stack.Name;
                int linkedStackId = _databaseManager.FindMatchingRecord(matchingStackName);
                _databaseManager.CreateFlashcard(front, back, linkedStackId);
            }
        }
    }

    internal void UpdateFlashcard()
    {
        var flashcardModel = _databaseManager.GetAllFlashcardsFromDB();
        ViewAllFlashcards(flashcardModel);

        int flashcardId = _flashcardsController.GetFlashcardId();
        if (flashcardId == 0) return;

        var databaseId = flashcardModel[flashcardId - 1].FlashcardId;

        var (front, back) = _flashcardsController.GetUpdatedFlashcardInputs();

        if (string.IsNullOrEmpty(front) && string.IsNullOrEmpty(back))
        {
            AnsiConsole.MarkupLine("No update changes provided, update cancelled, returning to main menu");
            return;
        }

        _databaseManager.UpdateAFlashcardFromDB(front, back, databaseId);
    }

    internal void DeleteAFlashcard()
    {
        var flashcardModel = _databaseManager.GetAllFlashcardsFromDB();
        ViewAllFlashcards(flashcardModel);

        int flashcardId = _flashcardsController.GetFlashcardId();
        if (flashcardId == 0) return;

        var databaseId = flashcardModel[flashcardId - 1].FlashcardId;

        int rowDeleted = _databaseManager.DeleteAFlashcardFromDB(databaseId);
        if (rowDeleted == 0)
        {
            AnsiConsole.MarkupLine($"Record with id of: {flashcardId} does not exist");
        }
        else
        {
            AnsiConsole.MarkupLine($"Record with id of: {flashcardId} deleted successfully");
        }
    }

    internal void StudySession()
    {
        var allStacks = _databaseManager.GetAllStacksFromDB();
        ViewAllStacks(allStacks);

        string? stackName = _stacksController.GetStackName("Please enter a stack name to study that stack of flashcards or enter 0 to return to main menu.");
        _menuHandler.ReturnToMainMenu(stackName);
        if (!IsStackValid(stackName)) return;

        if (!IsFlashcardsValid(stackName)) return;

        var score = InitiateStudySession();

        DateTime dateNow = DateTime.Now;
        var stackId = _databaseManager.FindMatchingRecord(stackName);
        SaveStudySession(dateNow, score, stackId);

        AnsiConsole.WriteLine($"Your total score for this current session is: {score}");
        _menuHandler.WaitForUserInput();
    }

    internal bool IsStackValid(string? stackName)
    {
        var recordExist = _listManager.CheckForExistingStackRecord(stackName);

        if (!recordExist)
        {
            AnsiConsole.WriteLine("Stack name entered does not exist. Returning to main menu");
            _menuHandler.WaitForUserInput();
        }
        return recordExist;
    }

    internal bool IsFlashcardsValid(string? stackName)
    {
        var flashcardsList = _databaseManager.GetStackSpecificFlashcards(stackName);

        if (!_listManager.IsListEmpty(flashcardsList))
        {
            AnsiConsole.WriteLine("Stack contains no flashcards");
            _menuHandler.WaitForUserInput();
            return false;
        }
        LoadFlashcardsToDisplay(flashcardsList);
        return true;
    }

    internal int InitiateStudySession()
    {
        int score = 0;
        foreach (var record in _listManager.flashcardsDTOs)
        {
            AnsiConsole.WriteLine($"Please enter your answer to this flashcard: {record.Front}");
            var flashcardAnswer = _flashcardsController.GetFlashcardAnswer();
            var flashcardBack = record.Back?.ToLower() ?? string.Empty;

            if (flashcardBack == flashcardAnswer)
            {
                AnsiConsole.WriteLine($"Correct! Your answer: {flashcardAnswer} ");
                score++;
                _menuHandler.WaitForUserInput();
            }
            else
            {
                AnsiConsole.WriteLine($"Wrong! The correct answer is: {flashcardBack}, better luck next time.");
                _menuHandler.WaitForUserInput();
            }
        }
        return score;
    }

    internal void SaveStudySession(DateTime date, int score, int stackId)
    {
        _databaseManager.CreateAStudySession(date, score, stackId);
    }

    internal void LoadFlashcardsToDisplay(List<FlashcardsModel> flashcardsList)
    {
        _listManager.flashcardsDTOs.Clear();
        var displayCount = 0;

        foreach (var record in flashcardsList)
        {
            displayCount++;
            string? nameOfStack = _databaseManager.FindMatchingStackNameRecord(record.StackId);
            var mappedDto = _flashcardMapper.MapFlashcardToDTO(displayCount, record, record, nameOfStack);
            _listManager.flashcardsDTOs.Add(mappedDto);
        }
    }

    internal void ViewStudySessionData()
    {
        var allStacks = _databaseManager.GetAllStacksFromDB();
        ViewAllStacks(allStacks);

        string? stackName = _stacksController.GetStackName("Please enter a stack name to view study session data or enter 0 to return to main menu.");
        _menuHandler.ReturnToMainMenu(stackName);

        if (!IsStackValid(stackName)) return;

        var studySessionList = _databaseManager.GetStackSpecificStudySessionData(stackName);

        LoadStudySessionToDisplay(studySessionList);

        _listManager.PrintStudySessionRecords();
        _menuHandler.WaitForUserInput();
    }

    internal void LoadStudySessionToDisplay(List<StudySession> studySessionList)
    {
        _listManager.studySessionsDTO.Clear();

        foreach (var record in studySessionList)
        {
            string? nameOfStack = _databaseManager.FindMatchingStackNameRecord(record.StackId);
            var mappedDto = _studySessionMapper.MapFlashcardToDTO(record, record, nameOfStack);
            _listManager.studySessionsDTO.Add(mappedDto);
        }
    }

    internal void ViewStudySessionSummary()
    {
        int year = _studyController.GetStudySessionYear();
        var studySessionSummaryList = _databaseManager.GetStudySessionSummaryData(year);
        _listManager.PrintStudySessionSummary(studySessionSummaryList, year);
        _menuHandler.WaitForUserInput();
    }
}