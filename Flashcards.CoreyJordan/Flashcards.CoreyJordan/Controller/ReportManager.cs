using Flashcards.CoreyJordan.Display;
using FlashcardsLibrary.Data;

namespace Flashcards.CoreyJordan.Controller;
internal class ReportManager : Controller
{
    public SessionUI UISession { get; set; } = new();

    internal void ManageReports()
    {
        UISession.DisplayUsers(SessionGateway.GetAllUsers());
        string userChoice = UserInput.GetString("Select a user or leave blank to select all");

        // Display Menu
        UISession.DisplayReportMenu();
        // Get Menu Choice
        throw new NotImplementedException();
    }
}
