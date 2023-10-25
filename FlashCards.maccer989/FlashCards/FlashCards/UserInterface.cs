using DataAccessLibrary.DataAccess;
using Microsoft.Extensions.Configuration;

namespace FlashCardsUI
{
    public static class UserInterface
    {
        public static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = "";
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var config = builder.Build();
            output = config.GetConnectionString(connectionStringName);
            return output;
        }
        public static void GetMainMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\nMain Menu");
                Console.WriteLine("---------\n");
                Console.WriteLine("What would you like to do?\n");
                Console.WriteLine("Type 1 to Manage Stacks");
                Console.WriteLine("Type 2 to Manage FlashCards");
                Console.WriteLine("Type 3 to Manage Study");
                Console.WriteLine("Type 0 to Close Application");
                Console.WriteLine("---------------------------\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetStacksMainMenu();
                        break;
                    case "2":
                        GetFlashCardsMainMenu();
                        break;
                    case "3":
                        GetStudyMainMenu();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.");
                        break;
                }
            }
        }
        public static void GetStacksMainMenu()
        {
            SqlStacksCrud sql = new SqlStacksCrud(GetConnectionString());
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\nMain Menu");
                Console.WriteLine("---------\n");
                Console.WriteLine("What would you like to do?\n");
                Console.WriteLine("Type 1 to View all Stack");
                Console.WriteLine("Type 2 to Insert a Stack");
                Console.WriteLine("Type 3 to Delete a Stack");
                Console.WriteLine("Type 4 to Update a Stack");
                Console.WriteLine("Type 5 to Return to Main Menu\n");
                Console.WriteLine("Type 0 to Close Application");
                Console.WriteLine("---------------------------\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        RequestStackData.ReadAllStacks(sql); 
                        break;
                    case "2":
                        RequestStackData.CreateNewStack(sql); 
                        break;
                    case "3":
                        RequestStackData.RemoveStack(sql);
                        break;
                    case "4":
                        RequestStackData.UpdateAStack(sql);
                        break;
                    case "5":
                        Console.Clear();
                        GetMainMenu();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.");
                        break;
                }
            }
        }
        public static void GetFlashCardsMainMenu()
        {
            SqlFlashCardsCrud sql = new SqlFlashCardsCrud(GetConnectionString());
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\nMain Menu");
                Console.WriteLine("---------\n");
                Console.WriteLine("What would you like to do?\n");
                Console.WriteLine("Type 1 to View all FlashCards");
                Console.WriteLine("Type 2 to Insert a FlashCard");
                Console.WriteLine("Type 3 to Delete a FlashCard");
                Console.WriteLine("Type 4 to Update a FlashCard");
                Console.WriteLine("Type 5 to Return to Main Menu\n");
                Console.WriteLine("Type 0 to Close Application");
                Console.WriteLine("---------------------------\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        RequestFlashCardData.ReadAllFlashCards(sql);
                        break;
                    case "2":
                        RequestFlashCardData.CreateNewFlashCard(sql);
                        break;
                    case "3":
                        RequestFlashCardData.RemoveFlashCard(sql);
                        break;
                    case "4":
                        RequestFlashCardData.UpdateFlashCard(sql);
                        break;
                    case "5":
                        Console.Clear();
                        GetMainMenu();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.");
                        break;
                }
            }
        }
        public static void GetStudyMainMenu()
        {
            SqlStudySessionsCrud sql = new SqlStudySessionsCrud(GetConnectionString());
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\nMain Menu");
                Console.WriteLine("---------\n");
                Console.WriteLine("What would you like to do?\n");
                Console.WriteLine("Type 1 to View all Study Sessions");
                Console.WriteLine("Type 2 to Start a new Study Session");
                Console.WriteLine("Type 3 to Return to Main Menu\n");
                Console.WriteLine("Type 0 to Close Application");
                Console.WriteLine("---------------------------\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        RequestStudySessionData.ReadAllStudySessions(sql);
                        break;
                    case "2":
                        RequestStudySessionData.CreateNewStudySession(sql);
                        break;
                    case "3":
                        Console.Clear();
                        GetMainMenu();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 3.");
                        break;
                }
            }
        }
    }
}
