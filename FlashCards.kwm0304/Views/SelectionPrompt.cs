using FlashCards.kwm0304.Models;

namespace FlashCards.kwm0304.Views;

public class SelectionPrompt
{
  public const string genericPrompt = "What would you like to do?";

  public static string MainMenu()
  {
    var menuOptions = new List<string> { "Go to stacks", "Study", "Reports", "Exit" };
    var menu = new PromptContainer<string>(genericPrompt, menuOptions);
    return menu.Show() ?? "Exit";
  }

  public static string StacksMenu()
  {
    var menuOptions = new List<string> { "Create Stack", "Edit Stack", "Delete Stack", "View Stack", "Back" };
    var menu = new PromptContainer<string>(genericPrompt, menuOptions);
    return menu.Show() ?? "Back";
  }

  public static string StudyMenu()
  {
    var menuOptions = new List<string> {"Study", "View all study sessions", "Back"};
    var menu = new PromptContainer<string>(genericPrompt, menuOptions);
    return menu.Show() ?? "Back";
  }

  public static string FlashcardEditOptionMenu()
  {
    var menuOptions = new List<string> {"Question", "Answer", "Back"};
    var menu = new PromptContainer<string>("What do you want to edit?", menuOptions);
    return menu.Show() ?? "Back";
  }

  public static string EditStackMenu()
  {
    var menuOptions = new List<string> {"Add a flashcard", "Edit a flashcard", "Delete a flashcard", "Change stack name", "Back"};
    var menu = new PromptContainer<string>(genericPrompt, menuOptions);
    return menu.Show() ?? "Back";
  }

  public static Stack? StudyMenu(List<Stack> stacks)
  {
    var menu = new PromptContainer<Stack>("Select a stack you would like to study: ", stacks);
    return menu.Show();
  }
  public static Stack? StackSelection(List<Stack> stacks)
  {
    var menu = new PromptContainer<Stack>("Which stack would you like to select?", stacks);
    return menu.Show();
  }
  public static FlashCard? FlashCardSelection(List<FlashCard> cards)
  {
    var menu = new PromptContainer<FlashCard>("Which flashcard would you like to select?", cards);
    return menu.Show();
  }

  public static string ReportsSelection()
  {
    var menuOptions = new List<string> {"View monthly score", "View monthly attempts", "Back"};
    var menu = new PromptContainer<string>(genericPrompt, menuOptions);
    return menu.Show() ?? "Back";
  }
}