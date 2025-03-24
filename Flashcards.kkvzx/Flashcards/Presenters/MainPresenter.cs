using Flashcards.Models;
using Flashcards.shared;
using Flashcards.Views;

namespace Flashcards.Presenters;

public class MainPresenter(MainView view, MainRepository repository)
{
    private readonly ManageStacksPresenter _manageStacksPresenter = new (view, repository);
    private readonly ManageFlashcardsPresenter _manageFlashcardPresenter = new (view, repository);
    private readonly LearnPresenter _learnPresenter = new(view, repository);
    
    public void Run()
    {
        repository.Init();

        while (true)
        {
            var menuOption = view.ShowMenu();

            switch (menuOption)
            {
                case MenuOption.Exit: return;
                case MenuOption.Seed: HandleSeed(); break;
                case MenuOption.DeleteData: HandleDeleteData(); break;
                case MenuOption.ManageStacks: _manageStacksPresenter.ManageStacks(); break;
                case MenuOption.ManageFlashcards: _manageFlashcardPresenter.ManageFlashcards(); break;
                case MenuOption.Learn: _learnPresenter.Learn(); break;
                case MenuOption.ShowSessions: ShowSessions(); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void HandleSeed()
    {
        repository.Seeder.Seed();
        view.ShowMessage("Seeded data successfully.");
        view.PressKeyToContinue();
    }

    private void HandleDeleteData()
    {
        repository.DeleteData();
        repository.InitTables();
        view.ShowMessage("Dropped tables successfully.");
        view.PressKeyToContinue();
    }

    private void ShowSessions()
    {
        view.Clear();
        view.DisplaySessions(repository.Sessions.GetAll(), repository.Stacks.GetAll());
        view.PressKeyToContinue();
    }
}