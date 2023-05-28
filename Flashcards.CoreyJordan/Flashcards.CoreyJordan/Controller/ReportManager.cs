using Flashcards.CoreyJordan.Display;
using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary.Data;
using FlashcardsLibrary.Models;

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
            string userChoice = UserInput.GetString("Select a user, type ALL, or 'cancel': ");
            
            while (users.Any(x => x == userChoice) == false)
            {
                UIConsole.PromptAndReset("That user does not exist. Try again.");
                userChoice = UserInput.GetString("Select a user, type ALL, or 'cancel': ");
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
                        DateTime startRange = UserInput.GetDate("Enter start date or type 'cancel': ");
                        // Add 1 day to end range to include results in the input date
                        DateTime endRange = UserInput.GetDate("Enter closing date or type 'cancel': ").AddMinutes(1439);
                        ViewReports(userChoice, startRange, endRange);
                        break;
                    case "3":
                        List<PackNamesDto> packs = DisplayPacksList(PackGateway.GetPacks());
                        PackModel pack = new(UIPack.GetPackChoice(packs));
                        ViewReports(userChoice, pack);
                        break;
                    case "X":
                        exitReportManager = true;
                        break;
                    default:
                        UIConsole.Prompt("Invalid selection. Please try again.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            UIConsole.Prompt(ex.Message);
        }
    }

    private void ViewReports(string userChoice)
    {
        List<SessionDto> sessions;
        if (userChoice.ToUpper() == "ALL")
        {
            sessions = SessionDto.GetSessionDtoList(SessionGateway.GetSessions());
        }
        else
        {
            sessions = SessionDto.GetSessionDtoList(SessionGateway.GetSessions(userChoice));
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
        List<SessionDto> sessions = userChoice switch 
        {
            "ALL" => SessionDto.GetSessionDtoList(SessionGateway.GetSessions(startRange, endRange)),
            _=> SessionDto.GetSessionDtoList(SessionGateway.GetSessions(startRange, endRange, userChoice))
        };

        UIConsole.TitleBar($"REPORT CARD: {userChoice.ToUpper()} FROM {startRange:d} TO {endRange:d}");
        if (sessions.Count == 0)
        {
            UIConsole.Prompt("No reports found");
            return;
        }

        UISession.DisplaySessions(sessions);
        UISession.DisplayReport(new ReportDto(sessions));
        UIConsole.Prompt();
    }

    private void ViewReports(string userChoice, PackModel pack)
    {
        List<SessionDto> sessions = userChoice switch
        {
            "ALL" => SessionDto.GetSessionDtoList(SessionGateway.GetSessions(pack)),
            _=> SessionDto.GetSessionDtoList(SessionGateway.GetSessions(pack, userChoice))
        };

        UIConsole.TitleBar($"REPORT CARD: {userChoice.ToUpper()} FOR THE {pack.Name.ToUpper()} PACK");
        switch (sessions.Count)
        {
            case 0:
                UIConsole.Prompt("No reports found");
                break;
            default:
                UISession.DisplaySessions(sessions);
                UISession.DisplayReport(new ReportDto(sessions));
                UIConsole.Prompt();
                break;
        }
    }
}
