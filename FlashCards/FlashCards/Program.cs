using FlashCards.Models;
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
            UserInterface UI = new UserInterface();

            CardStackRepositoryService cardStackService = new CardStackRepositoryService(cardStackRepository, UI);
            FlashCardRepositoryService flashCardService = new FlashCardRepositoryService(flashCardRepository, cardStackRepository, UI);

            string pathToDefaultData = @"E:\Git Repos\CodeReviews.Console.Flashcards\FlashCards\FlashCards\DefaultDataForAutoFill.json";
            FlashCardApp app = new FlashCardApp(cardStackService, flashCardService, UI, pathToDefaultData);
            app.Run();


        }   
    }

}
