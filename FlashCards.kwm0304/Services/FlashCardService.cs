using FlashCards.kwm0304.Dtos;
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
  public async Task<FlashCard> AddFlashCard(int stackId)
  {
    string question = GetAndConfirmInput("front");
    string answer = GetAndConfirmInput("back");
    FlashCard card = new(question, answer, stackId);
    await _repository.CreateFlashcard(card);
    return card;
  }

  public async Task<List<FlashCardDto>> MapFlashcardsToDtos(int stackId)
  {
    List<FlashCard> flashcards = await _repository.GetAllFlashcardsAsync(stackId);
    List<FlashCardDto> dtos = flashcards
    .Select((c, i) => new FlashCardDto(i + 1, c.Question, c.Answer)).ToList();
    return dtos;
  }

  public async Task DisplayFlashCards(int stackId)
  {
    List<FlashCardDto> dtos = await MapFlashcardsToDtos(stackId);
    if (dtos.Count == 0)
    {
      AnsiConsole.WriteLine("No flashcards to display.");
      return;
    }

    var flashCardOptions = dtos.Select(dto => $"{dto.FlashCardNumber}. {dto.Question}").ToList();
    var selectedFlashCardOption = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select a flashcard to view")
            .AddChoices(flashCardOptions)
    );

    var selectedFlashCard = dtos.FirstOrDefault(dto => $"{dto.FlashCardNumber}. {dto.Question}" == selectedFlashCardOption);
    if (selectedFlashCard != null)
    {
      AnsiConsole.WriteLine($"Question: {selectedFlashCard.Question}");
      AnsiConsole.WriteLine($"Answer: {selectedFlashCard.Answer}");
    }
    Console.WriteLine("\nPress any key to return to the main menu...");
    Console.ReadKey(true);
  }

  public async Task<List<FlashCard>> GetShuffledCardsAsync(int stackId)
  {
    List<FlashCard> allCards = await _repository.GetAllFlashcardsAsync(stackId);
    return ShuffleList(allCards);
  }

  private static string GetAndConfirmInput(string side)
  {
    string input = AnsiConsole.Ask<string>($"What do you want to put on the {side} of the flashcard?");
    if (!string.IsNullOrEmpty(input))
    {
      bool confirmQuestion = AnsiConsole.Confirm($"Do you want to change the {side} before proceeding? \n [bold blue]{input}[/]");
      if (!confirmQuestion)
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
    FlashCard? flashcard = SelectionPrompt.FlashCardSelection(flashcards);
    if (flashcard == null)
    {
      return;
    }
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
    FlashCard? flashcard = SelectionPrompt.FlashCardSelection(flashcards);
    if (flashcard == null)
    {
      return;
    }
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
    return;
  }

  private static void DisplayFlashcard(FlashCard card)
  {
    AnsiConsole.WriteLine($"Front of card: {card.Question}\n Back of card: {card.Answer}");
  }

  private static List<FlashCard> ShuffleList(List<FlashCard> flashcards)
  {
    Random random = new();
    return flashcards.OrderBy(x => random.Next()).ToList();
  }
}