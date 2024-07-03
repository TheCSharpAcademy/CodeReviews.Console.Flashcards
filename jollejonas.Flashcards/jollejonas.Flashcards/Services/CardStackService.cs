using jollejonas.Flashcards.Data;
using jollejonas.Flashcards.Models;
using jollejonas.Flashcards.DTOs;
using Spectre.Console;


namespace jollejonas.Flashcards.Services
{
    public class CardStackService(DatabaseManager databaseManager)
    {
        private readonly DatabaseManager _databaseManager = databaseManager;

        public CardStack DisplayAndSelectCardStacks()
        {
            var cardStacks = _databaseManager.GetCardStacks();

            var menuSelection = AnsiConsole.Prompt(
                new SelectionPrompt<CardStack>()
                .Title("Select an option")
                .PageSize(10)
                .AddChoices(cardStacks)
                .UseConverter(option => option.Name));

            return menuSelection;
        }

        public void DisplayCardStacks()
        {
            var cardStacks = _databaseManager.GetCardStacks();

            foreach (var stack in cardStacks)
            {
                Console.WriteLine($"ID: {stack.Id}, Name: {stack.Name}, Description: {stack.Description}");
            }
        }

        public void GetSelectedCardStack()
        {
            var selectedStackId = DisplayAndSelectCardStacks().Id;
            var selectedStack = _databaseManager.GetCardStackDTOs(selectedStackId);

            Console.WriteLine($"Selected stack: {selectedStack.CardStackName}");

            foreach(var card in selectedStack.Cards)
            {
                Console.WriteLine($"Question: {card.Question}, Answer: {card.Answer}");
            }
            Console.ReadKey();
        }

        public void CreateStack()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Existing stacks: ");
                DisplayCardStacks();
                Console.WriteLine("Enter the name of the stack:");
                string name = Console.ReadLine();

                Console.WriteLine("Enter the description of the stack:");
                string description = Console.ReadLine();

                var query = $@"
                INSERT INTO Stacks (Name, Description)
                VALUES ('{name}', '{description}')
            ";

                _databaseManager.ExecuteNonQuery(query);

                Console.WriteLine("Stack created successfully!");
                Console.WriteLine("Press 1 to add another stack or 0 to return to menu");

                if (Console.ReadLine() == "0")
                {
                    break;
                }
            }
        }

        public void EditStack()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Select the stack ID:");
                var cardStack = DisplayAndSelectCardStacks();

                Console.WriteLine("Enter the new name of the stack:");
                string name = Console.ReadLine();

                Console.WriteLine("Enter the new description of the stack:");
                string description = Console.ReadLine();

                var query = $@"
                UPDATE Stacks
                SET Name = '{name}', Description = '{description}'
                WHERE Id = {cardStack.Id}
            ";


                Console.WriteLine("Old stack: ");
                Console.WriteLine($"Stack name: {cardStack.Name} - Stack description: {cardStack.Description}\n");

                Console.WriteLine("New stack: ");
                Console.WriteLine($"Stack name: {name} - Stack description: {description}\n");

                if (!Confirmations("update"))
                {
                    break;
                }

                _databaseManager.ExecuteNonQuery(query);

                Console.WriteLine("Stack updated successfully!");
                Console.WriteLine("Press 1 to edit another stack or 0 to return to menu");

                if (Console.ReadLine() == "0")
                {
                    break;
                }
            }
        }

        public void DeleteStack()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Select the stack you want to delete:");
                int stackId = DisplayAndSelectCardStacks().Id;

                var query = $@"
                DELETE FROM Stacks
                WHERE Id = {stackId}
            ";

                if (!Confirmations("delete"))
                {
                    break;
                }
                _databaseManager.ExecuteNonQuery(query);

                Console.WriteLine("Stack deleted successfully!");
                Console.WriteLine("Press 1 to delete another stack or 0 to return to menu");

                if (Console.ReadLine() == "0")
                {
                    break;
                }
            }
        }

        public bool Confirmations(string operation)
        {
            Console.WriteLine($"Are you sure you want to {operation} this stack?");
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
