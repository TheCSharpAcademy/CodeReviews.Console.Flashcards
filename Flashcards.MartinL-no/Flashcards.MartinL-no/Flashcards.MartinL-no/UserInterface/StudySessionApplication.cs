using Flashcards.MartinL_no.Controllers;
using Flashcards.MartinL_no.Models;

namespace Flashcards.MartinL_no.UserInterface;

internal class StudySessionApplication
{
    private readonly StudySessionController _studySessionController;
    private readonly StackManagerController _stackManagerController;

    public StudySessionApplication(StudySessionController studySessionController, StackManagerController stackManagerController)
    {
        _studySessionController = studySessionController;
        _stackManagerController = stackManagerController;
    }

    public void Study()
    {
        var stackName = SelectStack();
        var stack = _stackManagerController.GetStackByName(stackName);
        var score = 0;

        if (stack.Flashcards.Count() == 0)
        {
            Helpers.ShowMessage("Stack has no flashcards");
            return;
        }

        foreach (var flashcard in stack.Flashcards)
        {
            Console.Clear();
            TableVisualizationEngine.ShowStudyQuestionTable(flashcard, stackName);

            Console.WriteLine("""

            Input your answer to this card
            Or 0 to exit

            """);

            var answer = Console.ReadLine();

            if (answer == "0") break;
            else if (answer == flashcard.Back)
            {
                Console.WriteLine("""

                    Correct answer!
                    """);
                ++score;
            }
            else
            {
                Console.WriteLine($"""

                    Your answer was wrong:

                    You answered {answer}
                    The correct answer was {flashcard.Back}

                    """);
            }

            Helpers.Ask("Press any key to continue");
        }

        _studySessionController.AddStudySession(DateTime.Now, score, stack.Id);

        Console.WriteLine($"""

            Exiting study session
            You got {score} out of {stack.Flashcards.Count}
            """);
        Helpers.Ask("Press any key to continue");
    }

    public void ViewStudySessionData()
    {
        while (true)
        {
            Console.Clear();

            Helpers.ShowLine();
            Console.WriteLine($"""
                0 to return to main menu
                A to view all study sessions
                S to view by stack
                """);
            Helpers.ShowLine();

            var op = Console.ReadLine();

            switch (op.ToUpper())
            {
                case "0":
                    return;
                case "A":
                    ViewAllStudySessions();
                    break;
                case "S":
                    var stackName = SelectStack();
                    ViewByStack(stackName);
                    break;
                default:
                    Helpers.ShowMessage("Invalid input, please try again");
                    break;
            }
        }
    }


    private void ViewAllStudySessions()
    {
        var sessions = _studySessionController.GetSessions();
        ViewStudySessions(sessions);
    }

    private void ViewByStack(string stackName)
    {
        var sessions = _studySessionController.GetSessionsByName(stackName);
        ViewStudySessions(sessions);
    }

    private void ViewStudySessions(List<StudySessionDTO> sessions)
    {
        if (sessions.Count() == 0)
        {
            Helpers.ShowMessage("Stack(s) does not have any flashcards yet");
            return;
        }

        Console.Clear();
        TableVisualizationEngine.ShowStudySessionsTable(sessions);

        Helpers.ShowLine();
        Console.WriteLine("Press any key to return to menu: ");
        Helpers.ShowLine();
        Console.ReadLine();
    }

    private string SelectStack()
    {
        while (true)
        {
            Console.Clear();

            var stackNames = _stackManagerController.GetStackNames();
            TableVisualizationEngine.ShowStackNameTable(stackNames);

            Helpers.ShowLine();
            Console.WriteLine("""
                Enter the name of the stack you would like to use
                """);
            Helpers.ShowLine();

            var stackName = Console.ReadLine().Trim();

            if (stackNames.Exists(s => s == stackName)) return stackName;

            else Helpers.ShowMessage("Invalid input, please try again");
        }
    }
}