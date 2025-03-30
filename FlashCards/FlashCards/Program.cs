using FlashCards.Models;
using Spectre.Console;
using System.Globalization;
using System.Text.Json;
using static System.Formats.Asn1.AsnWriter;

namespace FlashCards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            string connectionString = @"Data Source=(localdb)\LOCALDB;Initial Catalog=FlashCardsProject;Integrated Security=True;Connect Timeout=5;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            CardStackRepository cardStackRepository = new CardStackRepository(connectionString);
            FlashCardRepository flashCardRepository = new FlashCardRepository(connectionString);
            StudySessionRepository studySessionRepository = new StudySessionRepository(connectionString);

            UserInterface UI = new UserInterface();

            CardStackRepositoryService cardStackService = new CardStackRepositoryService(cardStackRepository, UI);
            FlashCardRepositoryService flashCardService = new FlashCardRepositoryService(flashCardRepository, cardStackRepository, UI);
            StudySessionRepositoryService studySessionService = new StudySessionRepositoryService(studySessionRepository);


            string pathToDefaultData = @"E:\Git Repos\CodeReviews.Console.Flashcards\FlashCards\FlashCards\DefaultDataForAutoFill.json";
            FlashCardApp app = new FlashCardApp(cardStackService, flashCardService, studySessionService,UI, pathToDefaultData);
            app.Run();


            StudySession session = new StudySession()
            {
                StackName = "Czech",
                StackId = 1,
                SessionDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Score = 15,
            };

            PrintResult(session, 3);
        }

        public static void PrintResult(StudySession session, int numberOfRounds)
        {
            var table = new Table();
            table.AddColumns("1", "2");
            table.HideHeaders();
            table.AddRow("Date", session.SessionDate.ToString("yyyy-MM-dd"));
            table.AddRow("Stack", session.StackName);
            table.AddRow("Rounds Played", numberOfRounds.ToString());
            table.AddRow("Score", session.Score.ToString());

            AnsiConsole.Write(table);

            Console.WriteLine("\nPress any key to continue or ESC to exit");
            Console.ReadKey();
        }
    }

}
