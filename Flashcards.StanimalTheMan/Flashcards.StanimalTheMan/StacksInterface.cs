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
            case ManageStacksOption.DeleteStack:
                DeleteStack();
                ShowMenu();
                break;
            case ManageStacksOption.EditStack:
                UpdateStack();
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



            string selectFlashcardsQuery = $"SELECT * FROM Stacks";

            using (SqlCommand command = new SqlCommand(selectFlashcardsQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
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

    internal static void DeleteStack()
    {
        SqlConnection connection = null;

        try
        {
            connection = DatabaseHelper.GetOpenConnection();
            string getStacksQuery = "SELECT * FROM Stacks";
            string deleteStackQuery = $"DELETE FROM Stacks WHERE StackName = @StackName";

            SqlCommand getStacksCommand = new SqlCommand(getStacksQuery, connection);
            List<string> stackNames = new List<string>();

            try
            {
                using (SqlDataReader reader = getStacksCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int stackId = reader.GetInt32(0);
                        string stackName = reader.GetString(1);

                        stackNames.Add(stackName);
                    }
                }

                stackNames.Add("Return to Manage Stacks Menu");

                Console.WriteLine("Choose a stack to delete");
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("-------------------------------")
                    .PageSize(10)
                    .AddChoices(stackNames));

                if (selection == "Return to Manage Stacks Menu")
                {
                    Console.Clear();
                    ShowMenu();
                }

                using (SqlCommand deleteStackCommand = new SqlCommand(deleteStackQuery, connection))
                {
                    deleteStackCommand.Parameters.AddWithValue("@StackName", selection);

                    int rowsAffected = deleteStackCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Stack '{selection}' deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"No stack found with the name '{selection}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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

    internal static void UpdateStack()
    {
        SqlConnection connection = null;

        try
        {
            connection = DatabaseHelper.GetOpenConnection();

            List<Stack> stacks = new List<Stack>();
            List<string> stackNames = new List<string>();

            // Read stacks
            string getStacksQuery = "SELECT * FROM Stacks";
            using (SqlCommand readCommand = new SqlCommand(getStacksQuery, connection))
            {
                using (SqlDataReader reader = readCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int stackId = reader.GetInt32(0);
                        string stackName = reader.GetString(1);

                        stacks.Add(new Stack(stackId, stackName));
                        stackNames.Add(stackName);
                    }
                }
            }

            // Allow user to select a stack to update
            stackNames.Add("Return to Manage Stacks Menu");
            Console.WriteLine("Choose a stack to update");
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("-------------------------------")
                    .PageSize(10)
                    .AddChoices(stackNames));

            if (selection == "Return to Manage Stacks Menu")
            {
                Console.Clear();
                ShowMenu();
            }

            // Get the new stack name
            Console.WriteLine("Enter New Stack Name: ");
            string newStackName = Console.ReadLine();

            // Update the selected stack
            string updateStackQuery = "UPDATE Stacks SET StackName = @NewStackName WHERE StackName = @StackName";
            using (SqlCommand updateCommand = new SqlCommand(updateStackQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@NewStackName", newStackName);
                updateCommand.Parameters.AddWithValue("@StackName", selection);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Stack '{selection}' updated to '{newStackName}' successfully.");
                }
                else
                {
                    Console.WriteLine($"No stack found with the name '{selection}'.");
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
}
