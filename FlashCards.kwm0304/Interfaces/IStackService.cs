using FlashCards.kwm0304.Dtos;

namespace FlashCards.kwm0304.Interfaces;

public interface IStackService
{
  Task<StackDto> GetStackAsync(int id);
  Task<List<StackDto>> GetAllStacksAsync();
  Task CreateStackAsync(string name);
  Task UpdateStackAsync(int id, string name);
  Task DeleteStackAsync(int id);
}
