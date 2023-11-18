using static Flashcards.GlobalVariables;

namespace Flashcards
{
    internal class Menu
    {


        internal static void MainMenu()
        {
            string pageText = "Welcome to Flash Cards your study program.";
            ConsoleKeyInfo key;
            int option = 1;
            bool isSelected = false;
            bool exitMenu = false;
            (int left, int top) = Console.GetCursorPosition();
            string color = $"{checkMark}{green}   ";

            Console.CursorVisible = false;
            while (!exitMenu)
            {
                while (!isSelected)
                {
                    OpenMenu(pageText);

                    Console.WriteLine($@"{(option == 1 ? color : "    ")}EXIT    {resetColor}");
                    Console.WriteLine($@"{(option == 2 ? color : "    ")}  Manage Stacks{resetColor}");
                    Console.WriteLine($@"{(option == 3 ? color : "    ")}  Manage FlashCards{resetColor}");
                    Console.WriteLine($@"{(option == 4 ? color : "    ")}  Study {resetColor}");
                    Console.WriteLine($@"{(option == 5 ? color : "    ")}  Study Sessions Reporting {resetColor}");

                    key = Console.ReadKey(true);



                    switch (key.Key)

                    {
                        case ConsoleKey.DownArrow:
                            option = (option == 5 ? 1 : option + 1);
                            break;

                        case ConsoleKey.UpArrow:
                            option = (option == 1 ? 5 : option - 1);
                            break;

                        case ConsoleKey.Enter:
                            isSelected = true;
                            break;
                    }
                }

                switch (option)
                {
                    case 1://Exit Complete
                        exitMenu = true;
                        isSelected = true;
                        break;
                    case 2://Manage Stacks Complete
                        Console.WriteLine(option);
                        StackMenu();
                        isSelected = false;
                        break;
                    case 3://Manage Flash Cards Complete
                        Console.WriteLine(option);
                        FLashCardMenu();
                        isSelected = false;
                        break;
                    case 4://Study Complete
                        Console.WriteLine(option);
                        StudyMenu();
                        isSelected = false;
                        break;
                    case 5: //Study Session Reporting Working
                        Console.WriteLine(option);
                        ReportingMenu();
                        isSelected = false;
                        break;

                }

            }
            Console.Clear();
            Console.WriteLine($@"Goodbye");
            Console.ReadLine();

        }
        internal static void StackMenu()
        {
            Console.Clear();
            string pageText = "Stack Menu";
            ConsoleKeyInfo key;
            int option = 1;
            bool exitMenu = false;
            bool isSelected = false;
            string color = $"{checkMark}{green}   ";


            while (!exitMenu)
            {
                while (!isSelected)
                {
                    OpenMenu(pageText);

                    Console.WriteLine($@"{(option == 1 ? color : "    ")}BACK{resetColor}");
                    Console.WriteLine($@"{(option == 2 ? color : "    ")}  View Stacks{resetColor}");
                    Console.WriteLine($@"{(option == 3 ? color : "    ")}  Add Stack{resetColor}");
                    Console.WriteLine($@"{(option == 4 ? color : "    ")}  Delete Stack{resetColor}");
                   
                    key = Console.ReadKey(true);



                    switch (key.Key)
                    {
                        case ConsoleKey.DownArrow:
                            option = (option == 4 ? 1 : option + 1);
                            break;
                        case ConsoleKey.UpArrow:
                            option = (option == 1 ? 4 : option - 1);
                            break;

                        case ConsoleKey.Enter:
                            isSelected = true;
                            break;
                    }
                }

                switch (option)
                {
                    case 1://Back
                        exitMenu = true;
                        isSelected = true;
                        break;
                    case 2://View Stacks
                        ActionPages.ViewStacks();
                        isSelected = false;
                        break;
                    case 3://Add Stack
                        ActionPages.AddStack();
                        isSelected = false;
                        //Console.ReadLine();
                        break;
                    case 4://Delete Stack
                        ActionPages.DeleteStacks();
                        isSelected = false;
                        break;

                }
            }


        }
        internal static void FLashCardMenu()
        {
            Console.Clear();
            string pageText = "Flash Card Menu";
            ConsoleKeyInfo key;
            int option = 1;

            bool exitMenu = false;
            bool isSelected = false;
            string color = $"{checkMark}{green}   ";


            while (!exitMenu)
            {
                while (!isSelected)
                {
                    OpenMenu(pageText);

                    Console.WriteLine($@"{(option == 1 ? color : "    ")}BACK{resetColor}");
                    Console.WriteLine($@"{(option == 2 ? color : "    ")}  View Cards{resetColor}");
                    Console.WriteLine($@"{(option == 3 ? color : "    ")}  Add Card{resetColor}");
                    Console.WriteLine($@"{(option == 4 ? color : "    ")}  Delete Cards{resetColor}");

                    key = Console.ReadKey(true);



                    switch (key.Key)
                    {
                        case ConsoleKey.DownArrow:
                            option = (option == 4 ? 1 : option + 1);
                            break;
                        case ConsoleKey.UpArrow:
                            option = (option == 1 ? 4 : option - 1);
                            break;
                        case ConsoleKey.Enter:
                            isSelected = true;
                            break;
                    }
                }

                switch (option)
                {
                    case 1://Back
                        exitMenu = true;
                        isSelected = true;
                        break;
                    case 2://View Cards
                        ActionPages.ViewCards();
                        isSelected = false;
                        break;
                    case 3://Add Card
                        ActionPages.AddCard();
                        isSelected = false;
                        break;
                    case 4://Delete Cards
                        ActionPages.DeleteCards();
                        isSelected = false;
                        break;

                }
            }
        }

