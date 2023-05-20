using Flashcards.CoreyJordan.Display;
using FlashcardsLibrary.Data;
using FlashcardsLibrary.DTOs;
using System.Data.SqlClient;

namespace Flashcards.CoreyJordan.Controller;
internal class PackManager
{
    private UserInterface UI { get; set; } = new();
    private InputModel User { get; set; } = new();
    private PackDisplay PackDisplay { get; set; } = new();

    internal void ManagePacks()
    {
        UI.TitleBar("PACK MANAGER");

        bool exitPackManager = false;
        while (exitPackManager == false)
        {
            PackDisplay.DisplayMenu();
            string menuChoice = User.GetMenuChoice();
            try
            {
                switch (menuChoice.ToUpper())
                {
                    case "N":
                        string packName = PackDisplay.NamePack();
                        PackGateway.InsertPack(packName);
                        break;
                    case "E":
                        List<PackOverviewDTO> allPacks = PackGateway.GetAllPacks();
                        PackDisplay.DisplayDecks(allPacks);
                        break;
                    case "R":
                        break;
                    case "D":
                        break;
                    case "X":
                        exitPackManager = true;
                        break;
                    default:
                        UI.Prompt("Invalid Selection. Please try again.");
                        break;
                }
            }
            catch (SqlException ex)
            {
                UI.Prompt(ex.Message);
            }
        }
        throw new NotImplementedException();
    }
}
