using Flash.ConfigureBeforeLaunching;
using Flash.Launching;

namespace Flash;
public static class Configuration
{
    public static string ConnectionString { get; } = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
}
class Program
{
    static void Main(string[] args)
    {
        ConfigureFlashCardDatabase.CreateFlashCardDatabase();

        Console.WriteLine("\nConfiguration Finished. Press Any Key To Launch MainMenu\n");
        Console.ReadLine();

        MainMenu.GetMainMenu();
    }
}



