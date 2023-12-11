namespace Flashcards.UgniusFalze.Menu;

public class Menu
{
    private List<string> MenuOptions { get; set; }
    private List<Action> Functions { get; set; }
    private int? ExitOption { get; set; }
    private string MenuTitle { get; set; }

    public Menu(string menuTitle)
    {
        MenuTitle = menuTitle;
        MenuOptions = new List<string>();
        Functions = new List<Action>();
        ExitOption = null;
    }

    public Menu AddOption(string option, Action function)
    {
        MenuOptions.Add(option);
        Functions.Add(function);
        return this;
    }

    public Menu AddExitOption(string option, Action function)
    {
        ExitOption = MenuOptions.Count;
        return AddOption(option, function);
    }
    public void Display(bool initial = true)
    {
        string? option;
        do
        {
            Console.WriteLine(MenuTitle);
            for (int i = 0; i < MenuOptions.Count; i++)
            {
                Console.WriteLine((i + 1).ToString() + ". " + MenuOptions[i]);
            }

            int? optionId = DisplayController.GetOptionId(initial, MenuOptions.Count);
            if (optionId == 0)
            {
                return;
            }else if (optionId != null)
            {
                Functions[(int)optionId - 1]();
                if (ExitOption == ((int)optionId - 1))
                {
                    return;
                }
            }
        } while (true);
    }
}