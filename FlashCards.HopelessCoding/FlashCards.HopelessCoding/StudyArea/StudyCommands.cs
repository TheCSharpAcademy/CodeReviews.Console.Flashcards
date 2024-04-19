using DbHelpers.HopelessCoding;
using FlashCards.HopelessCoding.DTOs;
using HelperMethods.HopelessCoding;
using Spectre.Console;
using System.Data.SqlClient;

namespace FlashCards.HopelessCoding.Study;

internal class StudyCommands
{
    internal static void CreateNewSession(string stackName)
    {
        try
        {
            FlashcardService flashcardService = new FlashcardService(DatabaseHelpers.connectionString);
            List<FlashcardDto> flashcards = flashcardService.GetFlashcards(stackName, null);
            int score = 0;

            if (flashcards.Count > 0)
            {
                foreach (FlashcardDto flashcard in flashcards)
                {
                    Console.Clear();
                    DisplayFlashcard(stackName, flashcard.Front);


                    Console.Write("Input your answer to this card or 0 to return menu: ");
                    string userAnswer = Console.ReadLine().ToLower();

                    if (userAnswer == "0")
                    {
                        break;
                    }
                    else if (userAnswer == flashcard.Back.ToLower())
                    {
                        AnsiConsole.Write(new Markup("[green]Your answer was correct![/]\n\n"));
                        score++;
                    }
                    else
                    {
                        AnsiConsole.Write(new Markup("[red]Your answer was wrong.[/]\n\n"));
                    }
                    Helpers.WaitForUserInput();
                }
                SaveStudySession(score, stackName);
                Console.Write($"\n\nYou completed this study session.\nYou got {score} right out of {flashcards.Count}.\n\n");
                Helpers.WaitForUserInput();
            }
            else
            {
                Helpers.WaitForUserInput();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating a new study session: {ex.Message}");
            Helpers.WaitForUserInput();
        }
    }

    private static void DisplayFlashcard(string stackName, string front)
    {
        var panel = new Panel(front);
        panel.Header = new PanelHeader($"[yellow1]{stackName}[/]").SetAlignment(Justify.Center);
        panel.Padding(3, 0);
        AnsiConsole.Write(panel);
    }

    private static void SaveStudySession(int score, string stackName)
    {
        string createStudySessionQuery = "INSERT INTO StudySessions " +
                                         "VALUES (@StackName, @Date, @Score);";

        using (SqlConnection connection = new SqlConnection(DatabaseHelpers.connectionString))
        {
            try
            {
                connection.Open();
                SqlCommand createStackCommand = new SqlCommand(createStudySessionQuery, connection);
                createStackCommand.Parameters.AddWithValue("@StackName", stackName);
                createStackCommand.Parameters.AddWithValue("@Date", DateTime.Now);
                createStackCommand.Parameters.AddWithValue("@Score", score);
                createStackCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup($"[red]$An error occurred while saving session details: {ex.Message}[/]\n\n"));
                Helpers.WaitForUserInput();
            }
        }
    }

    internal static void SelectStackToStudyMenu()
    {
        Console.Clear();

        StackService stackService = new StackService(DatabaseHelpers.connectionString);
        stackService.PrintAllStacks();

        Console.WriteLine("\n-------------------\nInput a stack name which you want to study or input 0 to return main menu");

        while (true)
        {
            Console.WriteLine("-------------------");

            var input = Console.ReadLine();

            if (input == "0")
            {
                Console.Clear();
                return;
            }

            if (DatabaseHelpers.StackExists(input))
            {
                CreateNewSession(input);
                break;
            }
            else
            {
                AnsiConsole.Write(new Markup("[red]Invalid input, try again.[/]\n"));
            }
        }
    }

    internal static void ViewStudySessions()
    {
        StudySessionService studySessionService = new StudySessionService(DatabaseHelpers.connectionString);
        studySessionService.PrintAllStudySessions();
    }
}