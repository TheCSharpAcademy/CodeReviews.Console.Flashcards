using Flashcards.Arashi256.Classes;
using Flashcards.Arashi256.Controllers;
using Flashcards.Arashi256.Models;
using Spectre.Console;

namespace Flashcards.Arashi256.Views
{
    internal class MainView
    {
        private const int QUIT_APPLICATION_OPTION_NUM = 10;
        private Table _tblMainMenu;
        private string _appTitle = "FLASHCARDS";
        private FigletText? _figletAppTitle;
        private StackView _stackView;
        private FlashcardView _flashcardView;
        private StudySessionView _studySessionView;
        private string[] _menuOptions =
        {
            "Add a new stack",
            "Update an existing stack",
            "Delete an existing stack",
            "List existing stacks",
            "List flashcards in stack",
            "Add new flashcard",
            "Update flashcard",
            "Delete flashcard",
            "Flashcard study sessions",
            "Exit application"
        };

        public MainView()
        {
            _figletAppTitle = new FigletText(_appTitle);
            _figletAppTitle.Centered();
            _figletAppTitle.Color = Color.Orange1;
            _tblMainMenu = new Table();
            _tblMainMenu.AddColumn(new TableColumn("[white]CHOICE[/]").Centered());
            _tblMainMenu.AddColumn(new TableColumn("[white]OPTION[/]").LeftAligned());
            for (int i = 0; i < _menuOptions.Length; i++)
            {
                _tblMainMenu.AddRow($"[yellow]{i + 1}[/]", $"[darkorange]{_menuOptions[i]}[/]");
            }
            _tblMainMenu.Alignment(Justify.Center);
            // Sub-views init.
            _stackView = new StackView();
            _flashcardView = new FlashcardView(_stackView);
            _studySessionView = new StudySessionView(_stackView, _flashcardView.FlashcardController);
        }

        public void DisplayMainMenu()
        {
            int selectedValue = 0;
            do
            {
                Console.Clear();
                AnsiConsole.Write(_figletAppTitle);
                AnsiConsole.Write(new Text("M A I N   M E N U").Centered());
                AnsiConsole.Write(_tblMainMenu);
                selectedValue = CommonUI.MenuOption($"Enter a value between 1 and {_menuOptions.Length}: ", 1, _menuOptions.Length);
                ProcessMainMenu(selectedValue);
            } while (selectedValue != QUIT_APPLICATION_OPTION_NUM);
            AnsiConsole.MarkupLine("[gold1]Goodbye![/]");
        }

        private void ProcessMainMenu(int option)
        {
            AnsiConsole.Markup($"[white]Menu option selected: {option}[/]\n");
            switch (option)
            {
                case 1:
                    // Add new stack.
                    _stackView.AddNewStack();
                    CommonUI.Pause("orange4_1");
                    break;
                case 2:
                    // Update an existing stack.
                    _stackView.UpdateStack();
                    CommonUI.Pause("orange4_1");
                    break;
                case 3:
                    // Delete existing stack.
                    _stackView.DeleteStack();
                    CommonUI.Pause("orange4_1");
                    break;
                case 4:
                    // List existing stacks.
                    _stackView.ViewStacks();
                    CommonUI.Pause("orange4_1");
                    break;
                case 5:
                    _flashcardView.ViewFlashcardsInStack();
                    CommonUI.Pause("orange4_1");
                    break;
                case 6:
                    // Add new Flashcard to Stack.
                    _flashcardView.AddNewFlashcard();
                    CommonUI.Pause("orange4_1");
                    break;
                case 7:
                    // Update flashcard.
                    _flashcardView.UpdateFlashcard();
                    CommonUI.Pause("orange4_1");
                    break;
                case 8:
                    // Delete flashcard.
                    _flashcardView.DeleteFlashcard();
                    CommonUI.Pause("orange4_1");
                    break;
                case 9:
                    // Study session area.
                    _studySessionView.DisplayStudySessionMenu();
                    break;
            }
        }
    }
}