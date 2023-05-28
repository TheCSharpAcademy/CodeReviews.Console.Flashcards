namespace FlashStudy.Utilities
{
  class Help
  {
    public static string mainMenu = @"
Main Menu
Type 1 to Study
Type 2 to Manage Stacks
Type 3 to Manage FlashCards
Type 4 to View Study Sessions
Type 0 to Exit";

    public static string stackMessage = @"
Manage Stacks
* show: to view stacks
* add [stack name]: to add stacks
* edit [old stack name] [new stack name]: to edit stacks
* remove [stack name]: to remove stacks
* Back or 0: go back to the main menu";

    public static string cardMessage = @"
Manage Flash Cards
* show: to view cards
* add: to add a flash card
* remove [card id]: to remove a card
* Back or 0: go back to the main menu";

    public static string stackAddErrorMessage = @"
add commands should be in the form 'add [stack name]'.
Example: 'add math' creates a stack with name 'math'";

    public static string stackRemoveErrorMessage = @"
remove commands should be in the form 'remove [stack name]'.
Example: 'remove math' removes the stack with name 'math'";

    public static string stackEditErrorMessage = @"
edit commands should be in the form 'edit [old stack name] [new stack name]'.
Example: 'edit grammar english' changes the stack with name 'grammar' to 'english'";

    public static string cardRemoveErrorMessage = @"
remove commands should be in the form of 'remove [stack name]'.
Example: 'remove biology' deletes the stack with the name 'biology'.";

  }
}
