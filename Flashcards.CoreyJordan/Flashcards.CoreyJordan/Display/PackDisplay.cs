using ConsoleTableExt;

namespace Flashcards.CoreyJordan.Display;
internal class PackDisplay
{
    private UserInterface UI { get; set; } = new();
    public InputModel User { get; set; } = new();
    private string[] Menu { get; } =
    {
        "N: New Study Deck",
        "E: Edit Packlist",
        "R: Rename Pack",
        "D: Delete Deck",
        "Q: Exit Pack Manager"
    };

    internal string NamePack()
    {
        UI.TitleBar("NEW PACK");

        string name = User.GetString("Enter a name for this deck: ");
        while (name.Length == 0 || name.Contains(' '))
        {
            UI.Prompt("Name cannot be blank or contain spaces.");
            name = User.GetString("Enter a name for this deck: ");
        }

        return name;
    }

    internal void DisplayMenu()
    {
        UI.TitleBar("PACK MANAGER");

        foreach (string s in Menu)
        {
            Console.WriteLine($"\t{s}");
        }
        Console.WriteLine();
    }

    internal void DisplayDecks<T>(List<T> decks) where T : class
    {
        ConsoleTableBuilder
            .From(decks)
            .ExportAndWriteLine();
    }
}
