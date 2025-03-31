using Spectre.Console;

namespace FlashCards
{
    /// <summary>
    /// Represents a user interface for FlashCard App.
    /// Inherits from UserInterace
    /// Implements IFlashCardAppUi
    /// </summary>
    internal class FlashCardAppUi : UserInterface, IFlashCardAppUi
    {
        /// <summary>
        /// Collections of enumerable values used to menuSelection
        /// </summary>
        private MainMenuOption[] _mainMenuOptions = (MainMenuOption[])Enum.GetValues(typeof(MainMenuOption));
        private StackMenuOption[] _stackMenuOptions = (StackMenuOption[])Enum.GetValues(typeof(StackMenuOption));
        private FlashCardMenuOption[] _flashcardMenuOptions = (FlashCardMenuOption[])Enum.GetValues(typeof(FlashCardMenuOption));
        /// <inheritdoc/>
        public MainMenuOption GetMainMenuSelection()
        {
            var mainMenuInput = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOption>()
                .WrapAround()
                .AddChoices(_mainMenuOptions)
                .UseConverter(x => x switch
                {
                    MainMenuOption.ManageStacks => "Manage Stacks",
                    MainMenuOption.ManageFlashCards => "Manage FlashCards",
                    MainMenuOption.Study => "Study",
                    MainMenuOption.ViewStudySessions => "View Study Sessions",
                    MainMenuOption.Exit => "Exit",
                    MainMenuOption.GetReport => "Get Report",
                    _ => throw new NotImplementedException("Invalid Main menu enum option")
                }));

            return mainMenuInput;
        }
        /// <inheritdoc/>
        public StackMenuOption GetStackMenuSelection()
        {
            var stackMenuInput = AnsiConsole.Prompt(
                new SelectionPrompt<StackMenuOption>()
                .WrapAround()
                .AddChoices(_stackMenuOptions)
                .UseConverter(x => x switch
                {
                    StackMenuOption.ViewAllStacks => "View stacks",
                    StackMenuOption.CreateNewStack => "Create new stack",
                    StackMenuOption.RenameStack => "Rename stack",
                    StackMenuOption.DeleteStack => "Delete stack",
                    StackMenuOption.ReturnToMainMenu => "Return to main menu",
                    _ => throw new NotImplementedException("Invalid stack menu enum option")
                }));

            return stackMenuInput;
        }
        /// <inheritdoc/>
        public FlashCardMenuOption GetFlashCardMenuSelection()
        {
            var flashCardMenuInput = AnsiConsole.Prompt(
                new SelectionPrompt<FlashCardMenuOption>()
                .WrapAround()
                .AddChoices(_flashcardMenuOptions)
                .UseConverter(x => x switch
                {
                    FlashCardMenuOption.ViewAllCards => "View all cards",
                    FlashCardMenuOption.ViewXCards => "View X cards",
                    FlashCardMenuOption.CreateNewFlashCard => "Create new flash card",
                    FlashCardMenuOption.UpdateFlashCard => "Update flashcard",
                    FlashCardMenuOption.DeleteFlashCard => "Delete flashcard",
                    FlashCardMenuOption.SwitchStack => "Switch stack",
                    FlashCardMenuOption.ReturnToMainMenu => "Return to main menu",
                    _ => throw new NotImplementedException("Invalid flashcard menu enum option")
                }));

            return flashCardMenuInput;
        }
    }
}
