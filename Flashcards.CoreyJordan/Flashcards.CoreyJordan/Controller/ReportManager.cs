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

            List<string> users = UsersGateway.GetAllUsers();
            UISession.DisplayUsers(users);
            users.Add("ALL");
            string userChoice = UserInput.GetString("Select a user or type ALL: ");
            
            while (users.Any(x => x == userChoice) == false)
            {
                UIConsole.PromptAndReset("That user does not exist. Try again.");
                userChoice = UserInput.GetString("Select a user or type ALL: ");
            }

            bool exitReportManager = false;
            while (exitReportManager == false)
            {
                UIConsole.TitleBar($"REPORT CARD: {userChoice.ToUpper()}");

                UISession.DisplayReportMenu();
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
            sessions = SessionDTO.GetSessionDtoList(SessionGateway.GetSessions());
        }
        else
        {
            sessions = SessionDTO.GetSessionDtoList(SessionGateway.GetSessions(userChoice));
        }

        UIConsole.TitleBar($"REPORT CARD: {userChoice.ToUpper()}");
        if (sessions.Count > 0)
        {
            UISession.DisplaySessions(sessions);
            UISession.DisplayReport(new ReportDto(sessions));
            UIConsole.Prompt();
        }
        else
        {
            UIConsole.Prompt("No reports found");
        }
    }

    private void ViewReports(string userChoice, DateTime startRange, DateTime endRange)
    {
        List<SessionDTO> sessions = userChoice switch 
        {
            "ALL" => SessionDTO.GetSessionDtoList(SessionGateway.GetSessions(startRange, endRange)),
            _=> SessionDTO.GetSessionDtoList(SessionGateway.GetSessions(startRange, endRange, userChoice))
        };

        UIConsole.TitleBar($"REPORT CARD: {userChoice.ToUpper()} FROM {startRange} TO {endRange}");
        if (sessions.Count == 0)
        {
            UIConsole.Prompt("No reports found");
            return;
        }

        UISession.DisplaySessions(sessions);
        UISession.DisplayReport(new ReportDto(sessions));
        UIConsole.Prompt();
    }

    private void ViewReports(string userChoice, string pack)
    {

    }
}
