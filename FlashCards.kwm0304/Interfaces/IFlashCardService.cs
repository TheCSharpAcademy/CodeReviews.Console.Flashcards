using FlashCards.kwm0304.Models;

namespace FlashCards.kwm0304.Interfaces;

public interface IFlashCardService
{
  Task CreateFlashcardAsync(int stackId, string question, string answer);
  Task UpdateFlashcardAsync(int stackId, int orderId, string question, string answer);
  Task DeleteFlashcardAsync(int stackId, int orderId);
  Task<List<FlashCard>> GetAllFlashcardsAsync(int stackId);
}
