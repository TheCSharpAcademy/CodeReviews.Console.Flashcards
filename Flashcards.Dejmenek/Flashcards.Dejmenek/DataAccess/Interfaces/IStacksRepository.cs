using Flashcards.Dejmenek.Models;

namespace Flashcards.Dejmenek.DataAccess.Interfaces;

internal interface IStacksRepository : IAddFlashcard
{
    Stack GetStack(string name);
    void DeleteStack(int stackId);
    void UpdateFlashcardInStack(int flashcardId, int stackId, string front, string back);
    int GetFlashcardsCountInStack(int stackId);
    void DeleteFlashcardFromStack(int flashcardId, int stackId);
    IEnumerable<Flashcard> GetFlashcardsByStackId(int stackId);
    void AddStack(string name);
    IEnumerable<Stack> GetAllStacks();
    bool StackExistsWithName(string name);
    bool StackExistsWithId(int stackId);
    bool HasStack();
    bool HasStackAnyFlashcards(int stackId);
}
