using DataAccess;
using DataAccess.Models;
using Flashcards.SamGannon.DTOs;
using Flashcards.SamGannon.Utilities;

namespace Flashcards.SamGannon.UI;

public class StudySession
{
    private DateTime startTime;
    private DateTime endTime;
    private double score;
    private List<FlashcardDto> flashcardList;
    private readonly IDataAccess _dataAccess;
    private IMenu _MainMenu;

    public StudySession(IMenu mainMenu)
    {
        _MainMenu = mainMenu;
    }

    public void StartStudySession()
    {
        Console.Clear();
        Console.WriteLine("=== Start Study Session ===");

        ConsoleHelper.Map(_dataAccess, "Study Session");

        Console.WriteLine("Please select the stack you wish to study by name");
        var stackName = ConsoleHelper.ValidateStackName(_dataAccess);
        int stackId = GetStackId(stackName);

        List<Flashcard> rawFlashcards = _dataAccess.GetFlashcardsByStackId(stackId);
        flashcardList = FlashcardDto.ToDto(rawFlashcards);
        
        Study();
        score = CalculateScore(flashcardList, score);

        Console.WriteLine("Press any key to end the study session.");
        Console.ReadKey();

        InsertStudySessionIntoDatabase(stackName, score, startTime, endTime);

        _MainMenu.ShowMenu();
    }

    private double CalculateScore(List<FlashcardDto> flashcards, double currentScore)
    {
        if (flashcards == null || flashcards.Count == 0)
        {
            return 0.0;
        }

        double percentage = currentScore / flashcards.Count * 100;

        return percentage;
    }

    private void Study()
    {
        if (flashcardList.Count > 0)
        {
            bool isStudying = true;

            while (isStudying)
            {
                StartTimer();
                foreach (var card in flashcardList)
                {
                    Console.WriteLine($"{card.Question}");
                    Console.WriteLine("Type your answer:");

                    string answer = Console.ReadLine();
                    CheckAnswer(card, answer);
                }

                StopTimer();
                isStudying = false;
                Console.WriteLine($"Study session is over: you got {score} out of {flashcardList.Count}");
            }
        }
        else
        {
            Console.WriteLine("No cards to study, please add cards by going back to the main menu and following the on screen prompts.");
            Console.WriteLine("Press a key to continue.");
            Console.ReadLine();
            _MainMenu.ShowMenu();
        }
    }

    private void CheckAnswer(FlashcardDto card, string? answer)
    {
        if (answer != null && answer.ToUpper() == card.Question.ToUpper())
        {
            Console.WriteLine("Correct!");
            score += 1;
        }
        else
        {
            Console.WriteLine($"Incorrect! The correct answer is: {card.Answer}");
            Console.WriteLine("Press a key to continue");
            Console.ReadLine();
        }
    }

    private void StartTimer()
    {
        startTime = DateTime.Now;
    }

    private void StopTimer()
    {
        endTime = DateTime.Now;
    }

    private void InsertStudySessionIntoDatabase(string stackName, double score, DateTime startTime, DateTime endTime)
    {
        int stackId = GetStackId(stackName);

        if (stackId > 0)
        {
            _dataAccess.InsertStudySession(stackId, startTime, endTime, score);
            return;
        }
        else
        {
            Console.WriteLine("An unexpected error occured. Terminating the program.");
            Environment.Exit(1);
        }
        
    }

    private int GetStackId(string stackName)
    {
        return _dataAccess.GetStackId(stackName);
    }

}
