using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.nikosnick13.Enums;

internal class Enums
{
    public enum MainMenuOptions
    {
        ManageStacks,
        ManageFlashcards,
        Study,
        Exit
    }

    public enum StacksMenuOptions
    {
        AddStack,
        ViewAllStacks,
        ViewStack,
        EditStacks,
        DeleteStack,
        ReturnToMainMenu
    }
    public enum FlashcardsMenuOptions
    {
        AddFlashcards,
        ViewAllFlashcards,
        ViewFlashcard,
        EditFlashcard,
        DeleteFlashcard,
        ReturnToMainMenu
    }
    public enum StudyMenuOptions 
    {
        StartStudySession,
        ViewStudySessions,
        ReturnToMainMenu
    }
}
