using Flashcards.CoreyJordan.Display;
using FlashcardsLibrary.Data;
using FlashcardsLibrary.Models;
using System.Data.SqlClient;

namespace Flashcards.CoreyJordan.Controller;
internal class PackManager : Controller
{
    internal void ManagePacks()
    {
        // TODO test UI for title placement
        UIConsole.TitleBar("PACK MANAGER");

        bool exitPackManager = false;
        while (exitPackManager == false)
        {
            UIPack.DisplayMenu();
            try
            {
                switch (UserInput.GetString("Select an option: ").ToUpper())
                {
                    case "1":
                        CreatePack();
                        break;
                    case "2":
                        EditPack();
                        break;
                    case "3":
                        RenamePack();
                        break;
                    case "4":
                        DeletePack();
                        break;
                    case "X":
                        exitPackManager = true;
                        break;
                    default:
                        UIConsole.Prompt("Invalid Selection. Please try again.");
                        break;
                }
            }
            catch (SqlException ex)
            {
                UIConsole.Prompt(ex.Message);
            }
        }
    }

    private void DeletePack()
    {
        string packChoice = ChoosePack(PackGateway.GetPacks());
        if (UserInput.Confirm("Are you sure you wish to delete this pack? ") == false)
        {
            UIConsole.Prompt("Delete canceled");
            return;
        }

        if (PackGateway.DeletePack(packChoice) == 0)
        {
            UIConsole.Prompt("There was an error deleting the pack.");
            return;
        }

        UIConsole.Prompt("Pack deleted successfully.");
        return;
    }

    private void RenamePack()
    {
        string packChoice = ChoosePack(PackGateway.GetPacks());
        string newName = "Default";
        bool isUnique = false;

        List<PackModel> packs = PackGateway.GetPacks();

        while (isUnique == false)
        {
            isUnique = true;
            newName = UIPack.NamePack("RENAME PACK");

            if (packs.Any(x => x.Name == newName) == true)
            {
                UIConsole.PromptAndReset("Name already exists");
                isUnique = false;
            }
        }


        if (PackGateway.UpdatePackName(packChoice, newName) == 0)
        {
            UIConsole.Prompt("There was an error renaming the pack.");
            return;
        }

        UIConsole.Prompt("Pack renamed successfully.");
        return;
    }

    private static void EditPack()
    {
        CardManager cardManager = new();
        cardManager.EditPack();
    }

    private void CreatePack()
    {
        string packName = "Default";
        bool isUnique = false;

        List<PackModel> packs = PackGateway.GetPacks();

        while (isUnique == false)
        {
            packName = UIPack.NamePack("NEW PACK");

            if (packs.Any(x => x.Name == packName) == false)
            {
                isUnique = true;
                break;
            }
            UIConsole.PromptAndReset("Name already exists");
        }

        PackGateway.InsertPack(packName);
        UIConsole.Prompt("Pack created successfully.");
    }
}
