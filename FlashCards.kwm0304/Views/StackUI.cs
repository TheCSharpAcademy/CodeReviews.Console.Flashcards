using FlashCards.kwm0304.Dtos;
using FlashCards.kwm0304.Models;
using FlashCards.kwm0304.Services;
using Spectre.Console;

namespace FlashCards.kwm0304.Views;

public class StackUI
{
  private readonly StackService _service;
  private readonly FlashCardUI _flashCardUI;
  public StackUI()
  {
    _service = new StackService();
    _flashCardUI = new FlashCardUI();
  }
  public async void HandleStack()
  {
    while (true)
    {
      Console.Clear();
      string choice = SelectionPrompt.StacksMenu();
      switch (choice)
      {
        case "Create":
          await CreateStackAsync();
          break;
        case "Edit":
          await EditStack();
          break;
        case "Delete":
          await DeleteStackAsync();
          break;
      }
    }
  }

  private async Task CreateStackAsync()
  {
    string stackName = AnsiConsole.Ask<string>("What name do you want to give this stack of flashcards?");
    int stackId = await _service.CreateStackAsync(stackName);
    Stack createdStack = await _service.GetStackAsync(stackId);
    bool createFlashcard = AnsiConsole.Confirm("Do you want to add a flashcard to this stack?");
    while (createFlashcard)
    {
      FlashCard card = _flashCardUI.AddFlashCard(stackId);
      createdStack.Flashcards.Add(card);
      createFlashcard = AnsiConsole.Confirm("Do you want to add another flashcard to this stack?");
    }
    await _service.UpdateStackAsync(stackId);
  }

  private async Task DeleteStackAsync()
  {
    List<StackDto> stacks = await _service.GetAllStacksAsync();
    StackDto stack = SelectionPrompt.StackSelection(stacks);
    bool confirmDelete = AnsiConsole.Confirm($"Are you sure you want to delete {stack.Name}?");
    if (confirmDelete)
    {
      await _service.DeleteStackAsync(stack.Id);
    }
  }

  private async Task EditStack()
  {
    List<StackDto> stacks = await _service.GetAllStacksAsync();
    StackDto stack = SelectionPrompt.StackSelection(stacks);
    string editOption = SelectionPrompt.EditStackMenu();
    int id = stack.Id;
    HandleEditStack(editOption, id);
  }

  private void HandleEditStack(string option, int stackId)
  {
    switch (option)
    {
      case "Add a flashcard":
        _flashCardUI.AddFlashCard(stackId);
        break;
      case "Edit a flashcard":
        _flashCardUI.EditFlashCard(stackId);
        break;
      case "Delete a flashcard":
        _flashCardUI.DeleteFlashCard(stackId);
        break;
      case "Back":
        break;
      default:
        break;
    }
  }
}