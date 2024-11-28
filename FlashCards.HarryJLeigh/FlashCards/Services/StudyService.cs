using FlashCards.Controllers;
using FlashCards.Data;
using FlashCards.Utilities;
using FlashCards.Views;
using Spectre.Console;

namespace FlashCards.Services;

public static class StudyService
{
    private static readonly StudyController _studyController = new StudyController();

    internal static void ViewAll()
    {
        string currentStack = FlashcardService.ChangeCurrentStack();
        int stack_id = FlashcardExtensions.GetStack_id(currentStack);
        List<StudyDto> studySessions = _studyController.GetAllStudySessions(stack_id);

        if (studySessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No study Sessions found![/]");
            Util.AskUserToContinue();
        }
        else
        {
            TableVisualisation.ShowStudySessions(studySessions);
            Util.AskUserToContinue();
        }
    }

    internal static void Start()
    {
        Random random = new Random();
        string formattedDate = DateTime.Now.ToString("yyyy-MM-dd");
        string currentStack = FlashcardService.ChangeCurrentStack();
        List<FlashCardDto> flashcards = FlashcardService.GetAllFlashcards(currentStack);
        int score = 0;
        int flashcardsCount = 0;

        while (true)
        {
            Console.Clear();

            FlashCardDto flashcard = GetRandomFlashcard(flashcards, random);
            TableVisualisation.ShowFlashcardToAnswer(flashcard);

            string answer = StudyExtensions.AskUserForAnswer();
            int result = StudyExtensions.CheckCorrectAnswer(answer, flashcard);

            if (result == -1)
            {
                EndSession(score, flashcardsCount);
                int stack_id = FlashcardExtensions.GetStack_id(currentStack);
                _studyController.InsertStudySession(formattedDate, score, flashcardsCount, stack_id);
                break;
            }
            score += result;
            flashcardsCount++;
        }
    }

    internal static  void DeleteAllStudySessions(int stack_id) => _studyController.DeleteAll(stack_id);
    
    private static FlashCardDto GetRandomFlashcard(List<FlashCardDto> flashcards, Random random)
    {
        int randomIndex = random.Next(flashcards.Count);
        return flashcards[randomIndex];
    }

    private static void EndSession(int score, int flashcardsCount)
    {
        StudyExtensions.ExitSessionReport(score, flashcardsCount);
    }
}