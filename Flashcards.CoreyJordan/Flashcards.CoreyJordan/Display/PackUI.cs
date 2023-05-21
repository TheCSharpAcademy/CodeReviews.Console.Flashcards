using ConsoleTableExt;
using Flashcards.CoreyJordan.DTOs;

namespace Flashcards.CoreyJordan.Display;
internal class PackUI
{
    private ConsoleUI UIConsole { get; set; } = new();
    public InputModel UserInput { get; set; } = new();
    public List<MenuModel> PackMenu { get; } = new()
    {
        new MenuModel("1", "New Study Deck"),
        new MenuModel("2", "Edit Pack Contents"),
        new MenuModel("3", "Rename Pack"),
        new MenuModel("4", "Delete Pack"),
        new MenuModel("X", "Exit Pack Manager")
    };

    internal string NamePack(string title)
    {
        string name = "";
        while (string.IsNullOrEmpty(name) || name.Contains(' '))
        {
            UIConsole.TitleBar(title);
            name = UserInput.GetString("Enter a name for this deck: ");

            if (string.IsNullOrEmpty(name))
            {
                UIConsole.Prompt("Name cannot be blank.");
            }
            else if (name.Contains(' '))
            {
                UIConsole.Prompt("Name cannot conatins spaces.");
            }
        }
        return name;
    }

    internal void DisplayMenu()
    {
        UIConsole.TitleBar("PACK MANAGER");

        ConsoleTableBuilder
            .From(PackMenu)
            .ExportAndWriteLine(TableAligntment.Center);
    }

    internal void DisplayPacks(List<PackNamesDTO> packs)
    {
        UIConsole.TitleBar("PACKS");

        ConsoleTableBuilder
            .From(packs)
            .WithColumn("#", "NAME")
            .ExportAndWriteLine(TableAligntment.Center);
        Console.WriteLine();
    }

    internal string ChoosePack(List<PackNamesDTO> packs)
    {
        int index = 0;

        while (packs.Any(x => x.Number == index) == false)
        {
            index = UserInput.GetInt("Select a pack number: ");
            if (packs.Any(x => x.Number == index) == false)
            {
                UIConsole.Prompt("Pack number not listed. Please try again.");
                DisplayPacks(packs);
            }
        }

        var name = packs.Where(x => x.Number == index).ToList();
        return name[0].Name;
    }
}
