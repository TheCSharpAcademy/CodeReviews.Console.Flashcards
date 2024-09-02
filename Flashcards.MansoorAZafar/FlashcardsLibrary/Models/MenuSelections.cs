namespace FlashcardsLibrary.Models;

internal enum HomeMenu
{
    exit = 0, 
    StackManager,
    FlashCardManager,
    Study
}

internal enum StackSelections
{
    exit = 0,
    ChangeCurrentStack,
    ViewAllFlashCardsInStack,
    ViewXFlashCardsInStack,
    CreateFlashCardInStack,
    EditFlashCard,
    DeleteFlashCard
}

internal enum StackHomeSelections
{
    exit = 0,
    InputStack,
    CreateStack,
    DeleteStack
}