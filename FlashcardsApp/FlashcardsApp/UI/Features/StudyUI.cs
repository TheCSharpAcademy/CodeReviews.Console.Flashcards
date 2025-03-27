using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsApp.Models;
using FlashcardsApp.Services;
using FlashcardsApp.UI.Core;
using Spectre.Console;

namespace FlashcardsApp.UI.Features
{
    internal class StudyUI
    {
        private readonly DatabaseService _databaseService;
        private readonly InputValidator _inputValidator;

        internal StudyUI(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _inputValidator = new InputValidator();
        }

        internal void StudyMain()
        {
            bool continueStudy = true;
            while (continueStudy)
            {
                if (!GetStackToStudy(out int stackId))
                {
                    return;
                }

                if (RunStudySession(stackId, out string finalScore))
                {
                    AddStudySession(stackId, finalScore);
                }

                continueStudy = _inputValidator.GetConfirmation("Would you like to study a stack again?");
            }
        }

        private bool GetStackToStudy(out int stackId)
        {
            stackId = -1;
            Console.Clear();
            _databaseService.GetAllStacks();

            List<Stack> stacks = _databaseService.GetAllStacksAsList();
            if (!stacks.Any())
            {
                Console.WriteLine("\nNo stacks found. Create a stack first!");
                Console.WriteLine("\nPress Enter to return to main menu...");
                Console.ReadLine();
                return false;
            }

            Dictionary<string, int> stackMapping = stacks.ToDictionary(
                s => $"Name: {s.Name} | {s.Description}",
                s => s.StackId);

            var choices = stackMapping.Keys.ToList();
            choices.Add("Return to Main Menu");

            var selectedStack = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a stack to study.")
                .AddChoices(choices));

            if (selectedStack == "Return to Main Menu")
                return false;

            stackId = stackMapping[selectedStack];
            return true;
        }

        private bool RunStudySession(int stackId, out string finalScore)
        {
            finalScore = "";
            List<Flashcard> flashcards = _databaseService.GetFlashcardsByStackID(stackId);
            if (!flashcards.Any())
            {
                Console.WriteLine("\nNo flashcards in this stack!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return false;
            }

            var studyData = PrepareStudyData(flashcards);
            int score = StudyFlashcards(studyData);

            finalScore = $"{score}/{studyData.Count}";
            Console.WriteLine($"\nYou got a score of {finalScore}");
            return true;
        }

        private Dictionary<string, string> PrepareStudyData(List<Flashcard> flashcards)
        {
            return flashcards.ToDictionary(
                f => $"Front: {f.Front}",
                f => $"{f.Back}");
        }

        private int StudyFlashcards(Dictionary<string, string> cardMapping)
        {
            int score = 0;
            var flashcardFront = cardMapping.Keys.ToList();

            Console.WriteLine($"\nPress Enter to Start");
            Console.ReadLine();

            foreach (var front in flashcardFront)
            {
                score += ProcessFlashcard(front, cardMapping[front]);
            }

            return score;
        }

        private int ProcessFlashcard(string front, string correctAnswer)
        {
            Console.Clear();
            Console.WriteLine(front);
            Console.Write("\nAnswer: ");
            string? answer = Console.ReadLine()?.Trim().ToLower();

            bool isCorrect = answer == correctAnswer.ToLower();
            ShowAnswerFeedback(answer, correctAnswer, isCorrect);

            return isCorrect ? 1 : 0;
        }

        private void ShowAnswerFeedback(string userAnswer, string correctAnswer, bool isCorrect)
        {
            Console.WriteLine($"\nAnswer = {correctAnswer.ToLower()}\n" +
                $"Your Answer: {userAnswer}\n");

            AnsiConsole.MarkupLine(isCorrect ? "[green]Correct![/]" : "[red]Incorrect![/]");
           
            Console.WriteLine("\nPress enter to Continue");
            Console.ReadLine();
        }

        private void AddStudySession(int stackId, string finalScore)
        {
            StudySession session = new StudySession();

            session.StackId = stackId;
            session.Score = finalScore;
            session.StudyDate = DateTime.Now;

            _databaseService.PostStudySession(session);
        }
    }
}
