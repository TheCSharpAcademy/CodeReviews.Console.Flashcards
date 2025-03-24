using Flashcards.Models;
using Flashcards.Models.Dtos;
using Flashcards.shared;
using Flashcards.Views;

namespace Flashcards.Presenters;

public class ManageStacksPresenter(MainView view, MainRepository repository)
{
    public void ManageStacks()
    {
            var isMenuVisible = true;
        
            while (isMenuVisible)
            {
                var menuOption = view.ManageStacks.Show(repository.Stacks.GetAll());

                switch (menuOption)
                {
                    case ManageStacksOption.Back: isMenuVisible = false; break;
                    case ManageStacksOption.Create: HandleCreate(); break;
                    case ManageStacksOption.ChangeName: HandleChangeName(); break;
                    case ManageStacksOption.DeleteStack: HandleStackDeletion();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        
            view.Clear();
    }

    private void HandleCreate()
    {
        var stackName = view.Input.GetText("Enter stack name: ");
        repository.Stacks.Insert(new StackDto(stackName));
        view.ShowMessage("[green]New stack created![/]");
        view.PressKeyToContinue();
    }
    
    private void HandleChangeName()
    {
        var stack = view.Input.GetExistingEntity(repository.Stacks.GetAll(), "Enter stack Id for update: ");
        var newName = view.Input.GetText("Enter new name: ");
        repository.Stacks.Update(new StackDto(stack.Id, newName));
        view.ShowMessage("[green]Name updated[/]");
        view.PressKeyToContinue();
    }

    private void HandleStackDeletion()
    {
        var selectedStack = view.Input.GetExistingEntity(repository.Stacks.GetAll().ToList(), "Enter stack Id to delete: ");
        repository.Stacks.DeleteById(selectedStack.Id);
        repository.Flashcards.DeleteByStackId(selectedStack.Id);
        repository.Sessions.DeleteByStackId(selectedStack.Id);
        view.ShowSuccess("Stack deleted along with associated flashcards.");
        view.PressKeyToContinue();
    }
    
}