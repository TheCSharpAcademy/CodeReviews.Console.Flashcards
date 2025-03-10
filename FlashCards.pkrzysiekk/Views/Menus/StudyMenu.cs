using FlashCards.Controllers;
using FlashCards.Models;
using FlashCards.Models.FlashCards;
using FlashCards.Models.MenuEnums;
using FlashCards.Models.Stack;
using Spectre.Console;

namespace FlashCards.Views.Menus
{
    public class StudyMenu : IMenu
    {
        private StackController _stackController = new();
        private FlashCardsController _flashCardsController = new();
        private StudyController _studyController = new();
        private SessionController _sessionController = new();

        private StudyMenuEnum GetUserChoice()
        {
            var menuChoice = AnsiConsole.Prompt(new SelectionPrompt<StudyMenuEnum>()
                .Title("[Purple]What would you want to do? [/]")
                .AddChoices(Enum.GetValues<StudyMenuEnum>())
                .UseConverter(x => x switch
                {
                    StudyMenuEnum.SelectStackForStudy => "Select Stack for study",
                    StudyMenuEnum.Statistics => "Statistics",
                    StudyMenuEnum.Exit => "Exit",
                    _ => throw new NotImplementedException()
                }
                )
                );
            return menuChoice;
        }

        public void Show()
        {
            IEnumerable<StackBO> stackList = new LinkedList<StackBO>();
            StackBO stack = new StackBO();
            IEnumerable<FlashCardBO> cards = new List<FlashCardBO>();
            SessionBO session = new SessionBO();
            while (true)
            {
                StudyMenuEnum choice = GetUserChoice();
                AnsiConsole.Clear();

                switch (choice)
                {
                    case StudyMenuEnum.SelectStackForStudy:
                        stackList = _stackController.GetAllBO();
                        if (stackList.Count() == 0)
                        {
                            AnsiConsole.MarkupLine("[White]No stacks avaliable, create one first[/]");
                            continue;
                        }
                        stack = _stackController.GetUserSelection(stackList);
                        cards = _flashCardsController.GetAllCardsFromStack(stack);
                        session = _studyController.StartSession(cards);
                        AnsiConsole.MarkupLine($"[Green]Your score is {session.Score}/{session.MaxScore}[/]");
                        _sessionController.Insert(stack, session);
                        break;

                    case StudyMenuEnum.Statistics:
                        var stacks = _stackController.GetAllBO();
                        if (stacks.Count() == 0)
                        {
                            AnsiConsole.MarkupLine("[White]No stacks avaliable, create one first[/]");
                            continue;
                        }
                        var selectedStack = _stackController.GetUserSelection(stacks);
                        var statisticsList = _sessionController.GetStatistics(selectedStack);
                        SessionView.ShowStatistics(statisticsList);
                        break;

                    case StudyMenuEnum.Exit:
                        return;
                }
            }
        }
    }
}