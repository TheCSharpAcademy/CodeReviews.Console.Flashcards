using FlashCardApp.DTO;
using FlashCardApp.Models;

namespace FlashCardApp.Data;

public interface IDatabaseManager
{
    public void CreateStackTable();

    public void CreateFlashCardTable();

    public void CreateStudyAreaTable();

    // Stack Operations
    public void AddNewStack(Stack newStack);

    public void UpdateStack(Stack oldStack, Stack newStack);

    public void DeleteStack(Stack stackToDelete);

    public List<StackDTO> GetStacks();
    
    // FlashCard Operations
    public void AddNewFlashCard(FlashCard flashCard, Stack stack);

    public void UpdateFlashCard(FlashCard oldFlashCard, FlashCard newFlashCard, Stack stack);

    public void UpdateFlashCardName(FlashCard oldFlashCard, FlashCard newFlashCard, Stack stack);

    public void UpdateFlashCardContent(FlashCard oldFlashCard, FlashCard newFlashCard, Stack stack);

    public void DeleteFlashCard(FlashCard flashCardToDelete, Stack stack);

    public List<FlashCardDTO> GetFlashCardsOfStack(Stack stack);
    
    // StudyArea Operations
    public void SaveScore(StudyArea studyArea, Stack stack);

    public List<StudyAreaDto> GetScoresHistory();
}