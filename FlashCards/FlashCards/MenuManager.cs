using FlashCardsLibrary;
using System.Collections;
namespace FlashCards
{
    internal class MenuManager
    {
        public void MainMenu()
        {
            Console.WriteLine("--Main Menu--");
            Console.WriteLine("1- Make Report");
            Console.WriteLine("2- Manage Stacks");
            Console.WriteLine("3- Make StudySession");
            Console.WriteLine("4- Help");
            Console.WriteLine("5- Exit");
        }
        public void MakeReport()
        {
            Console.WriteLine("--Make Report--");
        }
        public void ManageStacksMenu()
        {
            Console.WriteLine("--Stacks--");
            Console.WriteLine("Stacks: ");
            var stacks = StackController.GetStackNames();
            for (int i = 0; i < stacks.Count; i++)
            {
                Console.WriteLine($"{i+1}- {stacks[i]}");
            }
            Console.WriteLine("1- Add New Stack");
            Console.WriteLine("2- Change Stack's Name");
            Console.WriteLine("3- Delete Stack");
            Console.WriteLine("4- Edit Stack's FlashCards");
            
        }
        public void EditFlashCardsMenu(Stack stack)
        {
            Console.WriteLine("--FlashCards");
            Console.WriteLine("FlashCards: ");
            //TODO: DisplayFlashCards(stack)
            Console.WriteLine("1- Add Card");
            Console.WriteLine("2- Edit Card");
            Console.WriteLine("3- Delete Card");
        }
        public void MakeStudySession()
        {
            Console.WriteLine("--Make Study Session--");
        }
        public void HelpMenu()
        {
            Console.WriteLine("--Help--");
            Console.WriteLine("1- Make Report => Make a table of your progess (Stacks and StudySessions)");
            Console.WriteLine("2- Manage Stacks => Delete ,Update ,Add a Stack or its FlashCards");
            Console.WriteLine("3- Make Session =>");
            Console.WriteLine("4- Help => Assist if needed");
            Console.WriteLine("5- Exit => Quits Application Gracefully ");
        }
    }
}
