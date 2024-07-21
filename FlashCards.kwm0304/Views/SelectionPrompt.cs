using System.Collections;

namespace FlashCards.kwm0304.Views;

public class SelectionPrompt
{
  public const string genericPrompt = "What would you like to do?";

  public static string MainMenu()
  {
    var menuOptions = new List<string> { "Go to stacks", "Study", "Reports", "Exit" };
    var menu = new PromptContainer<string>(genericPrompt, menuOptions);
    return menu.Show();
  }

  public static string StacksMenu()
  {
    var menuOptions = new List<string> { "Create", "Edit", "Delete", "Back" };
    var menu = new PromptContainer<string>(genericPrompt, menuOptions);
    return menu.Show();
  }

  public static string EditStackMenu()
  {
    var menuOptions = new List<string> {"Add a flashcard", "Edit a flashcard", "Delete a flashcard", "Back"};
    var menu = new PromptContainer<string>(genericPrompt, menuOptions);
    return menu.Show();
  }

  public static Stack StudyMenu(List<Stack> stacks)
  {
    var title = "Select a stack you would like to study: ";
    var menu = new PromptContainer<Stack>(title, stacks);
    return menu.Show();
  }
}
