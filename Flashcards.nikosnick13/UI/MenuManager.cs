using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Flashcards.nikosnick13.Enums.Enums;


namespace Flashcards.nikosnick13.UI;

internal class MenuManager
{

    private readonly StackMenu _stackMenu;
    private readonly FlashcardMenu _flashcardMenu;
    private readonly StudyMenu _studyMenu;

    public MenuManager()
    {
        _stackMenu = new StackMenu();
        _flashcardMenu = new FlashcardMenu();
        _studyMenu = new StudyMenu();
    }

    public void ShowMainMenu()
    {
        bool isAppRunning = true;

        while (isAppRunning)
        {
            var mainMenu = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                .Title("What would you like to do?")
                .AddChoices(
                    MainMenuOptions.ManageStacks,
                    MainMenuOptions.ManageFlashcards,
                    MainMenuOptions.Study,
                    MainMenuOptions.Exit
                ));

            switch (mainMenu)
            {
                case MainMenuOptions.ManageStacks:
                    _stackMenu.ShowStackMenu();
                    break;
                case MainMenuOptions.ManageFlashcards:
                    _flashcardMenu.ShowFlashcartMenu();
                    break;
                case MainMenuOptions.Study:
                    _studyMenu.ShowStudyMenu();
                    break;
                case MainMenuOptions.Exit:
                    AnsiConsole.MarkupLine("[green]Goodbye![/]");
                    isAppRunning = false;
                    break;
            }
        }
    }
}
