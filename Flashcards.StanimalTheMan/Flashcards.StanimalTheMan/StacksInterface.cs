using Flashcards.StanimalTheMan.Models;
using Spectre.Console;
using System.Data.SqlClient;

namespace Flashcards.StanimalTheMan;

internal enum ManageStacksOption
{
    ReturnToMainMenu,
    ReturnToManageStacksMenu,
    ViewAllStacks,
    CreateStack,
    DeleteStack,
    EditStack
}

internal class StacksInterface
{
    internal static void ShowMenu()
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<ManageStacksOption>()
            .Title("Manage Stacks")
            .PageSize(10)
            .AddChoices(ManageStacksOption.ViewAllStacks, ManageStacksOption.CreateStack, ManageStacksOption.DeleteStack, ManageStacksOption.EditStack, ManageStacksOption.ReturnToMainMenu));

        switch (selection)
        {
            case ManageStacksOption.ReturnToMainMenu:
                MainMenu.ShowMenu();
                break;
            case ManageStacksOption.ViewAllStacks:
                ViewAllStacks();
                break;
            case ManageStacksOption.CreateStack:
                CreateStack();
                ShowMenu();
                break;
        }
    }

    internal static void ViewAllStacks()
    {
        SqlConnection connection = null;
        List<string> stackNames = new();

        try
        {

            connection = DatabaseHelper.GetOpenConnection();

            // Perform database operations here

            //Console.WriteLine("Connection successful!");

            string selectFlashcardsQuery = $"SELECT * FROM Stacks";

            using (SqlCommand command = new SqlCommand(selectFlashcardsQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        //int stackId = reader.GetInt32(0);
                        string stackName = reader.GetString(1);

                        stackNames.Add(stackName);
                    }

                    foreach (string stackName in stackNames)
                    {
                        Console.WriteLine(stackName);
                    }
                    var selection = AnsiConsole.Prompt(
                        new SelectionPrompt<ManageStacksOption>()
                        .Title("-------------------------------")
                        .PageSize(10)
                        .AddChoices(ManageStacksOption.ReturnToManageStacksMenu));

                    switch (selection)
                    {
                        case ManageStacksOption.ReturnToManageStacksMenu:
                            Console.Clear();
                            ShowMenu();
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            DatabaseHelper.CloseConnection(connection);
        }
    }

    internal static void CreateStack()
    {
        SqlConnection connection = null;

        try
        {
            connection = DatabaseHelper.GetOpenConnection();
            Console.Write("Enter Stack Name: ");
            string stackName = Console.ReadLine();


                string insertStackQuery = $"INSERT INTO Stacks (StackName) VALUES (@StackName)";

                using (SqlCommand command = new SqlCommand(insertStackQuery, connection))
                {
                    command.Parameters.AddWithValue("@StackName", stackName);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Stack '{stackName}' created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create the stack.");
                    }
                Console.Clear();
                }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            DatabaseHelper.CloseConnection(connection);
        }
    }
}
