using System.Globalization;

namespace Flashcards;

public class SessionController
{
    internal static void StudySessionHandler()
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            Console.Clear();

            string? userInput = SessionsView.SessionsMenu();

            switch (userInput)
            {
                case "View All Sessions":
                    GetSessionTable();
                    Views.ShowMessage("Press any key to continue.");
                    Console.ReadLine();
                    break;
                case "Start Study Session":
                    NewSessionHandler();
                    break;
                default:
                    exitMenu = true;
                    break;
            }
        }
    }

    internal static void NewSessionHandler()
    {
        int count;
        count = Controller.CountStackTable();

        if (count == 0)
        {
            Views.ShowMessage("Currently no stacks to work with.");
            Views.ShowMessage("Press any key to continue.");
            Console.ReadLine();
        }
        else
        {
            int stackId;
            string? stackName;

            var stackList = FlashcardsModel.FetchStacks();
            Controller.DisplayConvertedStacks(stackList);

            stackName = StacksView.SelectStackMenu(stackList, count);

            if (stackName != "Exit")
            {
                bool stackFound = false;

                foreach (var stack in stackList)
                {
                    if (stackName?.ToLower() == stack.StackName?.ToLower())
                    {
                        stackId = stack.StackId;
                        stackName = stack.StackName;
                        stackFound = true;
                        GetStackForStudy(stackId, stackName);
                        break;
                    }
                }
                if (stackFound)
                {
                    Views.ShowErrorMessage($"Something went wrong. {stackName} wasn't found. Returning to main.");
                }
            }
        }
    }

    internal static void GetStackForStudy(int stackId, string? stackName)
    {
        var cardsList = FlashcardsModel.FetchCardsInStack(stackId);

        if (cardsList != null && cardsList.Count > 0)
        {
            Views.ShowMessage($"Start study session with {stackName}?\n");
            string input = Views.SelectYesOrNo().ToLower();

            if (input == "yes")
            {
                StartStudySession(cardsList, stackName, stackId);
            }
        }
        else
        {
            Views.ShowMessage($"There are no cards in {stackName}.");
            Console.ReadLine();
        }
    }

    internal static void StartStudySession(List<Flashcard> cardsList, string? stackName, int stackId)
    {
        int points = 0;
        int cardsShown = 0;
        DateTime date = DateTime.Now.Date;

        foreach (Flashcard card in cardsList)
        {
            Console.Clear();
            Views.ShowWorkingStack(stackName);

            VisualizationEngine.DisplayCardFront(card);

            string? userInput = Input.GetString("Input answer to card. Press 0 to cancel.");

            if (userInput == "0")
            {
                break;
            }
            else
            {
                Console.Clear();
                Views.ShowWorkingStack(stackName);

                bool correct = CheckCardBack(userInput, card);
                VisualizationEngine.DisplayCardBack(card);

                if (correct)
                {
                    Views.ShowMessage("Correct!");
                    points++;
                }
                else
                {
                    Views.ShowMessage($"{userInput} was wrong. The correct answer was {card.CardBack}");
                }

                Views.ShowMessage("Press enter to continue.");
                Console.ReadLine();
                cardsShown++;
            }
        }
        Views.ShowMessage($"Ending study session.");

        if (cardsShown != 0)
        {
            Views.ShowMessage($"You got {points} out of {cardsShown} right.");
            Console.ReadLine();

            SessionsModel.InsertSession(date, stackName, stackId, points, cardsShown);
        }
    }

    internal static bool CheckCardBack(string? input, Flashcard card)
    {
        bool correct = false;

        if (input?.ToLower() == card?.CardBack.ToLower())
        {
            correct = true;
        }

        return correct;
    }

    internal static void GetSessionTable()
    {
        List<Session> sessionList = SessionsModel.FetchSessions();

        if (sessionList.Count > 0)
        {
            VisualizationEngine.DisplaySessionsTable(sessionList);
        }
        else
        {
            Views.ShowMessage("No recorded sessions.");
        }
    }

    internal static void ViewSessionData()
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            Console.Clear();
            string? input = SessionsView.ReportMenu();

            switch (input)
            {
                case "View Amount of Sessions Per Month":
                    ViewSessionsPerMonth();
                    break;
                case "View Average Score Per Session":
                    ViewAverageScorePerMonth();
                    break;
                default:
                    exitMenu = true;
                    break;
            }
        }
    }

    internal static void ViewSessionsPerMonth()
    {
        string? year = Input.GetDate();

        if (year != "0")
        {
            var sessionList = SessionsModel.FetchMonthlySession(year);
            bool dataFound = CountReportList(sessionList);

            if (dataFound)
            {
                Console.Clear();
                VisualizationEngine.DisplaySessionsTable(sessionList, year);
            }

            Views.ShowMessage("Press any key to continue.");
            Console.ReadLine();
        }
    }

    internal static void ViewAverageScorePerMonth()
    {
        string? year = Input.GetDate();

        if (year != "0")
        {
            var averageList = SessionsModel.FetchMonthlyAverage(year);
            bool dataFound = CountReportList(averageList);

            if (dataFound)
            {
                Console.Clear();
                VisualizationEngine.DisplayAverageScoreTable(averageList, year);
            }

            Views.ShowMessage("Press any key to continue.");
            Console.ReadLine();
        }
    }

    internal static bool CountReportList(List<ReportItem> list)
    {
        bool valid = false;

        if (list == null)
        {
            Views.ShowErrorMessage("Something went wrong. List was null");
        }
        else if (list.Count <= 0)
        {
            Views.ShowMessage("No sessions found.");
        }
        else
        {
            valid = true;
        }

        return valid;
    }
}