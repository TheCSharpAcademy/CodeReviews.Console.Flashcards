using jollejonas.Flashcards.Data;
using jollejonas.Flashcards.DTOs;
using jollejonas.Flashcards.Models;
using jollejonas.Flashcards.Services;
using Spectre.Console;

namespace jollejonas.Flashcards.Services
{
    public class CardService(DatabaseManager databaseManager)
    {
        private readonly DatabaseManager _databaseManager = databaseManager;

        public void CreateCard()
        {
            var cardStackService = new CardStackService(_databaseManager);
            Console.WriteLine("Select the stack name:");
            var cardStack = cardStackService.DisplayAndSelectCardStacks();
            int stackId = cardStack.Id;
            Console.Clear();
            while (true)
            {
                Console.WriteLine($"Selected stack: {cardStack.Name}\n");
                Console.WriteLine("Enter the question:");
                string question = Console.ReadLine();

                Console.WriteLine("Enter the answer:");
                string answer = Console.ReadLine();

                var query = $@"
                    INSERT INTO Cards (Question, Answer, StackId)
                    VALUES ('{question}', '{answer}', {stackId})
                ";

                _databaseManager.ExecuteNonQuery(query);

                Console.WriteLine("Card created successfully!");
                Console.WriteLine("Press 1 to add another card to stack or 0 to return to menu");

                if (Console.ReadLine() == "0")
                {
                    break;
                }
                Console.Clear();
            }
        }

        public void EditCard()
        {
            Console.Clear();
            while(true)
            {
                Console.WriteLine("Select the card stack:");
                var cardStackService = new CardStackService(_databaseManager);
                int stackId = cardStackService.DisplayAndSelectCardStacks().Id;

                Console.WriteLine("Select the card ID:");
                var card = DisplayCardsAndSelectId(stackId);

                Console.WriteLine($"Current question: {card.Question}");
                Console.WriteLine("Enter the new question:");
                string question = Console.ReadLine();

                Console.WriteLine($"Current answer: {card.Answer}");
                Console.WriteLine("Enter the new answer:");
                string answer = Console.ReadLine();

                var query = $@"
                    UPDATE Cards
                    SET Question = '{question}', Answer = '{answer}'
                    WHERE Id = {card.Id}
                ";

                Console.WriteLine("Old card: ");
                Console.WriteLine($"Question: {card.Question} - Answer: {card.Answer}\n");

                Console.WriteLine("New card:: ");
                Console.WriteLine($"Question: {question} - Answer: {answer}\n");

                if(!Confirmations("update"))
                {
                    break;
                }
                _databaseManager.ExecuteNonQuery(query);

                Console.WriteLine("Card updated successfully!");
                Console.WriteLine("Press 1 to edit another card or 0 to return to menu");

                if (Console.ReadLine() == "0")
                {
                    break;
                }
            }
        }
        public void DeleteCard()
        {
            Console.Clear();
            while(true)
            {
                Console.WriteLine("Select the card stack:");
                var cardStackService = new CardStackService(_databaseManager);
                int stackId = cardStackService.DisplayAndSelectCardStacks().Id;

                Console.WriteLine("Select the card ID:");
                var card = DisplayCardsAndSelectId(stackId);


                var query = $@"
                    DELETE FROM Cards
                    WHERE Id = {card.Id}
                ";
                Console.WriteLine($"You selected this card: ");
                Console.WriteLine($"Question: {card.Question} - Answer: {card.Answer}");

                if(!Confirmations("delete"))
                {
                    break;
                }

                _databaseManager.ExecuteNonQuery(query);

                Console.WriteLine("Card deleted successfully!");
                Console.WriteLine("Press 1 to delete another card or 0 to return to menu");

                if (Console.ReadLine() == "0")
                {
                    break;
                }
            }
        }
        public CardsDTO DisplayCardsAndSelectId(int stackId)
        {
            var cards = _databaseManager.GetCardStackDTOs(stackId).Cards;

            var menuSelection = AnsiConsole.Prompt(
                new SelectionPrompt<CardsDTO>()
                .Title("Select a card")
                .PageSize(10)
                .AddChoices(cards)
                .UseConverter(card => $"ID: {card.Id}, Question: {card.Question}, Answer: {card.Answer}"));
            
            return menuSelection;
        }

        public bool Confirmations(string operation)
        {
            Console.WriteLine($"Are you sure you want to {operation} this card?");
            Console.WriteLine("Press 1 to confirm or 0 to return to menu");

            var menuSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select an option")
                .PageSize(10)
                .AddChoices(new[] { "1", "0" })
                .UseConverter(option => option));

            if (menuSelection == "1")
            {
                return true;
            }
            return false;
        }
    }
}
