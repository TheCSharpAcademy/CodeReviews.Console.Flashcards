using Spectre.Console;

namespace Flash.Helper.MainHelper
{
    internal class ShowMainMenuOptions
    {
        internal static void GetShowMainMenuOptions()
        {
            var rows = new List<Text>(){
            new Text("-Type 0 to Exit", new Style(Color.Red, Color.Black)),
            new Text("-Type 1 to Manage Stacks", new Style(Color.Green, Color.Black)),
            new Text("-Type 2 to Manage Flashcards", new Style(Color.Blue, Color.Black)),
            new Text("-Type 3 to Study", new Style(Color.Purple, Color.Black)),
            new Text("-Type 4 to Study History", new Style(Color.Orange3, Color.Black)),
            new Text("-Type 5 to Delete a Stack",  new Style(Color.Aqua, Color.Black))
            };

            AnsiConsole.Write(new Rows(rows));
        }
    }
}
