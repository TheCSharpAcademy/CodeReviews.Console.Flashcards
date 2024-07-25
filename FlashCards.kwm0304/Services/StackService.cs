using FlashCards.kwm0304.Dtos;
using FlashCards.kwm0304.Interfaces;
using FlashCards.kwm0304.Models;
using FlashCards.kwm0304.Repositories;

namespace FlashCards.kwm0304.Services;

public class StackService : IStackService
{
  private readonly StackRepository _repository;
  public StackService()
  {
    _repository = new StackRepository();
  }
  public async Task<int> CreateStackAsync(string name)
  {
    //fill out repository methods
    return _repository.CreateStackAsync(name);
  }

  public async Task DeleteStackAsync(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<List<StackDto>> GetAllStacksAsync()
  {
    throw new NotImplementedException();
  }

  public async Task<Stack> GetStackAsync(int id)
  {
    throw new NotImplementedException();
  }

  public async Task UpdateStackAsync(int id)
  {
    // pass generic field depending on what i update
    await _repository.UpdateStackAsync(id);
  }

  public async Task AddFlashCardToStackAsync(FlashCard card)
  {
    _repository.AddFlashCardToStackAsync(card);
  }

}