using AskInputs;
using ConsoleTableExt;
using ObjectsLibrary;

namespace Screens;

internal class StudySessionMenu
{
    AskInput askInput = new AskInput();
    SettingsLibrary.Settings settings = new();
    DbCommandsLibrary.DbCommands dbCmd = new();
    public void View()
    {
        bool exitMenu = false;
        List<object> optionsString = new List<object> {
            "1 - New Session",
            "2 - See Sessions",
            "0 - Return"
        };
        while (!exitMenu)
        {
            Console.Clear();
            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Study Sessions")
                .ExportAndWriteLine();
            Console.Write("\n");

            switch (askInput.PositiveNumber("Please select a valid option"))
            {
                case 0: exitMenu = true; continue;
                case 1:  NewStudyMenu(); break;
                case 2:  ViewAllSessions(); break;
                default: break;
            }
        }
        return;
    }

    public void NewStudyMenu()
    {
        Console.Clear();
        int stackViewId, stackIndex;
        StudySession studySession = new();
        Stack stack = new();

        StacksMenu stacks = new();
        stacks.DisplayStackList(dbCmd.Return.AllStacks(), "CHOOSE");

        do
        {
            stackViewId = askInput.PositiveNumber("Choose the stack you want to study, or 0 to return");
            stackIndex = dbCmd.Return.IdFromViewId(settings.stacksTableName, stackViewId);
            stack = dbCmd.Return.StackByIndex(stackIndex);
        }
        while ((stackViewId != 0) && (stackIndex == 0));
        if (stackViewId == 0) return;

        studySession.StackId = stack.Id;
        studySession.StackName = stack.Name;
        studySession.Score = 0;
        studySession.RoundsPlayed= 0;

        StartSession(studySession);
    }

    public void StartSession (StudySession session)
    {
        List<Card> cardsToShow = dbCmd.Return.CardsByStackId(session.StackId);
        session.Date = DateTime.Now;

        foreach (Card card in cardsToShow) 
        {
            session.RoundsPlayed += 1;
            session.Score += DisplayCard(card, session.StackName);
        }
        dbCmd.Insert.IntoTable(session);

        List<StudySession> displaySessionList = new()
        {
            session
        };
        Console.Clear();
        DisplaySessions(displaySessionList, "Session end");

    }

    public void ViewAllSessions()
    {
        Console.Clear();
        List<StudySession> allSessions = dbCmd.Return.AllSessions();
        if (allSessions != null)
        {
            foreach (StudySession session in allSessions)
            {
                session.StackName = dbCmd.Return.StackByIndex(session.StackId).Name;
            }
        }
        DisplaySessions(allSessions,"VIEW");
        askInput.AnyKeyToContinue();
    }

    private void DisplaySessions(List<StudySession> sessions, string title)
    {
        List<List<object>> displaySessions = new();

        if(sessions != null)
        {
            foreach (StudySession session in sessions)
            {
                displaySessions.Add(
                    new List<object> { session.Date.ToString("g"), session.StackName, session.RoundsPlayed, session.Score });
            }
        }
        else
        {
            title = "Empty";
            displaySessions.Add(
                    new List<object> { "", "", "", ""});
        }
        ConsoleTableBuilder.From(displaySessions)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Date", "Stack", "Rounds Played", "Score")
                .WithTitle(title)
                .ExportAndWriteLine();
        Console.Write("\n");
    }

    private int DisplayCard(Card cardToDisplay, string title)
    {
        int scoreToAdd = 0;

        List<object> titleList = new List<object>{title};

        List<object> cardWithoutAnswer = 
            new List<object> {
            cardToDisplay.Prompt,
            "                                  ",
            "???"};

        List<object> cardWithAnswer =
            new List<object> {
            cardToDisplay.Prompt,
            "                                  ",
            cardToDisplay.Answer};

        Console.Clear();

        ConsoleTableBuilder.From(titleList)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine();
        Console.Write("\n");

        ConsoleTableBuilder.From(cardWithoutAnswer)
            .WithTextAlignment(new Dictionary<int, TextAligntment> {{ 0, TextAligntment.Center }})
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);
        Console.Write("\n");

        askInput.AnyKeyToContinue("Press any key to show the answer");

        Console.Clear();

        ConsoleTableBuilder.From(titleList)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine();
        Console.Write("\n");

        ConsoleTableBuilder.From(cardWithAnswer)
            .WithTextAlignment(new Dictionary<int, TextAligntment> { { 0, TextAligntment.Center } })
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);
        Console.Write("\n");

        if(askInput.IsRightArrow_FromLeftOrRight($"WRONG: < Left arrow\nRight: > Right arrow"))
        {
            scoreToAdd++;
        }
        return scoreToAdd;
    }
}
