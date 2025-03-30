using Spectre.Console;

class ManageStacksMenuController
{
    public static void Start()
    {
        Console.Clear();

        Enums.ManageStacksMenuOptions userInput = DisplayMenu.ManageStacksMenu();

        switch (userInput)
        {
            case Enums.ManageStacksMenuOptions.CREATESTACK:
                break;
            case Enums.ManageStacksMenuOptions.RENAMESTACK:
                break;
            case Enums.ManageStacksMenuOptions.DELETESTACK:
                break;
            case Enums.ManageStacksMenuOptions.EXIT:
                break;
        }
    }

    public static void CreateStack(){}
    public static void RenameStack(){}
    public static void DeleteStack(){}
}