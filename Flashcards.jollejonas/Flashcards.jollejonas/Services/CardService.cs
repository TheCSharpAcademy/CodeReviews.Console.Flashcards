using Flashcards.jollejonas.Data;
using Flashcards.jollejonas.DTOs;
using Flashcards.jollejonas.Models;
using Flashcards.jollejonas.UserInputs;
using Spectre.Console;

namespace Flashcards.jollejonas.Services
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

                _databaseManager.CreateCard(question, answer, cardStack.Id);

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
            while (true)
            {
                Console.WriteLine("Select the card stack:");
                var cardStackService = new CardStackService(_databaseManager);
                CardStack cardStack = cardStackService.DisplayAndSelectCardStacks();

                if (cardStack == null)
                {
                    Console.ReadKey();
                    break;
                }

                int stackId = cardStack.Id;
                var card = DisplayCardsAndSelectId(stackId);

                Console.WriteLine($"Current question: {card.Question} \n");
                string question = UserInput.GetStringInput("Enter the new question(Leave blank if you don't want to edit):");

                if (question == null)
                {
                    question = card.Question;
                }

                Console.WriteLine($"Current answer: {card.Answer} \n");
                string answer = UserInput.GetStringInput("Enter the new answer(Leave blank if you don't want to edit):");

                if (answer == null)
                {
                    answer = card.Answer;
                }

                Console.WriteLine("Old card: ");
                Console.WriteLine($"Question: {card.Question} - Answer: {card.Answer}\n");

                Console.WriteLine("New card:: ");
                Console.WriteLine($"Question: {question} - Answer: {answer}\n");

                if (!Confirmations("update"))
                {
                    break;
                }

                _databaseManager.UpdateCard(question, answer, card.Id);

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
            while (true)
            {
                Console.WriteLine("Select the card stack:");
                var cardStackService = new CardStackService(_databaseManager);
                CardStack cardStack = cardStackService.DisplayAndSelectCardStacks();

                if (cardStack == null)
                {
                    Console.ReadKey();
                    break;
                }

                int stackId = cardStack.Id;

                Console.WriteLine("Select the card ID:");
                var card = DisplayCardsAndSelectId(stackId);
                if (card == null)
                {
                    Console.ReadKey();
                    break;
                }
                Console.WriteLine($"You selected this card: ");
                Console.WriteLine($"Question: {card.Question} - Answer: {card.Answer}");

                if (!Confirmations("delete"))
                {
                    break;
                }

                _databaseManager.DeleteCard(card.Id);

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
            CardStackDetailsDto cardStack = _databaseManager.GetCardStackDTOs(stackId);
            if (cardStack == null)
            {
                Console.ReadKey();
                return null;
            }

            var cards = cardStack.Cards;

            var menuSelection = AnsiConsole.Prompt(
                new SelectionPrompt<CardsDto>()
                .Title("Select a card")
                .PageSize(10)
                .AddChoices(cards)
                .UseConverter(card => $"ID: {card.PresentationId}, Question: {card.Question}, Answer: {card.Answer}"));

            return menuSelection;
        }

        public bool Confirmations(string operation)
        {
            var menuSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title($"Are you sure you want to {operation} this card?")
                .PageSize(10)
                .AddChoices(new[] { "Yes", "No" })
                .UseConverter(option => option));

            if (menuSelection == "Yes")
            {
                return true;
            }
            Console.WriteLine("Cancelled.");
            return false;
        }
    }
}
