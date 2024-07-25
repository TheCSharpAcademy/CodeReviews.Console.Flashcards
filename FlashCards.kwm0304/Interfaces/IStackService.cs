using FlashCards.kwm0304.Dtos;
using FlashCards.kwm0304.Models;

namespace FlashCards.kwm0304.Interfaces;

public interface IStackService
{
  Task<Stack> GetStackAsync(int id);
  Task<List<StackDto>> GetAllStacksAsync();
  Task<int> CreateStackAsync(string name);
  Task UpdateStackAsync(int id);
  Task DeleteStackAsync(int id);
}
