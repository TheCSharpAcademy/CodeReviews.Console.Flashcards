
using System.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using Flashcards;

string? connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

if(connectionString == null)
{
    Console.WriteLine("No Connection String found.");
    return;
}

FlashcardsController appController = new FlashcardsController(connectionString);
appController.RunApp();

// using (var connection = new SqlConnection(connectionString))
// {
//     connection.Open();
//     var sql = @"CREATE TABLE Customers (
// First_Name varchar(50) NOT NULL,
// Last_Name varchar(50) NOT NULL,
// City varchar(50) NOT NULL,
// Email varchar(100) NOT NULL,
// Phone_Number varchar(20) NOT NULL,
// Registration_Date date NOT NULL
// );";
//     connection.Execute(sql);
// }