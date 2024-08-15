using jollejonas.Flashcards.Data;
using jollejonas.Flashcards.DTOs;
using jollejonas.Flashcards.Models;
using jollejonas.Flashcards.UserInputs;
using jollejonas.Flashcards.Validation;
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
            Console.Clear();
            while (true)
            {
                Console.WriteLine($"Selected stack: {cardStack.Name}\n");
                string question = UserInput.GetStringInput("Enter the question:");

                string answer = UserInput.GetStringInput("Enter the answer:");

                var query = $@"
                    INSERT INTO Cards (Question, Answer, StackId)
                    VALUES ('{question}', '{answer}', {cardStack.Id})
                ";

                _databaseManager.ExecuteNonQuery(query);

                Console.WriteLine("Card created successfully!");
                Console.WriteLine("Do you want to add another card?(y/n)");

                if (Console.ReadLine() == "n")
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
                string question = UserInput.GetStringInput("Enter the new question:");

                Console.WriteLine($"Current answer: {card.Answer}");
                string answer = UserInput.GetStringInput("Enter the new answer:");

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
                Console.WriteLine("Do you want to edit another card?(y/n)");

                if (Console.ReadLine() == "n")
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
                Console.WriteLine("Do you want to delete another card?(y/n)");

                if (Console.ReadLine() == "n")
                {
                    break;
                }
            }
        }
        public CardsDto DisplayCardsAndSelectId(int stackId)
        {
            var cards = _databaseManager.GetCardStackDTOs(stackId).Cards;

            var menuSelection = AnsiConsole.Prompt(
                new SelectionPrompt<CardsDto>()
                .Title("Select a card")
                .PageSize(10)
                .AddChoices(cards)
                .UseConverter(card => $"ID: {card.Id}, Question: {card.Question}, Answer: {card.Answer}"));
            
            return menuSelection;
        }

        public bool Confirmations(string operation)
        {
            Console.WriteLine($"Are you sure you want to {operation} this card?");

            var menuSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select an option")
                .PageSize(10)
                .AddChoices(new[] { "Yes", "No" })
                .UseConverter(option => option));

            if (menuSelection == "1")
            {
                return true;
            }
            return false;
        }
    }
}
