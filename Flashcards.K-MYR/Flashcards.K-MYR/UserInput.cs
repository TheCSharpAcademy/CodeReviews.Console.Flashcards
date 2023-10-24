using Flashcards.K_MYR.Models;
using System.Diagnostics;
using System.Text;

namespace Flashcards.K_MYR;

internal class UserInput
{
    internal static void MainMenu()
    {
        while (true)
        {
            Console.Clear();

            string[] options = { "Manage Stacks/Flashcards ", "Study Area ", "View Study Session Data ", "Yearly Report ", "Exit " };
            int selected = Helpers.PrintMenu(options);

            switch (selected)
            {
                case 0:
                    ManageStacksMenu();
                    break;
                case 1:
                    StudyArea();
                    break;
                case 2:
                    SessionDataMenu();
                    break;
                case 3:
                    ReportMenu();
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                    break;
            }
        }
    }

    internal static void ManageStacksMenu()
    {
        bool returnToMainMenu = false;

        while (!returnToMainMenu)
        {
            var stacks = SqlController.SelectStacksFromDB();
            List<CardStackDto> stackDTOs = new();
            int index = 1;

            foreach (var stack in stacks)
            {
                stackDTOs.Add(new CardStackDto
                {
                    Row = index,
                    Name = stack.Name,
                    NumberOfCards = stack.NumberOfCards,
                    Created = stack.Created
                });
                index++;
            }

            Helpers.PrintStacksMenu(stackDTOs);

            int selected = 3;
            bool actionKeyPressed = false;
            int row;

            while (!actionKeyPressed)
            {
                if (stackDTOs.Count != 0)
                {
                    Console.SetCursorPosition(1, selected);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(">");
                    Console.ResetColor();
                    Console.CursorTop = stackDTOs.Count * 2 + 7;
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (stackDTOs.Count != 0)
                        {
                            Console.SetCursorPosition(1, selected);
                            Console.Write(" ");
                            selected = Math.Max(3, selected - 2);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (stackDTOs.Count != 0)
                        {
                            Console.SetCursorPosition(1, selected);
                            Console.Write(" ");
                            selected = Math.Min(stackDTOs.Count * 2 + 1, selected + 2);
                        }
                        break;
                    case ConsoleKey.A:
                        Console.CursorLeft = 0;
                        string Name = GetStackName("Please enter a unique name for the new stack (max 25 letters): ");
                        SqlController.InsertStack(Name);
                        actionKeyPressed = true;
                        break;
                    case ConsoleKey.D:
                        if (stackDTOs.Count != 0)
                        {
                            row = (selected - 3) / 2;
                            var stack = stacks.Where(x => x.Name == stackDTOs[row].Name).First();
                            stack.Delete();
                            actionKeyPressed = true;
                        }
                        break;
                    case ConsoleKey.R:
                        if (stackDTOs.Count != 0)
                        {
                            string newName = GetStackName("Please enter a new unique name for the stack (max 50 letters): ");
                            row = (selected - 3) / 2;
                            var stack = stacks.Where(x => x.Name == stackDTOs[row].Name).First();
                            stack.Rename(newName);
                            actionKeyPressed = true;
                        }
                        break;
                    case ConsoleKey.Escape:
                        actionKeyPressed = true;
                        returnToMainMenu = true;
                        break;
                    case ConsoleKey.Enter:
                        if (stackDTOs.Count != 0)
                        {
                            row = (selected - 3) / 2;
                            var selectedStack = stacks.Where(x => x.Name == stackDTOs[row].Name).First();
                            FlashcardMenu(selectedStack);
                            actionKeyPressed = true;
                        }
                        break;
                }
            }
        }
    }

    internal static void FlashcardMenu(CardStack cardStack)
    {
        bool returnToStacksMenu = false;

        while (!returnToStacksMenu)
        {
            var cards = SqlController.SelectFlashcardsFromDB(args: $"WHERE StackId = {cardStack.StackId}");
            List<FlashcardDto> cardDTOs = new();

            int index = 1;

            foreach (var card in cards)
            {
                cardDTOs.Add(new FlashcardDto
                {
                    Row = index,
                    FrontText = card.FrontText,
                    BackText = card.BackText,
                    Created = card.Created
                });
                index++;
            }

            Helpers.PrintCardsMenu(cardDTOs, cardStack.Name);

            int selected = 3;
            bool actionKeyPressed = false;

            while (!actionKeyPressed)
            {
                if (cardDTOs.Count != 0)
                {
                    Console.SetCursorPosition(1, selected);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(">");
                    Console.ResetColor();
                    Console.CursorTop = cardDTOs.Count * 2 + 7;
                }

                int row;
                string frontText;
                string backText;

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (cardDTOs.Count != 0)
                        {
                            Console.SetCursorPosition(1, selected);
                            Console.Write(" ");
                            selected = Math.Max(3, selected - 2);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (cardDTOs.Count != 0)
                        {
                            Console.SetCursorPosition(1, selected);
                            Console.Write(" ");
                            selected = Math.Min(cardDTOs.Count * 2 + 1, selected + 2);
                        }
                        break;
                    case ConsoleKey.A:
                        Console.CursorLeft = 0;
                        frontText = GetFlashcardText("Please enter the text of the frontside (max 25 letters): ");
                        backText = GetFlashcardText("Please enter the text of the backside (max 25 letters): ");
                        SqlController.InsertFlashcard(cardStack.StackId, frontText, backText);
                        cardStack.UpdateNumberOfCards();
                        actionKeyPressed = true;
                        break;
                    case ConsoleKey.D:
                        if (cardDTOs.Count != 0)
                        {
                            row = (selected - 3) / 2;
                            cards[row].Delete();
                            cardStack.UpdateNumberOfCards(-1);
                            actionKeyPressed = true;
                        }
                        break;
                    case ConsoleKey.B:
                        if (cardDTOs.Count != 0)
                        {
                            Console.CursorLeft = 0;
                            backText = GetFlashcardText("Please enter the text of the backside: ");
                            row = (selected - 3) / 2;
                            cards[row].UpdateBackText(backText);
                            actionKeyPressed = true;
                        }
                        break;
                    case ConsoleKey.F:
                        if (cardDTOs.Count != 0)
                        {
                            Console.CursorLeft = 0;
                            frontText = GetFlashcardText("Please enter the text of the frontside: ");
                            row = (selected - 3) / 2;
                            cards[row].UpdateFrontText(frontText);
                            actionKeyPressed = true;
                        }
                        break;
                    case ConsoleKey.Escape:
                        returnToStacksMenu = true;
                        actionKeyPressed = true;
                        break;
                }
            }
        }
    }

    internal static void StudyArea()
    {
        bool returnToMainMenu = false;

        while (!returnToMainMenu)
        {
            Console.Clear();

            var stacks = SqlController.SelectStacksFromDB();
            List<CardStackDto> stackDTOs = new();
            int index = 1;

            foreach (var stack in stacks)
            {
                stackDTOs.Add(new CardStackDto
                {
                    Row = index,
                    Name = stack.Name,
                    NumberOfCards = stack.NumberOfCards,
                    Created = stack.Created
                });
                index++;
            }

            Helpers.PrintStackOptions(stackDTOs);

            int selected = 3;
            bool actionKeyPressed = false;
            int row;

            while (!actionKeyPressed)
            {
                if (stackDTOs.Count != 0)
                {
                    Console.SetCursorPosition(1, selected);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(">");
                    Console.ResetColor();
                    Console.CursorTop = stackDTOs.Count * 2 + 7;
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (stackDTOs.Count != 0)
                        {
                            Console.SetCursorPosition(1, selected);
                            Console.Write(" ");
                            selected = Math.Max(3, selected - 2);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (stackDTOs.Count != 0)
                        {
                            Console.SetCursorPosition(1, selected);
                            Console.Write(" ");
                            selected = Math.Min(stackDTOs.Count * 2 + 1, selected + 2);
                        }
                        break;
                    case ConsoleKey.Escape:
                        actionKeyPressed = true;
                        returnToMainMenu = true;
                        break;
                    case ConsoleKey.Enter:
                        if (stackDTOs.Count != 0)
                        {
                            row = (selected - 3) / 2;
                            var selectedStack = stacks.Where(x => x.Name == stackDTOs[row].Name).First();
                            if (selectedStack.NumberOfCards != 0)
                            {
                                StudyStack(selectedStack);
                                actionKeyPressed = true;
                            }
                            else
                            {
                                Console.CursorLeft = 0;
                                Console.WriteLine("----------------------------------------------");
                                Console.WriteLine("| Please add flashcards to this stack first! |");
                                Console.WriteLine("----------------------------------------------");
                                Console.CursorTop += 4;
                            }
                        }
                        break;
                }
            }
        }
    }

    internal static void StudyStack(CardStack stack)
    {
        CancellationTokenSource cancelTokenSource = new();
        CancellationToken token = cancelTokenSource.Token;

        var flashcards = SqlController.SelectFlashcardsFromDB(args: $"WHERE StackId = {stack.StackId}");
        flashcards.Shuffle();

        Console.Clear();
        Console.WriteLine("-------------------------------------------------------------");
        Console.WriteLine($"| {stack.Name,-25}  | Score:     | Timer:          |");
        Console.WriteLine("-------------------------------------------------------------");

        string input;
        int score = 0;
        int answeredQuestions = 0;
        Console.CursorTop = 3;

        Thread thread = new(() => Helpers.Count(token));
        thread.Start();
        Stopwatch sw = Stopwatch.StartNew();

        for (int i = 0; i < flashcards.Count; i++)
        {            
            Console.WriteLine($"| {flashcards[i].FrontText,-27}|                              |");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("| Enter - Submit Answer  | Enter 0 to exit |");
            Console.WriteLine("--------------------------------------------");

            Console.CursorTop -= 5;
            Console.CursorLeft = 31;

            input = GetStringInput(25, allowEmptyString: true);
            
            Console.CursorLeft = 31;

            if (input == "0")
            {
                break;
            }
            else if (input == flashcards[i].BackText)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(flashcards[i].BackText.PadRight(23));
                score++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(flashcards[i].BackText.PadRight(23));
            }

            Console.ResetColor();

            int top = Console.CursorTop;
            Console.SetCursorPosition(38, 1);
            Console.Write(score);

            Console.SetCursorPosition(0, top + 2);
            answeredQuestions++;
        }

        cancelTokenSource.Cancel();
        sw.Stop();
        TimeSpan timeSpan = sw.Elapsed;

        Console.CursorTop += 6;
        Console.CursorLeft = 0;

        if (answeredQuestions > 0)
        {
            Console.WriteLine($"You got {score} out of {answeredQuestions} questions right!");
            SqlController.InsertSession(stack.StackId, timeSpan.Ticks, score);
        }

        Console.Write("Press Enter to return to the menu");
        Console.ReadLine();
    }

    internal static void SessionDataMenu()
    {
        var sessions = SqlController.SelectSessionsFromDB();
        List<SessionDto> sessionsDTO = new();
        int index = 1;

        foreach (var session in sessions)
        {
            sessionsDTO.Add(new SessionDto
            {
                Row = index,
                StackName = session.StackName,
                Date = session.Date.ToString("dd.MM.yyyy HH\\:mm"),
                Score = session.Score,
                Duration = session.Duration.ToString("hh\\:mm\\:ss"),
            });
            index++;
        }

        bool returnToMainMenu = false;

        while (!returnToMainMenu)
        {
            Console.Clear();
            Helpers.PrintSessions(sessionsDTO);

            Console.WriteLine("\n-------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Sort by:  N - Name Of Stack | D - Date | S - Score | U - Duration | Esc - Return To Main Menu |");
            Console.WriteLine("-------------------------------------------------------------------------------------------------");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.N:
                    sessionsDTO.Sort((x, y) => x.StackName.CompareTo(y.StackName));
                    Helpers.ReassignRows(sessionsDTO);
                    break;
                case ConsoleKey.D:
                    sessionsDTO = sessionsDTO.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();
                    Helpers.ReassignRows(sessionsDTO);
                    break;
                case ConsoleKey.S:
                    sessionsDTO = sessionsDTO.OrderByDescending(x => x.Score).ToList();
                    Helpers.ReassignRows(sessionsDTO);
                    break;
                case ConsoleKey.U:
                    sessionsDTO = sessionsDTO.OrderByDescending(x => TimeSpan.Parse(x.Duration)).ToList();
                    Helpers.ReassignRows(sessionsDTO);
                    break;
                case ConsoleKey.Escape:
                    returnToMainMenu = true;
                    break;
            }
        }
    }

