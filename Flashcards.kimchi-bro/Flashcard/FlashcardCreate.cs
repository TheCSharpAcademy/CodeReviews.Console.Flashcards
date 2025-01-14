using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

internal class FlashcardCreate
{
    internal static void AddFlashcards()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("Adding flashcard to stack.");

        var stack = StackRead.GetStack();
        if (stack.StackName == null)
        {
            Console.Clear();
            return;
        }

        FlashcardAddingLoop(stack);
    }

    internal static void FlashcardAddingLoop(Stack stack)
    {
        var addNewFlashcard = true;
        while (addNewFlashcard)
        {
            var (front, back) = GetFlashcardInput();
            if (front == "" && back == "")
            {
                Console.Clear();
                return;
            }

            var addFlashcardSuccess = AddNewFlashcard(stack, front, back);
            if (!addFlashcardSuccess)
            {
                Console.Clear();
                return;
            }

            addNewFlashcard = DisplayInfoHelpers.GetYesNoAnswer(
                $"Do you want to add another flashcard to your [green]{stack.StackName}[/] stack?");
        }
        Console.Clear();
    }

    private static (string, string) GetFlashcardInput()
    {
        var flashcardFront = InputHelpers.StringLengthCheck(200, "Enter flashcard's front (0 for exit):");
        if (flashcardFront == "0")
        {
            Console.Clear();
            return ("", "");
        }

        var flashcardBack = InputHelpers.StringLengthCheck(200, "Enter flashcard's back (0 for exit):");
        if (flashcardBack == "0")
        {
            Console.Clear();
            return ("", "");
        }

        AnsiConsole.MarkupLine($"[yellow]{flashcardFront}[/] => [yellow]{flashcardBack}[/]");
        return (flashcardFront, flashcardBack);
    }

    internal static void AddFlashcardsInBulk(List<Flashcard> flashcards, Stack stack)
    {
        using var connection = new SqlConnection(Config.ConnectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();
        var parameters = new DynamicParameters();

        try
        {
            foreach (var flashcard in flashcards)
            {
                parameters.Add("@StackId", stack.StackId);
                parameters.Add("@StackName", stack.StackName);
                parameters.Add("@FlashcardFront", flashcard.FlashcardFront);
                parameters.Add("@FlashcardBack", flashcard.FlashcardBack);
                connection.Execute(@"
                    INSERT INTO Flashcard (StackId, StackName, FlashcardFront, FlashcardBack)
                    VALUES (@StackId, @StackName, @FlashcardFront, @FlashcardBack)",
                    parameters, transaction);
            }
            transaction.Commit();
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            DisplayErrorHelpers.SqlError(ex);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            DisplayErrorHelpers.GeneralError(ex);
        }
    }

    private static bool AddNewFlashcard(Stack stack, string front, string back)
    {
        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@StackId", stack.StackId);
            parameters.Add("@StackName", stack.StackName);
            parameters.Add("@FlashcardFront", front);
            parameters.Add("@FlashcardBack", back);
            connection.Execute(@"
                INSERT INTO Flashcard (StackId, StackName, FlashcardFront, FlashcardBack)
                VALUES (@StackId, @StackName, @FlashcardFront, @FlashcardBack)",
                parameters);

            AnsiConsole.MarkupLine($"[green]A new flashcard created successfully![/]\n");
            return true;
        }
        catch (SqlException ex)
        {
            DisplayErrorHelpers.SqlError(ex);
            return false;
        }
        catch (Exception ex)
        {
            DisplayErrorHelpers.GeneralError(ex);
            return false;
        }
    }
}
