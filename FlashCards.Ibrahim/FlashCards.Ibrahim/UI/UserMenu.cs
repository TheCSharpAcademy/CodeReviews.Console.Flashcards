using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Ibrahim.Helper;
using FlashCards.Ibrahim.Database_Access;
using FlashCards.Ibrahim.Database_Acess;
using FlashCards.Ibrahim.Models;
using System.ComponentModel.Design;
namespace FlashCards.Ibrahim.UI
{
    public class UserMenu
    {
        public static void ShowMenu()
        {
            bool endApp = false;

            while(!endApp)
            {
                Console.WriteLine("Flashcard application");
                Console.WriteLine("Type S to create a new Stack");
                Console.WriteLine("Type V to view Current Stacks");
                Console.WriteLine("Type D to delete a Stack");
                Console.WriteLine("Type U to Update a current Stack");
                Console.WriteLine("Type P to study a Stack");

                string firstOption = helpers.ValidateFirstChoice(Console.ReadLine());

                switch(firstOption)
                {
                    case "S":
                        Console.WriteLine("Please enter the name of your new Stack");
                        string name = Console.ReadLine();
                        int Stack_Id= Stacks_DB_Access.Insert_Stack(name).ID;
                        bool endLoop = false;
                        while (!endLoop)
                        {
                            Console.Clear();
                            Console.WriteLine("Please type in the front side of the flashcard");
                            String Front= Console.ReadLine();
                            Console.WriteLine("Please type in the back side of the flashcard");
                            String Back = Console.ReadLine();
                            Flashcard_DB_Access.Insert_Flashcard(Stack_Id, Front, Back);
                            Console.WriteLine(@"Type Q if you are done adding flashcards to the stack 
or press any key to keep adding cards");
                            string option = Console.ReadLine();
                            endLoop = option.ToUpper() == "Q" ? true : false;
                        }
                        Console.WriteLine("press any key to go back to main menu");
                        Console.ReadLine();
                        break;
                    case "V":
                        Console.WriteLine("Your Stacks");
                        Stacks_DB_Access.GetAllStacks();

                        Console.WriteLine("Type in the Stack you wish to view");
                        int stack_Id = Stacks_DB_Access.GetOneStack(Console.ReadLine()).ID;
                        Flashcard_DB_Access.GetAllFlashcards(stack_Id);

                        break;
                    case "D":
                        break;
                    case "U":
                        break;
                    case "P":
                        StudyGame studyGame = new StudyGame();
                        studyGame.ShowMenu();
                        Console.WriteLine("press any key to go back to main menu");
                        break;
                }
            }
        }
    }
}
