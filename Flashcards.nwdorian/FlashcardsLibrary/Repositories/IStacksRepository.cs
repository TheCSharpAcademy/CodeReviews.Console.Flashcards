using FlashcardsLibrary.Models;

namespace FlashcardsLibrary.Repositories;
public interface IStacksRepository
{
    Task<IEnumerable<Stack>> GetAllAsync();
    Task AddAsync(Stack stack);
    Task UpdateAsync(Stack stack);
    Task DeleteAsync(Stack stack);
    Task<bool> StackNameExistsAsync(string name);
}
