using DatabaseLibrary.Models;
using Spectre.Console;

namespace DatabaseLibrary.Helpers;

public class FlashcardIDValidator
{
  public static bool IsValid(List<FlashcardDTO> flashcards, int displayId)
  {
    if (flashcards.FirstOrDefault(flashcard => flashcard.Display_Id == displayId) == null)
    {
      AnsiConsole.Markup("[red]There is no flashcard with given ID. [/]");
      return false;
    }

    return true;
  }
}