using FlashCards.kwm0304.Interfaces;
using FlashCards.kwm0304.Models;

namespace FlashCards.kwm0304.Services;

public class FlashCardService : IFlashCardService
{
  public async Task CreateFlashcardAsync(int stackId, string question, string answer)
  {
    throw new NotImplementedException();
  }

  public async Task DeleteFlashcardAsync(int stackId, int orderId)
  {
    throw new NotImplementedException();
  }

  public async Task<List<FlashCard>> GetAllFlashcardsAsync(int stackId)
  {
    throw new NotImplementedException();
  }

  public async Task UpdateFlashcardAsync(int stackId, int orderId, string question, string answer)
  {
    throw new NotImplementedException();
  }
}