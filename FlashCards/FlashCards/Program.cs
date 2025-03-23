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
            FlashCardApp app = new FlashCardApp(cardStackRepository,flashCardRepository,UI);
            app.Run();

            /*var allCards = flashCardRepository.GetAllRecords().ToList();

            foreach (var card in allCards) 
            {
                Console.WriteLine($"CardID: {card.CardID} | Front: {card.FrontText} | Back: {card.BackText}");
            }*/
        }   
    }
}