        internal static void StudyMenu()
        {
            bool exitMenu = false;
            String pageText = "Please choose the stack you want to study.";

            while (!exitMenu)
            {
                Stack selectedStack = ActionPages.ChooseStack(pageText);
                if (selectedStack.StackID == 0)
                {
                    exitMenu = true;
                    break;
                }
                else 
                {
                    List <DtoStackAndCard> studySession = DtoStackAndCard.LoadStackAndCardList(selectedStack);
                    if (studySession.Count > 0)
                    {
                        StudySession.StudyStack(studySession);
                    }
                    else
                    {
                        Console.WriteLine("This stack has no cards.  Please add some cards.");
                        Console.ReadLine();
                    }
                    //Action Page Flash Card Session
                }
            }
        }

        internal static void ReportingMenu()
        {
            Console.Clear();
            string pageText = "Reporting Menu";
            ConsoleKeyInfo key;
            int option = 1;
            bool exitMenu = false;
            bool isSelected = false;
            string color = $"{checkMark}{green}   ";


            while (!exitMenu)
            {
                while (!isSelected)
                {
                    OpenMenu(pageText);

                    Console.WriteLine($@"{(option == 1 ? color : "    ")}BACK{resetColor}");
                    Console.WriteLine($@"{(option == 2 ? color : "    ")}  View Session{resetColor}");
                    Console.WriteLine($@"{(option == 3 ? color : "    ")}  By Month Year{resetColor}");

                    /// going to need to figure out this one as well
                    key = Console.ReadKey(true);

                    switch (key.Key)
                    {
                        case ConsoleKey.DownArrow:
                            option = (option == 3 ? 1 : option + 1);
                            break;
                        case ConsoleKey.UpArrow:
                            option = (option == 1 ? 3 : option - 1);
                            break;
                        case ConsoleKey.Enter:
                            isSelected = true;
                            break;
                    }
                }

                switch (option)
                {
                    case 1://Exit
                        exitMenu = true;
                        isSelected = true;
                        break;
                    case 2://View Sessions
                        ActionPages.ViewStudySessions();
                        isSelected = false;
                        break;
                    case 3://View Pivot Table
                        ActionPages.ReportByYear();
                        isSelected = false;
                        break;
                }
            }
        }

        internal static void OpenMenu(string pageText)
        {
            Console.Clear();

            (int left, int top) = Console.GetCursorPosition();

            Console.Clear();
            Console.SetCursorPosition(left, top);
            Console.WriteLine($@"Use {green}{upArrow}{resetColor} and {green}{downArrow}{resetColor} to navigate and press {green}Enter{resetColor} to select.");
            Console.WriteLine(pageText);
            Console.WriteLine();

        }

        
    }
}
