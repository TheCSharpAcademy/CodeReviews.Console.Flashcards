using Flashcards.CoreyJordan.Display;
using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary.Data;
using System.Data.SqlClient;

namespace Flashcards.CoreyJordan.Controller;
internal class ReportManager : Controller
{
    public SessionUI UISession { get; set; } = new();

    internal void ManageReports()
    {
        try
        {
            UIConsole.TitleBar("REPORT MANAGER");
            List<string> users = SessionGateway.GetAllUsers();
            UISession.DisplayUsers(users);
            string userChoice = UserInput.GetString("Select a user or type ALL");
            // TODO vaildate input

            bool exitReportManager = false;
            while (exitReportManager == false)
            {
                // Display Menu
                UIConsole.TitleBar($"REPORT CARD: {userChoice.ToUpper()}");
                UISession.DisplayReportMenu();
                // Get Menu Choice
                switch (UserInput.GetString("Select an option: ").ToUpper())
                {
                    case "1":
                        ViewReports(userChoice);
                        break;
                    case "2":
                        ViewReports(userChoice, UserInput.GetDate("Enter start date: "), UserInput.GetDate("Enter closing date: "));
                        break;
                    case "3":
                        List<PackNamesDTO> packs = DisplayPacksList(PackGateway.GetPacks());
                        ViewReports(userChoice, UIPack.GetPackChoice(packs));
                        break;
                    case "X":
                        exitReportManager = true;
                        break;
                    default:
                        UIConsole.Prompt("Invalid selection. Please try again.");
                        break;
                };
            }
        }
        catch (SqlException ex)
        {
            UIConsole.Prompt(ex.Message);
        }
    }

    private void ViewReports(string userChoice)
    {
        List<SessionDTO> sessions;
        if (userChoice.ToUpper() == "ALL")
        {
            sessions = SessionDTO.GetSessionDtoList(SessionGateway.GetAllSessions());
        }
        else
        {
            sessions = SessionDTO.GetSessionDtoList(SessionGateway.GetSessionsByUser(userChoice));
        }
    }

    private void ViewReports(string userChoice, DateTime startRange, DateTime endRange)
    {

    }

    private void ViewReports(string userChoice, string pack)
    {

    }
}
