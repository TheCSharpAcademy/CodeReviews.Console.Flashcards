using FlashCards.kwm0304.Dtos;
using FlashCards.kwm0304.Models;
using FlashCards.kwm0304.Repositories;
using FlashCards.kwm0304.Views;
using Spectre.Console;

namespace FlashCards.kwm0304.Services;

public class StackService
{
  private readonly StackRepository _repository;
  private readonly FlashCardService _flashcardService;
  public StackService()
  {
    _repository = new StackRepository();
    _flashcardService = new FlashCardService();
  }
  public async Task HandleStack()
  {
    while (true)
    {
      Console.Clear();
      string choice = SelectionPrompt.StacksMenu();
      switch (choice)
      {
        case "Create Stack":
          await CreateStackAsync();
          break;
        case "Edit Stack":
          await EditStack();
          break;
        case "Delete Stack":
          await DeleteStackAsync();
          break;
        case "Back":
          return;
        default:
          break;
      }
    }
  }

  private async Task CreateStackAsync()
  {
    string stackName = AnsiConsole.Ask<string>("What name do you want to give this stack of flashcards?");
    int stackId = await _repository.CreateStackAsync(stackName);
    Stack createdStack = await _repository.GetStackAsync(stackId);
    bool createFlashcard = AnsiConsole.Confirm("Do you want to add a flashcard to this stack?");
    while (createFlashcard)
    {
      FlashCard card = await _flashcardService.AddFlashCard(stackId);
      createdStack.Flashcards.Add(card);
      createFlashcard = AnsiConsole.Confirm("Do you want to add another flashcard to this stack?");
    }
  }


  private async Task DeleteStackAsync()
  {
    List<Stack> stacks = await _repository.GetAllStacksAsync();
    Stack? stack = SelectionPrompt.StackSelection(stacks);
    if (stack == null)
    {
      return;
    }
    int id = stack.StackId;
    string name = stack.StackName ?? string.Empty;
    bool confirmDelete = AnsiConsole.Confirm($"Are you sure you want to delete {name}?");
    if (confirmDelete)
    {
      await _repository.DeleteStackAsync(id);
    }
  }

  private async Task EditStack()
  {
    List<Stack> stacks = await GetAllStacks();
    Stack? stack = SelectionPrompt.StackSelection(stacks);
    if (stack == null)
    {
      return;
    }
    int id = stack.StackId;
    await HandleEditStack(id);
  }
  public async Task<List<Stack>> GetAllStacks()
  {
    List<Stack> stacks = await _repository.GetAllStacksAsync();
    return stacks;
  }

  private static string ChangeStackName()
  {
    string answer = AnsiConsole.Ask<string>("What would you like to change the stack's name to?");
    if (!string.IsNullOrEmpty(answer))
    {
      return answer;
    }
    else
    {
      return ChangeStackName();
    }
  }
  private async Task HandleEditStack(int stackId)
  {
    while (true)
    {
      string option = SelectionPrompt.EditStackMenu();
      switch (option)
      {
        case "Add a flashcard":
          await _flashcardService.AddFlashCard(stackId);
          break;
        case "Edit a flashcard":
          await _flashcardService.EditFlashCard(stackId);
          break;
        case "Delete a flashcard":
          await _flashcardService.DeleteFlashCard(stackId);
          break;
        case "Change stack name":
          string newName = ChangeStackName();
          await _repository.UpdateStackAsync(stackId, newName);
          break;
        case "Back":
          return;
        default:
          break;
      }
    }
  }
}