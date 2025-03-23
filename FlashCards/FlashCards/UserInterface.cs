using Spectre.Console;
using System.Text.RegularExpressions;

namespace FlashCards
{
    internal class UserInterface
    {
        private MainMenuOption[] _mainMenuOptions = (MainMenuOption[])Enum.GetValues(typeof(MainMenuOption));
        private StackMenuOption[] _stackMenuOptions = (StackMenuOption[])Enum.GetValues(typeof(StackMenuOption));
        private FlashCardMenuOption[] _flashcardMenuOptions = (FlashCardMenuOption[])Enum.GetValues(typeof(FlashCardMenuOption));

        private Regex validString = new Regex("^[A-Za-z0-9]+(?: [A-Za-z0-9]+)*$");

        ///############################################################################# GENERAL
        public void ClearConsole()
        {
            Console.Clear();
            PrintApplicationHeader();
        }
        public void PrintApplicationHeader()
        {
            Console.WriteLine("Welcome to Flash card application");
            Console.WriteLine(new string('-',50));
        }
        public void PrintPressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        
        public MainMenuOption GetMainMenuSelection()
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

            return mainMenuInput;
        }
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
        public int GetCount()
        {
            var count = AnsiConsole.Prompt(
                new TextPrompt<int>("Please enter number of records you'd like to retrieve")
                .Validate(x => x>0)
                );

            return count;
           
        }
        public string GetNewText(string prompt)
        {
            var text = AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                .AllowEmpty()
                .Validate(x => validString.IsMatch(x))
                );

            return text;
        }
        ///########################################################################################################################### STACKS
        public void PrintStacks(List<CardStack> stacks)
        {
            var table = new Table();
            table.AddColumn("Stacks");

            foreach (var stack in stacks)
            {
                table.AddRow(stack.StackName);
            }

            AnsiConsole.Write(table);

        }
        public CardStack GetStack(List<CardStack> stacks)
        {
            var stack = AnsiConsole.Prompt(
                new SelectionPrompt<CardStack>()
                .WrapAround()
                .AddChoices(stacks)
                .UseConverter(x => x.StackName)
                
                );
            return stack;
        }
        ///############################################################################################################################ CARDS
        public void PrintCards(List<FlashCardDto> cards)
        {
            var table = new Table();
            table.AddColumns("Card ID", "Front Text", "Back Text");
            foreach (var card in cards)
            {
                table.AddRow(card.CardID.ToString(), card.FrontText, card.BackText);
            }
            AnsiConsole.Write(table);

        }
        public FlashCard GetNewCard()
        {
            FlashCard card = new FlashCard();
            card.FrontText = GetNewText("Please enter front text value (question): ");
            card.BackText = GetNewText("Please enter back text value (answer): ");
            return card;
        }
        public int GetCardID(List<FlashCardDto> cards)
        {
            PrintCards(cards);

            var cardId = AnsiConsole.Prompt(
                new TextPrompt<int>("Please select card: ")
                .AddChoices(cards.Select(x => x.CardID))
                .HideChoices()
                );
            return cardId;
        }

    }
}
