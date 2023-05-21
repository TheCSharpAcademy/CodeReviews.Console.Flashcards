﻿using ConsoleTableExt;
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
        bool notValid = true;

        while (notValid)
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
            else
            {
                notValid = false;
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
        bool inRange = false;

        while (inRange == false)
        {
            index = UserInput.GetInt("Select a pack number: ");
            inRange = packs.Any(x => x.Number == index);

            if (inRange == false)
            {
                UIConsole.Prompt("Pack number not listed. Please try again.");
                DisplayPacks(packs);
            }
        }

        List<PackNamesDTO> names = packs.Where(x => x.Number == index).ToList();
        return names[0].Name;
    }
}
