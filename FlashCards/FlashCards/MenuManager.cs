using FlashCardsLibrary;
using Spectre.Console;
using FlashCardsLibrary.Models;
namespace FlashCards
{
    internal static class MenuManager
    {
        public static void Greeting()
        {
            Console.Clear();
            AnsiConsole.Write(
                new FigletText("Welcome to FlashCards").Centered().Color(Color.White));
        }
        public static void MainMenu()
        {
            Console.WriteLine("--Main Menu--");
            Console.WriteLine("1- Make Report");
            Console.WriteLine("2- Manage Stacks");
            Console.WriteLine("3- Start StudySession");
            Console.WriteLine("4- Help");
            Console.WriteLine("5- Exit");
        }
        public static void MakeReport()
        {
            Console.Clear();
            Console.WriteLine("--Make Report--");
        }
        public static void ManageStacksMenu()
        {
            Console.Clear();
            Console.WriteLine("--Stacks--");
            Console.WriteLine("Stacks: ");
            var stacks = StackController.GetStackNames();
            for (int i = 0; i < stacks.Count; i++)
            {
                Console.WriteLine($"{i+1}- {stacks[i].Name}");
            }
            Console.WriteLine();
            Console.WriteLine("1- Add New Stack");
            Console.WriteLine("2- Change Stack's Name");
            Console.WriteLine("3- Delete Stack");
            Console.WriteLine("4- Edit Stack's FlashCards");
            
        }
        public static void DisplayFlashCards(List<FlashCardRead> cards)
        {
            Table table = new Table();
            table.AddColumn(new TableColumn("[darkturquoise]ID[/]").Centered());
            table.AddColumn("[darkturquoise]Front[/]");
            table.AddColumn("[darkturquoise]Back[/]");
            table.BorderColor(Color.White);
            foreach (var card in cards) 
            {
                table.AddRow($"[white]{card.ID}[/]",$"[white]{card.Front}[/]",$"[white]{card.Back}[/]");               
            }
            AnsiConsole.Write(table);
        }

        public static List<FlashCardRead> EditFlashCardsMenu(Stack stack)
        {
            Console.Clear();
            Console.WriteLine("--FlashCards--");
            Console.WriteLine($"FlashCards of {stack.Name}: ");
            var cards = FlashCardController.GetFlashCards(stack);
            var display = cards.Select((card,index) => new FlashCardRead(index+1,card.StackName,card.Front,card.Back)).ToList();
            DisplayFlashCards(display);
            Console.WriteLine("1- Add Card");
            Console.WriteLine("2- Edit Card");
            Console.WriteLine("3- Delete Card");
            return cards;
        }
        public static void MakeStudySession()
        {
            Console.Clear();
            Console.WriteLine("--Make Study Session--");
        }
        public static void HelpMenu()
        {
            Console.Clear();
            Console.WriteLine("--Help--");
            Console.WriteLine("1- Make Report => Make a table of your progess (Stacks)");
            Console.WriteLine("2- Manage Stacks => Delete ,Update ,Add a Stack or its FlashCards");
            Console.WriteLine("3- Make Study Session => Make study sessions with stacks to observe progress (quiz) ");
            Console.WriteLine("4- Help => Assist if needed");
            Console.WriteLine("5- Exit => Quits Application Gracefully ");
            EnterPause();
        }
        public static void EnterPause()
        {
            Console.WriteLine("press enter to continue...");
            Console.ReadLine();
        }
    }
}
