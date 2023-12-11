using Flashcards.UgniusFalze;using Flashcards.UgniusFalze.Menu;


try
{
    string? connString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnString");
    if (connString == null)
    {
        Console.WriteLine("Connection string is missing");
        return;
    }
    Driver driver = new Driver(connString);

    DisplayController displayController = new DisplayController(driver);
    new Menu("Choose what you want to do")
        .AddOption("Manage stacks", () => displayController.ManageStacks())
        .AddOption("Add stack", () => displayController.AddStack())
        .AddOption("Check study sessions", () => displayController.DisplaySessions())
        .AddOption("Study", () => displayController.Study())
        .Display();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
