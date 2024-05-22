using Flashcards.kalsson.DTOs;
using Flashcards.kalsson.Models;

namespace Flashcards.kalsson.Interfaces;

public interface IStackRepository
{
    Task<IEnumerable<StackDTO>> GetAllStacksAsync();
    Task<int> AddStackAsync(StackDTO stack);
    Task DeleteStackAsync(int stackId);
}