using System.ComponentModel;

namespace Flashcards;
public class Enums { }
public enum MainMenu
{
    [Description("Manage Stacks")]
    ManageStacks,

    [Description("Manage Flashcards")]
    ManageFlashcards,

    [Description("Start study session")]
    Study,

    [Description("Show Study Report")]
    ShowStudyReport,

    [Description("Quit Application")]
    Quit
}

public enum FlashcardMenu
{
    [Description("Add Flashcard")]
    AddFlashcard,

    [Description("Remove Flashcard")]
    RemoveFlashcard,
    [Description("Show Flashcards")]
    ShowFlashcard,

    [Description("Quit")]
    Quit
}

public enum StackMenu
{
    [Description("Add Stack")]
    AddStack,

    [Description("Remove Stack")]
    RemoveStack,

    [Description("Show Stacks")]
    ShowStack,

    [Description("Quit")]
    Quit
}