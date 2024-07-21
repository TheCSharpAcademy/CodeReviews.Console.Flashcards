using FlashCards.kwm0304.Interfaces;

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

  public async Task UpdateFlashcardAsync(int stackId, int orderId, string question, string answer)
  {
    throw new NotImplementedException();
  }
}