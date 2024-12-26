using FlashCards.FlashCardsManager.Models;
using FlashCards.Database;
using Spectre.Console;

namespace FlashCards.FlashCardsManager.Controllers
{
    internal class StacksController
    {
        internal  Stacks GetStack(DataTools dataTools)
        {
            List<Stacks> stacks = dataTools.GetStacks();
            return AnsiConsole.Prompt(new SelectionPrompt<Stacks>()
                .AddChoices(new Stacks { Name = "[red]Cancel[/]", NumberOfCards = 0 })
                .AddChoices(stacks)
                .UseConverter(x => $"Name: {x.Name} - Number Of Cards: {x.NumberOfCards}"));
        }

        internal  List<Stacks> GetAllStacks(DataTools dataTools)
        {
            return dataTools.GetStacks();
        }

        internal  void AddNewStack(DataTools dataTools)
        {
            string name = UserInputs.GetInputString();
            int numberofCards = 0;
            dataTools.AddNewStack(new Stacks { Name=name,NumberOfCards=numberofCards});
        }

        internal  void AddNewCard(string stack,DataTools dataTools)
        {
            FlashCard newCard = new();
            newCard.Question = AnsiConsole.Ask<string>("Enter a Question: \n\r");
            newCard.Answer = AnsiConsole.Ask<string>($"Enter a Response to '{newCard.Question}': \n\r");
            dataTools.AddCard(newCard,stack);
        }

        internal  List<FlashCard> RandomizeCards(Stacks stack)
        {
            Random random = new();
            int a = random.Next(0, stack.NumberOfCards);
            int b =  random.Next(0, stack.NumberOfCards);
            FlashCard tempoCard;
            int shuffles = stack.NumberOfCards * 2;
            for (int i = 0; i < shuffles; i++)
            {
                tempoCard = stack.FlashCards[a];
                stack.FlashCards[a] = stack.FlashCards[b];
                stack.FlashCards[b] = tempoCard;
                a = random.Next(0, stack.NumberOfCards);
                b = random.Next(0, stack.NumberOfCards);
            }
            return stack.FlashCards;
        }

        internal  void CreateTestStacks(DataTools dataTools)
        {
            Random random = new();
            for (int i = 0; i < 5; i++)
            {
                Stacks stack = new();
                stack.Name = $"testStack_{i}";
                stack.NumberOfCards = random.Next(40, 61);
                dataTools.AddNewStack(stack);
                for(int j = 0; j < stack.NumberOfCards; j++)
                {
                    dataTools.AddCard(new FlashCard { Question = $"Question_{j}", Answer = $"Answer_{j}" },stack.Name);
                }
            }
        }

        internal  void PrintStacks(DataTools dataTools)
        {
            List<Stacks> stacks = dataTools.GetStacks();
            Spectre.Console.Table table;
            foreach (Stacks stack in stacks)
            {
                table = new();
                table.Expand();
                table.Centered();
                table.Title($"Name: {stack.Name} - Number Of Cards: {stack.NumberOfCards}");
                table.AddColumns("Id", "Question", "Answer");
                stack.FlashCards = dataTools.GetFlashCards(stack.Name);
                foreach (FlashCard card in stack.FlashCards) { table.AddRow(card.Id.ToString(), card.Question, card.Answer); }
                AnsiConsole.Write(table);
            }
            
            AnsiConsole.WriteLine();
            table = new();
            table.Centered();
            table.Border(TableBorder.DoubleEdge);
            table.Title("Resume");
            table.AddColumns("ID", "NAME", "NUMBER OF CARDS");
            foreach (Stacks stack in stacks) table.AddRow($"{stack.Id}", $"{stack.Name}", $"{stack.NumberOfCards}");
            AnsiConsole.Write(table);
        }
    }
}
