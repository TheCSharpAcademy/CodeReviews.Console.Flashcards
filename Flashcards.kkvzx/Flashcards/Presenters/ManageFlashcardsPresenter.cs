using Flashcards.Models;
using Flashcards.Models.Dtos;
using Flashcards.shared;
using Flashcards.Views;

namespace Flashcards.Presenters;

public class ManageFlashcardsPresenter(MainView view, MainRepository repository)
{
    private StackDto SelectedStack { get; set; } = new();
    private List<StackDto> Stacks { get; set; } = [];
    private List<FlashcardDto> Flashcards { get; set; } = [];

    public void ManageFlashcards()
    {
        var isMenuVisible = true;
        Stacks = repository.Stacks.GetAll();

        if (Stacks.Count == 0)
        {
            view.ShowMessage("[red]Currently theres are no stacks with flashcards.[/]\nCreate stack first.\n");
            view.PressKeyToContinue();
            return;
        }

        SelectedStack = view.SelectStack(Stacks);

        while (isMenuVisible)
        {
            Flashcards = repository.Flashcards.GetFlashcardsByStack(SelectedStack.Id);
            var menuOption =
                view.ManageFlashcards.ShowMenu(Flashcards, SelectedStack.Name);

            switch (menuOption)
            {
                case ManageFlashcardsOption.BackToMenu: isMenuVisible = false; break;
                case ManageFlashcardsOption.ChangeStack: HandleChangeStack(); break;
                case ManageFlashcardsOption.Create: HandleCreate(); break;
                case ManageFlashcardsOption.Update: HandleUpdate(); break;
                case ManageFlashcardsOption.Delete: HandleDeletion(); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        view.Clear();
    }

    private void HandleChangeStack()
    {
        SelectedStack = view.SelectStack(Stacks);
    }

    private void HandleCreate()
    {
        var flashcard = view.ManageFlashcards.GetNewFlashcard(SelectedStack.Id);
        repository.Flashcards.Insert(flashcard);
        view.ShowSuccess("Flashcard created");
        view.PressKeyToContinue();
    }

    private void HandleUpdate()
    {
        var selectedFlashcard = view.Input.GetExistingEntity(Flashcards, "Enter flashcard Id to update: ");
        var updatedFlashcard = view.ManageFlashcards.GetNewFlashcard(selectedFlashcard);
        
        repository.Flashcards.Update(updatedFlashcard);
    }

    private void HandleDeletion()
    {
        var selectedFlashcard = view.Input.GetExistingEntity(Flashcards, "Enter flashcard Id to delete: ");
        repository.Flashcards.DeleteById(selectedFlashcard.Id);
        view.ShowSuccess("Flashcard deleted");
        view.PressKeyToContinue();
    }
}