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
                Console.Clear();
                Console.WriteLine("Flashcard Application\n");
                Console.WriteLine("Type C to create a new Stack of flashcards");
                Console.WriteLine("Type V to view current Stacks");
                Console.WriteLine("Type D to delete a Stack");
                Console.WriteLine("Type U to update a Stack"); // updates stack and flashcards
                Console.WriteLine("Type S to study a Stack");
                Console.WriteLine("Type H to view your Game History");
                Console.WriteLine("Type R to view your Game reports");
                string firstOption = Console.ReadLine().ToUpper().Trim();
                Console.Clear();

                switch(firstOption)
                {
                    case "C":
                        Console.WriteLine("Please enter the name of your new Stack");
                        string name = Console.ReadLine();
                        Stacks_DB_Access.Insert_Stack(name);
                        int Stack_Id = Stacks_DB_Access.GetOneStack(name).ID;
                        bool endLoop = false;
                        while (!endLoop)
                        {
                            Console.Clear();
                            Console.WriteLine("\nPlease type in the front side of the flashcard");
                            String Front= Console.ReadLine();
                            Console.WriteLine("\nPlease type in the back side of the flashcard");
                            String Back = Console.ReadLine();
                            Flashcard_DB_Access.Insert_Flashcard(Stack_Id, Front, Back);
                            Console.WriteLine("\nType Q if you are done adding flashcards to the stack or press any key to keep adding cards");
                            string option = Console.ReadLine();
                            endLoop = option.ToUpper() == "Q" ? true : false;
                        }
                        Console.WriteLine("press any key to go back to main menu");
                        Console.ReadLine();
                        break;
                    case "V":
                        Console.WriteLine("Your Stacks");
                        TableVisualization.ShowTable(Stacks_DB_Access.GetAllStacks());
                        Console.WriteLine("\nType in the Stack you wish to view");
                        int stack_Id = Stacks_DB_Access.GetOneStack(Console.ReadLine().Trim()).ID;
                        Console.Clear();
                        TableVisualization.ShowTable(Flashcard_DB_Access.GetAllFlashcards(stack_Id));
                        Console.WriteLine("press any key to go back to main menu");
                        Console.ReadLine();
                        break;
                    case "D":
                        Console.WriteLine("Please enter the stack name you would like to delete");
                        name = Console.ReadLine();
                        int stackID = Stacks_DB_Access.GetOneStack(name).ID;
                        Stacks_DB_Access.Delete_Stack(stackID);
                        Console.WriteLine("press any key to go back to main menu");
                        Console.ReadLine();
                        break;
                    case "U":
                        Console.WriteLine("Please enter the stack name you would like to update");
                        name = Console.ReadLine();
                        stackID = Stacks_DB_Access.GetOneStack(name).ID;
                        Console.WriteLine($"type N to change the stack's name");
                        Console.WriteLine($"type V to view and update the flashcards in {name}");
                        string userChoice = Console.ReadLine();
                        Console.Clear();
                        switch (userChoice.ToUpper())
                        {
                            case "N":
                                Console.Write($"Enter new name for {name} stack here: ");
                                string newName = Console.ReadLine();
                                Stacks_DB_Access.Update_Stack(stackID, newName);
                                break;
                            case "V":
                                Console.WriteLine($"{name} Flashcards");
                                TableVisualization.ShowTable(Flashcard_DB_Access.GetAllFlashcards2(stackID));

                                Console.WriteLine("Type I to insert a Flashcard");
                                Console.WriteLine("Type U to update a Flashcard");
                                Console.WriteLine("Type D to delete a Flashcard");
                                string secondChoice = Console.ReadLine().ToUpper().Trim();

                                switch (secondChoice)
                                {
                                    case "I":
                                        Console.WriteLine("\nNew Flashcard Info");
                                        Console.WriteLine("\nPlease type in the front side of the flashcard");
                                        String Front = Console.ReadLine();
                                        Console.WriteLine("\nPlease type in the back side of the flashcard");
                                        String Back = Console.ReadLine();
                                        Flashcard_DB_Access.Insert_Flashcard(stackID, Front, Back);
                                        break;

                                    case "U":
                                        Console.WriteLine("Type in the Id of the flashcard you wish to update");
                                        int Id = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("Type in new front side of the card or hit enter to leave it as is");
                                        string front= Console.ReadLine();
                                        Console.WriteLine("Type in new back side of the card or hit enter to leave it as is");
                                        string back = Console.ReadLine();

                                        front = String.IsNullOrEmpty(front) ? null : front;
                                        back = String.IsNullOrEmpty(back) ? null : back;

                                        Flashcard_DB_Access.Update_Flashcard(Id, front, back);
                                        break;
                                    case "D":
                                        Console.WriteLine("Type in the Id of the flashcard you wish to update");
                                        Id = Convert.ToInt32(Console.ReadLine());
                                        Flashcard_DB_Access.Delete_Flashcard(Id);
                                        break;
                                }
                                break;
                        }
                        Console.WriteLine("press any key to go back to main menu");
                        Console.ReadLine();
                        break;
                    case "S":
                        StudyGame studyGame = new StudyGame();
                        studyGame.ShowMenu();
                        Console.WriteLine("press any key to go back to main menu");
                        break;
                    case "H":
                        Console.WriteLine("Your Game History \n");
                        TableVisualization.ShowTable(StudySession_DB_Access.GetAllSessions());
                        Console.WriteLine("press any key to go back to main menu");
                        Console.ReadLine();
                        break;
                    case "R":
                        TableVisualization.ShowTable(Stacks_DB_Access.GetAllStacks());
                        Console.WriteLine("Please enter the stack Id which you'd like to select");
                        int stackId= Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Please enter the year you'd like to view");
                        int year = Convert.ToInt32(Console.ReadLine());
                        StudySession_DB_Access.GetReports(year, stackId);
                        Console.WriteLine("enter any key to go back");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
