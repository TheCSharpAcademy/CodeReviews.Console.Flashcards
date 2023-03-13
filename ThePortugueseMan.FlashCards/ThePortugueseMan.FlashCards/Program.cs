using DbCommandsLibrary;
using Screens;

public class Program
{
    static void Main() 
    {
        var dbCmds = new DbCommands();
        dbCmds.Initialize.AllTables();

        var screen = new Screen();
        screen.MainMenu.View();
    }
}