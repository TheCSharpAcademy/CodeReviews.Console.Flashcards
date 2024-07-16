global using Microsoft.Data.SqlClient;
global using Spectre.Console;
global using Dapper;
using Flashcards.MainView;

while (true)
{
    try
    {
        var quit = MainViewController.InitMainView();
        if (quit == true) return;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
        Console.ReadKey();
    }
}