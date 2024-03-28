using System.ComponentModel;

namespace Flashcards;
public class Enums { }
public enum MainMenu
{
    [Description("Manage stacks")]
    ManageStacks,

    [Description("Manage flashcards")]
    ManageFlashcards,

    [Description("Start study session")]
    Study,

    [Description("Show study Report")]
    ShowStudyReport,

    [Description("Quit application")]
    Quit
}

public enum FlashcardMenu
{
    [Description("Add flashcard")]
    AddFlashcard,

    [Description("Remove flashcard")]
    RemoveFlashcard,
    [Description("Show flashcards")]
    ShowFlashcard,

    [Description("Return to main menu")]
    Quit
}

public enum StackMenu
{
    [Description("Add stack")]
    AddStack,

    [Description("Remove stack")]
    RemoveStack,

    [Description("Show stacks")]
    ShowStack,

    [Description("Return to main menu")]
    Quit
}