using FlashCards.kwm0304.Models;
using FlashCards.kwm0304.Services;
using Spectre.Console;

namespace FlashCards.kwm0304.Views;

public class FlashCardUI
{
  private readonly FlashCardService _service;
  public FlashCardUI()
  {
    _service = new FlashCardService();
  }
  public FlashCard AddFlashCard(int stackId)
  {
    string question = GetAndConfirmInput("front");
    string answer = GetAndConfirmInput("back");
    FlashCard card = new(question, answer, stackId);
    return card;
  }
  private static string GetAndConfirmInput(string side)
  {
    string input = AnsiConsole.Ask<string>($"What do you want to put on the {side} of the flashcard?");
    if (!string.IsNullOrEmpty(input))
    {
      bool confirmQuestion = AnsiConsole.Confirm($"Do you want to change the front before proceeding? \n [bold blue]{input}[/]");
      if (confirmQuestion)
      {
        return input;
      }
      else
        return GetAndConfirmInput(side);
    }
    AnsiConsole.WriteLine("Flashcard cannot be blank, please try again");
    return GetAndConfirmInput(side);
  }

  public async void DeleteFlashCard(int stackId)
  {
    List<FlashCard> flashcards = await _service.GetAllFlashcardsAsync(stackId);
    FlashCard flashcard = SelectionPrompt.FlashCardSelection(flashcards);
    int id = flashcard.FlashCardId;
    bool confirmDelete = AnsiConsole.Confirm($"Are you sure you want to delete this flashcard?");
    if (confirmDelete)
    {
      await _service.DeleteFlashcardAsync(stackId, id);
    }
  }

  public void EditFlashCard(int stackId)
  {
    throw new NotImplementedException();
  }
}
