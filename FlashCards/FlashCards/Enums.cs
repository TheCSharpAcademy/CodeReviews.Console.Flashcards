/// <summary>
/// Represents menu choices in the Main menu
/// </summary>
internal enum MainMenuOption
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
internal enum StackMenuOption
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
internal enum FlashCardMenuOption
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
internal enum PivotFunction
{
    Average,
    Count,
    Sum
}