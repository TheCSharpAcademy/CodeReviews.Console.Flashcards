using FlashcardsLibrary;
public class Program
{
    private static bool exit;

    private static void Main(string[] args)
    {
        DatabaseQueries.Run.CreateTablesIfNotExist();
        while (!exit)
        {
            exit = UserInput.SwitchMenuChoice(UserInput.ShowMenu());
        }
    }
}