    private static void ReportMenu()
    {
        Console.Clear();
        Console.WriteLine("-----------------------------");
        Console.WriteLine("| Please enter a year: YYYY |");
        Console.WriteLine("-----------------------------");

        Console.WriteLine("\n--------------------------------------------------");
        Console.WriteLine("| Enter - Submit Year | Esc - Return To Main Menu |");
        Console.WriteLine("---------------------------------------------------");

        bool returnToMenu = false;

        while (!returnToMenu)
        {
            Console.SetCursorPosition(23, 1);

            var sb = new StringBuilder();
            ConsoleKeyInfo key;
            bool actionKeyPressed = false;

            while (!actionKeyPressed)
            {
                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                            Console.CursorLeft -= 1;
                            Console.Write("Y");
                            Console.CursorLeft -= 1;
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (sb.Length == 4)
                        {
                            var tableData = SqlController.SumScorePerMonth(sb.ToString());
                            var tableData2 = SqlController.AvgScorePerMonth(sb.ToString());
                            var tableData3 = SqlController.SumTimePerMonth(sb.ToString());
                            var tableData4 = SqlController.AvgTimePerMonth(sb.ToString());
                            Helpers.PrintReport(tableData, tableData2, tableData3, tableData4, sb.ToString());
                            actionKeyPressed = true;
                        }
                        break;
                    case ConsoleKey.Escape:
                        actionKeyPressed = true;
                        returnToMenu = true;
                        break;
                    default:
                        if (sb.Length < 4)
                        {
                            if (int.TryParse(key.KeyChar.ToString(), out _))
                            {
                                sb.Append(key.KeyChar);
                                Console.Write(key.KeyChar.ToString());
                            }
                        }
                        break;
                }
            }
        }
    }

    internal static string GetStackName(string message)
    {
        string input;

        do
        {
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"| {message,-112}|");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");

            Console.CursorTop -= 2;
            Console.CursorLeft = message.Length + 2;

            input = GetStringInput(25);

            Console.CursorTop -= 1;
            Console.CursorLeft = 0;

        } while (SqlController.StackNameExists(input) == 1);

        return input;
    }

    internal static string GetFlashcardText(string message)
    {
        string input;

        Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"| {message,-111}|");
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------");

        Console.CursorTop -= 2;
        Console.CursorLeft = message.Length + 2;

        input = GetStringInput(25);

        Console.CursorTop -= 1;
        Console.CursorLeft = 0;

        return input;
    }

    internal static string GetStringInput(int charLimit, char placeHolder = ' ', bool allowEmptyString = false)
    {
        var sb = new StringBuilder();
        ConsoleKeyInfo key;

        bool enterPressed = false;

        while (!enterPressed)
        {
            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        Console.CursorLeft -= 1;
                        Console.Write(placeHolder);
                        Console.CursorLeft -= 1;
                    }
                    break;
                case ConsoleKey.Enter:
                    if (!allowEmptyString)
                    {
                        if (!string.IsNullOrEmpty(sb.ToString().Trim()))
                            enterPressed = true;
                    }                        
                    else
                    {
                        enterPressed = true;
                    }
                    break;
                default:
                    if (sb.Length < charLimit)
                    {
                        if (!char.IsControl(key.KeyChar))
                        {
                            sb.Append(key.KeyChar);
                            Console.Write(key.KeyChar.ToString());
                        }
                    }
                    break;
            }
        }
        return sb.ToString().Trim();
    }
}
