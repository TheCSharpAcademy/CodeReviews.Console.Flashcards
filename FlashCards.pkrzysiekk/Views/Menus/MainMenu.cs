using FlashCards.Controllers;
using FlashCards.Models;
using FlashCards.Models.MenuEnums;
using Spectre.Console;

namespace FlashCards.Views.Menus
{
    public class MainMenu : IMenu
    {
        private MainMenuEnum _mainMenuEnum;
        private IMenu _stackMenu = new StackMenu();
        private IMenu _flashCardMenu = new FlashCardMenu();
        private IMenu _studyMenu = new StudyMenu();
        private SessionController _sessionController = new SessionController();

        public MainMenuEnum GetUserChoice()
        {
            var menuChoice = AnsiConsole.Prompt(new SelectionPrompt<MainMenuEnum>()
               .Title("[Teal]What would you want to do? [/]")
               .AddChoices(Enum.GetValues<MainMenuEnum>())
               .UseConverter(x => x switch
               {
                   MainMenuEnum.Menage_Stacks => "Menage Stacks",
                   MainMenuEnum.Menage_FlashCards => "Menage FlashCards",
                   MainMenuEnum.Study => "Study",
                   MainMenuEnum.View_StudySessions => "View StudySessions",
                   MainMenuEnum.Exit => "Exit",
                   _ => throw new NotImplementedException()
               })
               );
            return menuChoice;
        }

        public void Show()
        {
            while (true)
            {
                MainMenuEnum choice = GetUserChoice();
                AnsiConsole.Clear();
                switch (choice)
                {
                    case MainMenuEnum.Menage_Stacks:
                        _stackMenu.Show();
                        break;

                    case MainMenuEnum.Menage_FlashCards:
                        _flashCardMenu.Show();
                        break;

                    case MainMenuEnum.Study:
                        _studyMenu.Show();
                        break;

                    case MainMenuEnum.View_StudySessions:
                        var sessions = _sessionController.GetAll();
                        if (sessions.Count() == 0)
                        {
                            AnsiConsole.MarkupLine("[White]No sessions, study first![/]");
                            continue;
                        }
                        SessionView.ShowSessions(sessions);
                        break;

                    case MainMenuEnum.Exit:
                        return;
                }
            }
        }
    }
}