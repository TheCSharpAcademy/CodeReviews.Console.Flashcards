using Flashcards.SamGannon.UI;
using System.Configuration;
using DataAccess;

string? connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;

if (connectionString == null )
{
    Console.WriteLine("No connection detection.");

}

//IDataAccess _dataAccess = new SqlDataAccess(connectionString);
//InitalizeTables();
//Console.ReadLine();

//void InitalizeTables()
//{
//    _dataAccess.CreateStackTable();
//    _dataAccess.CreateFlashcardTable();
//    _dataAccess.CreateStudyTable();
//}

MainMenu.ShowMenu();