using DbCommandsLibrary;
using Screens;

public class Program
{
    static void Main() 
    {
        var dbCmds = new DbCommands();
        dbCmds.Initialize.AllTables();

        var mainMenu = new MainMenu();
        mainMenu.View();

        Console.Clear();
        Console.WriteLine("Goodbye!");
        Environment.Exit(0);
    }
}