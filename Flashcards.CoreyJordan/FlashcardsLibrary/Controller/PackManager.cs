using FlashcardsLibrary.Display;
using FlashcardsLibrary.FlashCard.Data;
using FlashcardsLibrary.FlashCard.Models;
using System.Data.SqlClient;

namespace FlashcardsLibrary.Controller;
internal class PackManager
{
    private UserInterface UI { get; set; } = new();
    private InputModel User { get; set; } = new();
    private PackDisplay Pack { get; set; } = new();

    internal void ManagePacks()
    {
        UI.TitleBar("PACK MANAGER");

        bool exitPackManager = false;
        while (exitPackManager == false)
        {
            Pack.DisplayMenu();
            string menuChoice = User.GetMenuChoice();
            try
            {
                switch (menuChoice.ToUpper())
                {
                    case "N":
                        string packName = Pack.CreatePack();
                        PackGateway.InsertPack(packName);
                        break;
                    case "E":
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
