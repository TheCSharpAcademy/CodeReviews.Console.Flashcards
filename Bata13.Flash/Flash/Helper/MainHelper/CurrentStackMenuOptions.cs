using Spectre.Console;

namespace Flash.Helper.MainHelper;
internal class CurrentStackMenuOptions
{
    internal static void GetCurrentStackMenuOptions(string currentWorkingStack)
    {
        Console.WriteLine($"Current Stack: {currentWorkingStack}\n");
        var rows = new List<Text>(){
                new Text("-Type 0 to Return to Main Menu", new Style(Color.Red, Color.Black)),
                new Text("-Type 1 to Change Current Stack", new Style(Color.Green, Color.Black)),
                new Text("-Type 2 to View All Flashcards in Stack", new Style(Color.Blue, Color.Black)),
                new Text("-Type 3 to View X amount of cards in stack", new Style(Color.Purple, Color.Black)),
                new Text("-Type 4 to Create a Flashcard in current stack", new Style(Color.Orange3, Color.Black)),
                new Text("-Type 5 to Edit a Flashcard", new Style(Color.Red, Color.Black)),
                new Text("-Type 6 to Delete a Flashcard", new Style(Color.Green, Color.Black)),
                };

        AnsiConsole.Write(new Rows(rows));
    }
}
