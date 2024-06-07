using Flashcards.Models;

namespace Flashcards;

internal static class Helpers
{
    internal static string[] ConvertFlashcardsToArray(IEnumerable<Flashcard>? flashcards)
    {
       if(flashcards == null)
        {
            return Array.Empty<string>();
        }
        return flashcards.Select( s => s!.Front).ToArray();
    }

    internal static string[] ConvertStacksToArray(IEnumerable<Stack>? stacks)
    {
        if(stacks == null)
        {
            return Array.Empty<string>();
        }
        return stacks.Select( s => s!.StackName).ToArray();
    }
    
}