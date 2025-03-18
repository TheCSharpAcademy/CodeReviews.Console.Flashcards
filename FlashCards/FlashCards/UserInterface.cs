using Spectre.Console;

namespace FlashCards
{
    internal class UserInterface
    {
        private MainMenuOption[] _mainMenuOptions = (MainMenuOption[])Enum.GetValues(typeof(MainMenuOption));
        private StackMenuOption[] _stackMenuOptions = (StackMenuOption[])Enum.GetValues(typeof(MainMenuOption));
        private FlashCardMenuOption[] _flashcardMenuOptions = (FlashCardMenuOption[])Enum.GetValues(typeof(MainMenuOption));

        public void PrintMainMenu()
        {
            
            var mainMenuInput = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOption>()
                .WrapAround()
                .AddChoices(_mainMenuOptions)
                .UseConverter( x => x switch
                {
                    MainMenuOption.ManageStacks => "Manage Stacks",
                    MainMenuOption.ManageFlashCards => "Manage FlashCards",
                    MainMenuOption.Study => "Study",
                    MainMenuOption.ViewStudySessions => "View Study Sessions",
                    MainMenuOption.Exit => "Exit",
                    _ => throw new NotImplementedException("Invalid Main menu enum option")
                }));

            Console.WriteLine(mainMenuInput);
        }
    }
}
