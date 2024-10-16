using Flashcards.AnaClos.DTOs;
using Flashcards.AnaClos.Models;
using System.Globalization;

namespace Flashcards.AnaClos.Controllers;

public class StudySessionController
{
    private ConsoleController _consoleController;
    private DataBaseController _dataBaseController;
    private StackController _stackController;
    private FlashCardController _flashCardController;

    public StudySessionController(ConsoleController consoleController, DataBaseController dataBaseController, StackController stackController, FlashCardController flashCardController)
    {
        _consoleController = consoleController;
        _dataBaseController = dataBaseController;
        _stackController = stackController;
        _flashCardController = flashCardController;
    }

    public void AddStudySession()
    {
        string title = "Select a stack to study.";
        string returnOption = "Return to Main";
        var stackName =_stackController.ShowStacks(title,returnOption);
        if(stackName == returnOption)
        {
            return;
        }
        int score = 0;
        var stack = _stackController.GetStackByName(stackName);

        var flashcards = _flashCardController.GetFlashCardsDTO(stackName);
        if (flashcards.Count==0)
        {
            _consoleController.MessageAndPressKey("There is no FlashCard in this Stack", "red bold");
            return;
        }
        foreach( var flashCard in flashcards) 
        {
            StackStudy(flashCard, ref score);
        }
        var studySession = new StudySession { Date = DateTime.Now, StackId = stack.Id, Score = score };
        var sqlInsert = "INSERT INTO StudySessions (Date, StackId, Score) VALUES (@Date, @StackId, @Score)";
        try
        {
            int rows = _dataBaseController.Execute<StudySession>(sqlInsert, studySession);
            _consoleController.MessageAndPressKey($"{rows} study session added.", "green");
        }
        catch(Exception ex)
        {
            _consoleController.MessageAndPressKey(ex.Message, "red bold");
        }
    } 

    public void StackStudy(FlashCardDTO flashCard, ref int score)
    {
        string title = "Study Session";
        string returnOption = "Return to Main";
        string[] columnsFront = { "FlashcardId", "Front" };
        string[] columnsBack = { "FlashcardId", "Back" };
        var flashCards = new List<FlashCardDTO> { flashCard};
        var tableRecords =_flashCardController.FrontToTableRecord(flashCards);
        _consoleController.ShowTable(title, columnsFront, tableRecords);
        var back = _consoleController.GetString("Please enter your answer to the above flashcard: ");
        while (back.Trim() == "")
        {
            _consoleController.ShowMessage("Invalid entry, cannot be empty.", "red bold");
            back = _consoleController.GetString("Please enter your answer to the above flashcard: ");
        }
        if (back.Trim().ToLower() == flashCard.Back)
        {
            score++;//falta mostrar tabla
            _consoleController.MessageAndPressKey($"Correct! your current score is {score}", "green");
        }
        else
        {
            _consoleController.MessageAndPressKey($"Incorrect!", "red bold");
        }
    }

    public void ViewStudySession() 
    {
        string stackTitle = "Select a stack to view study sessions.";
        string [] columns = { "Session Date", "Score" };
        string returnOption = "Return to Main";
        var stackName = _stackController.ShowStacks(stackTitle, returnOption);
        if (stackName == returnOption)
        {
            return;
        }
        var stack = _stackController.GetStackByName(stackName);
        var studySessions = GetStudySessions(stack.Id);
        var tableRecord = StudyToTableRecord(studySessions);
        int totalScore = 0;
        int count = 0;
        foreach( var studySession in studySessions)
        {
            totalScore += studySession.Score;
            count++;
        }
        if(count>0)
        {
            string tableTitle = $"{stackName} Study Sessions";
            _consoleController.ShowTable(tableTitle, columns, tableRecord);
            _consoleController.MessageAndPressKey($"Average study session score: {totalScore / count}", "green");
        }
        else
        {
            _consoleController.MessageAndPressKey($"No study session for this stack", "red bold");
        }

    }

    public List<TableRecordDTO> StudyToTableRecord(List<StudySession> studySessions)
    {
        var tableRecord = new List<TableRecordDTO>();
        foreach (var studySession in studySessions)
        {
            var record = new TableRecordDTO { Column1 = studySession.Date.ToString(new CultureInfo("en-us")), Column2 = studySession.Score.ToString() };
            tableRecord.Add(record);
        }
        return tableRecord;
    }

    public List<StudySession> GetStudySessions(int stackId)
    {
        var studySessions = new List<StudySession>();
        var sql = $"SELECT * FROM StudySessions WHERE StackId ={stackId}";
        try
        {
            studySessions = _dataBaseController.Query<StudySession>(sql);
        }
        catch (Exception ex)
        {
            _consoleController.MessageAndPressKey(ex.Message, "red bold");
        }
        return studySessions;
    }
}