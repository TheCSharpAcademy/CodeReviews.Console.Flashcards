using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary.Data;
using System.Data.SqlClient;

namespace Flashcards.CoreyJordan.Controller;
internal class PackManager : Controller
{
    internal void ManagePacks()
    {
        UIConsole.TitleBar("PACK MANAGER");

        bool exitPackManager = false;
        while (exitPackManager == false)
        {
            UIPack.DisplayMenu();
            string menuChoice = UserInput.GetString("Select an option: ");
            try
            {
                switch (menuChoice.ToUpper())
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
        List<PackNamesDTO> allPacks = PackNamesDTO.GetPacksDTO(PackGateway.GetPacksList());
        UIPack.DisplayPacks(allPacks);

        string choiceName = UIPack.ChoosePack(allPacks);
        if (UserInput.Confirm("Are you sure you wish to delete this pack? ") == false)
        {
            UIConsole.Prompt("Delete canceled");
            return;
        }

        if (PackGateway.DeletePack(choiceName) == 0)
        {
            UIConsole.Prompt("There was an error deleting the pack.");
            return;
        }

        UIConsole.Prompt("Pack deleted successfully.");
        return;
    }

    private void RenamePack()
    {
        List<PackNamesDTO> allPacks = PackNamesDTO.GetPacksDTO(PackGateway.GetPacksList());
        UIPack.DisplayPacks(allPacks);

        string choiceName = UIPack.ChoosePack(allPacks);
        string newName = UIPack.NamePack("RENAME PACK");

        int success = PackGateway.UpdatePackName(choiceName, newName);
        switch (success)
        {
            case 0:
                UIConsole.Prompt("There was an error renaming the pack.");
                break;
            default:
                UIConsole.Prompt("Pack renamed successfully.");
                break;
        }
    }

    private static void EditPack()
    {
        CardManager cardManager = new();
        cardManager.EditPack();
    }

    private void CreatePack()
    {
        string packName = UIPack.NamePack("NEW PACK");

        PackGateway.InsertPack(packName);
        UIConsole.Prompt("Pack created successfully.");
    }
}
