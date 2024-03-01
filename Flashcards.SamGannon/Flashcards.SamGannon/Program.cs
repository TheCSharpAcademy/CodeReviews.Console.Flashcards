using Flashcards.SamGannon.UI;
using System.Configuration;
using DataAccess;

string? connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;

if (connectionString == null )
{
    Console.WriteLine("No connection detection.");

}

IDataAccess _dataAccess = new SqlDataAccess(connectionString);
InitalizeTables();

void InitalizeTables()
{
    _dataAccess.CreateStackTable();
    _dataAccess.CreateFlashcardTable();
    _dataAccess.CreateStudyTable();
}

MainMenu mainMenu = new MainMenu(_dataAccess);
StackMenu stackMenu = new StackMenu(mainMenu);
FlashcardMenu flashcardMenu = new FlashcardMenu(mainMenu);
StudySession session = new StudySession(mainMenu);
mainMenu.ShowMenu();