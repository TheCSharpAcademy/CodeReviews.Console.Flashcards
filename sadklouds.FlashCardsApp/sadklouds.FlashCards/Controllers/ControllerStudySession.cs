using ConsoleTableExt;
using FlashCardsLibrary;
using FlashCardsLibrary.DTOs;
using FlashCardsLibrary.Models;
using FlashCardsLibrary.Tools;
using sadklouds.FlashCards.Helpers;

namespace sadklouds.FlashCards.Controllers
{
    public class ControllerStudySession
    {
        private readonly SQLDataAccess db = new();

        public void Study()
        {
            string stackName = UserInputHelper.GetUserStringInput("\nWhat stack do you wish to study: ");
            var flashCards = db.LoadFlashCards(stackName);
            if (flashCards.Count == 0)
            {
                Console.WriteLine($"There are no flash cards found in the stack name {stackName}stack");
                return;
            }
            StudySession(stackName, flashCards);
        }
        public void StudySession(string stackName, List<FlashCardModel> flashCards)
        {
            List<FlashCardDTO> flashCardsDTO = CreateDTOHelper.CreateFlashCardDTO(flashCards);
            int score = 0;
            foreach (var card in flashCardsDTO)
            {
                Console.WriteLine();
                Console.WriteLine(card.Front);
                string userAnwser = UserInputHelper.GetUserStringInput("\nInput your anwser to this card: ");
                if (userAnwser == card.Back)
                {
                    Console.WriteLine("Anwser was correct");
                    score++;
                }
                else
                {
                    Console.WriteLine("\nAnwser was incorrect");
                    Console.WriteLine($"\nCorrect anwser was {card.Back}");
                }
            }
            Console.WriteLine($"\nstudy sesison finished score was {score}/{flashCardsDTO.Count}");
            db.CreateStudySession(stackName, DateTime.Now, score);
        }

        public void ViewStudySessions()
        {
            var studySessions = db.GetAllStudySessions();
            if (studySessions.Count != 0)
            {
                var studySessionsDTO = CreateDTOHelper.CreateStudySessionDTO(studySessions);
                ConsoleTableBuilder
                    .From(studySessionsDTO)
                    .WithTitle("Study Sessions", ConsoleColor.Yellow, ConsoleColor.DarkGray)
                    .WithColumn("Id", "Date", "Score", "Stack")
                    .ExportAndWriteLine();
            }
            else
            {
                Console.WriteLine("No study sessions were found");
            }

        }
    }
}
