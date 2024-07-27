using FlashCards.kwm0304.Models;
using FlashCards.kwm0304.Repositories;
using FlashCards.kwm0304.Views;
using Spectre.Console;

namespace FlashCards.kwm0304.Services;

public class FlashCardService
{
  private readonly FlashCardRepository _repository;
  public FlashCardService()
  {
    _repository = new FlashCardRepository();
  }
  public FlashCard AddFlashCard(int stackId)
  {
    string question = GetAndConfirmInput("front");
    string answer = GetAndConfirmInput("back");
    FlashCard card = new(question, answer, stackId);
    _repository.CreateFlashcard(card);
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

  public async Task DeleteFlashCard(int stackId)
  {
    List<FlashCard> flashcards = await _repository.GetAllFlashcardsAsync(stackId);
    FlashCard flashcard = SelectionPrompt.FlashCardSelection(flashcards);
    int id = flashcard.FlashCardId;
    bool confirmDelete = AnsiConsole.Confirm($"Are you sure you want to delete this flashcard?");
    DisplayFlashcard(flashcard);
    if (confirmDelete)
    {
      await _repository.DeleteFlashcardAsync(id);
    }
  }

  public async Task EditFlashCard(int stackId)
  {
    List<FlashCard> flashcards = await _repository.GetAllFlashcardsAsync(stackId);
    FlashCard flashcard = SelectionPrompt.FlashCardSelection(flashcards);
    Console.Clear();
    DisplayFlashcard(flashcard);
    string choice = SelectionPrompt.FlashcardEditOptionMenu();
    if (choice == "Question")
    {
      string newQuestion = GetAndConfirmInput("front");
      flashcard.Question = newQuestion;
    }
    else if (choice == "Answer")
    {
      string newAnswer = GetAndConfirmInput("back");
      flashcard.Answer = newAnswer;
    }
    await _repository.EditFlashardAsync(flashcard);
  }

  private static void DisplayFlashcard(FlashCard card)
  {
    AnsiConsole.WriteLine($"Front of card: {card.Question}\n Back of card: {card.Answer}");
  }
}