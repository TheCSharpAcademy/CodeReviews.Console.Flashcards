/// <summary>
/// Represents menu choices in the Main menu
/// </summary>
enum MainMenuOption
{
    ManageStacks,
    ManageFlashCards,
    Study,
    ViewStudySessions,
    GetReport,
    Exit,
}
/// <summary>
/// Represents menu choices in the stack menu
/// </summary>
enum StackMenuOption
{
    ViewAllStacks,
    CreateNewStack,
    RenameStack,
    DeleteStack,
    ReturnToMainMenu
}
/// <summary>
/// Represents menu choices in the flash card menu
/// </summary>
enum FlashCardMenuOption
{
    ViewAllCards,
    ViewXCards,
    CreateNewFlashCard,
    UpdateFlashCard,
    DeleteFlashCard,
    SwitchStack,
    ReturnToMainMenu
}
/// <summary>
/// Represents pivot functions for report function
/// </summary>
enum PivotFunction
{
    Average,
    Count,
    Sum
}
