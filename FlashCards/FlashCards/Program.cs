using FlashCards.Models;
using System.Globalization;
using System.Text.Json;

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

            var defaultData = JsonSerializer.Deserialize<DefaultDataObject>(File.ReadAllText(pathToDefaultData));

            /*string isoDate = "1994-08-23";
            DateTime dateTime = DateTime.Parse(isoDate);

            Console.WriteLine(dateTime.ToString())0;*/
        }   
    }

}
