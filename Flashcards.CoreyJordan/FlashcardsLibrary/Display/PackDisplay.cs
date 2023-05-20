namespace FlashcardsLibrary.Display;
internal class PackDisplay
{
    private UserInterface UI { get; set; } = new();
    private string[] Menu { get; } =
    {
        "N: New Study Deck",
        "E: Edit Packlist",
        "R: Rename Pack",
        "D: Delete Deck",
        "Q: Exit Pack Manager"
    };

    internal string CreatePack()
    {
        throw new NotImplementedException();
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
}
