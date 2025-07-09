using DotNETConsole.Flashcards.Database;
using DotNETConsole.Flashcards.Controllers;

public class Program
{
    public static void Main(string[] args)
    {
        // Load Env
        DotNetEnv.Env.Load();
        var db = new DbContext();
        var migration = new Migaration();
        migration.Up();
        var menu = new MenuController();
        menu.MainMenu();
    }
}
