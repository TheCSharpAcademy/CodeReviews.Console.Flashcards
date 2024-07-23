using Flashcards.Controllers;
using Flashcards.Models;

namespace Flashcards.Services;

/// <summary>
/// Helper service for generating seed data for the database.
/// </summary>

internal static class SeedDataService
{
    #region Methods - Internal

    internal static Dictionary<string, List<FlashcardDto>> GenerateStackSeedData()
    {
        return new Dictionary<string, List<FlashcardDto>>
        {
            { "French", GenerateFrenchStack() },
            { "German", GenerateGermanStack() },
            { "Italian", GenerateItalianStack() },
            { "Spanish", GenerateSpanishStack() }
        };
    }

    internal static List<DateTime> GenerateStudySessionDatesSeedData()
    {
        var output = new List<DateTime>();

        // Generate a hundred random study session datetimes within this and the last year.
        var startOfLastYear = new DateTime(DateTime.Now.Year - 1, 1, 1);
        int range = (DateTime.Now - startOfLastYear).Days;
        for (int i = 0; i < 100; i++)
        {
            var dateTime = startOfLastYear.AddDays(Random.Shared.Next(range)).AddHours(Random.Shared.Next(0, 24)).AddMinutes(Random.Shared.Next(0, 60)).AddSeconds(Random.Shared.Next(0, 60));
            output.Add(dateTime);
        }

        // Order by date
        output.Sort();

        return output;
    }

    #endregion
    #region Methods - Private

    private static List<FlashcardDto> GenerateFrenchStack()
    {
        return
        [
            new(0, "Hello", "Bonjour"),
            new(0, "My name is", "Mon nom est"),
            new(0, "Please", "S il vous plait"),
            new(0, "Thank you", "Merci"),
            new(0, "Sorry", "Pardon"),
            new(0, "Yes", "Oui"),
            new(0, "No", "Non"),
            new(0, "Good", "Bien"),
            new(0, "Bad", "Mal")
        ];
    }

    private static List<FlashcardDto> GenerateGermanStack()
    {
        return
        [
            new(0, "Hello", "Hallo"),
            new(0, "My name is", "Ich heisse"),
            new(0, "Please", "Bitte"),
            new(0, "Thank you", "Vielen Dank"),
            new(0, "Sorry", "Es tut uns leid"),
            new(0, "Yes", "Ja"),
            new(0, "No", "Nein"),
            new(0, "Good", "Gut"),
            new(0, "Bad", "Schlecht")
        ];
    }

    private static List<FlashcardDto> GenerateItalianStack()
    {
        return
        [
            new(0, "Hello", "Ciao"),
            new(0, "My name is", "Il mio nome e"),
            new(0, "Please", "Per favore"),
            new(0, "Thank you", "Grazie"),
            new(0, "Sorry", "Scusate"),
            new(0, "Yes", "Si"),
            new(0, "No", "No"),
            new(0, "Good", "Bene"),
            new(0, "Bad", "Male")
        ];
    }

    private static List<FlashcardDto> GenerateSpanishStack()
    {
        return
        [
            new(0, "Hello", "Hola"),
            new(0, "My name is", "Me llamo"),
            new(0, "Please", "Por favor"),
            new(0, "Thank you", "Gracias"),
            new(0, "Sorry", "Lo siento"),
            new(0, "Yes", "Si"),
            new(0, "No", "No"),
            new(0, "Good", "Bueno"),
            new(0, "Bad", "Malo")
        ];
    }

    #endregion
}
