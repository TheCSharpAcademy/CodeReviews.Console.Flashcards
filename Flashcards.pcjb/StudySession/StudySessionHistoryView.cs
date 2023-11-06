namespace Flashcards;

using ConsoleTableExt;

class StudySessionHistoryView : BaseView
{
    private readonly StudySessionController controller;
    private readonly List<StudySessionHistoryDto> history;

    public StudySessionHistoryView(StudySessionController controller, List<StudySessionHistoryDto> history)
    {
        this.controller = controller;
        this.history = history;
    }

    public override void Body()
    {
        Console. WriteLine("Study Session History");
        if (history != null && history.Count > 0)
        {
            ConsoleTableBuilder.From(history).ExportAndWriteLine();
        }
        else
        {
            Console.WriteLine("No study results found.");
        }
        Console.WriteLine("Press enter to return to menu.");
        Console.ReadLine();
        controller.ShowMenu();
    }
}