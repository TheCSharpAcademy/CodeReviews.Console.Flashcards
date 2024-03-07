using Flash.Helper.MainHelper;
using Spectre.Console;

namespace Flash.Launching;
internal class StudyHistory
{
    internal static void GetStudyHistory()
    {
        Console.Clear();

        ShowBanner.GetShowBanner("View Study Session Data", Color.Orange3);

        ShowStudyHistory.GetShowStudyHistory();

        ReturnComment.MainMenuReturnComments();
    }
}