
using Flashcards.StanimalTheMan.DTOs;
using Spectre.Console;
using System.Data;
using System.Data.SqlClient;

namespace Flashcards.StanimalTheMan;

internal class StudyInterface
{
    internal static void ShowMenu()
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

                    // probably should rename stackNames with better name as I am adding a return to main menu option which isn't a stack name
                    stackNames.Add("Return to Main Menu");
                    var selection = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("-------------------------------")
                        .PageSize(10)
                        .AddChoices(stackNames));

                    if (selection == "Return to Main Menu")
                    {
                        Console.Clear();
                        ShowMenu();
                    }

                    // otherwise study a stack
                    Study(selection);
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

    private static void Study(string stackName)
    {
        // probably bad to fetch stack by stackName to get StackId, then fetch flashcards with matching foreign key
        SqlConnection connection = null;

        int selectedStackId = 0;
        try
        {
            connection = DatabaseHelper.GetOpenConnection();

            // Perform database operations here

            //Console.WriteLine("Connection successful!");

            string selectStackQuery = $"SELECT * FROM Stacks WHERE StackName = @StackName";

            using (SqlCommand command = new SqlCommand(selectStackQuery, connection))
            {
                command.Parameters.AddWithValue("@StackName", stackName);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int stackId = reader.GetInt32(0);

                        selectedStackId = stackId;
                    }
                }
            }

            Dictionary<long, int> mapping = new();
            List<FlashcardDTO> flashcardDTOs = new();
            List<string> flashcardSelectionOptions = new();
            string fetchFlashcardsQuery = $"SELECT FlashcardId, ROW_NUMBER() OVER (ORDER BY FlashcardId) AS SequentialId, Front, Back FROM Flashcards WHERE StackId = @StackId";

            SqlCommand getFlashcards = new SqlCommand(fetchFlashcardsQuery, connection);

            getFlashcards.Parameters.AddWithValue("@StackId", selectedStackId);
            using (SqlDataReader reader = getFlashcards.ExecuteReader())
            {
                while (reader.Read())
                {
                    int flashcardId = reader.GetInt32(0);
                    long sequentialId = reader.GetInt64(1);
                    string front = reader.GetString(2);
                    string back = reader.GetString(3);

                    mapping.Add(sequentialId, flashcardId);
                    flashcardDTOs.Add(new FlashcardDTO(sequentialId, flashcardId, front, back));
                }
            }

            foreach (FlashcardDTO flashcardDTO in flashcardDTOs)
            {
                string selectionString = "";
                selectionString += flashcardDTO.FlashcardSequentialId;
                selectionString += $"\t{flashcardDTO.Front}";
                selectionString += $"\t{flashcardDTO.Back}";
                flashcardSelectionOptions.Add(selectionString);
            }

            flashcardSelectionOptions.Add("Return to Manage Flashcards Menu");

            Console.WriteLine("Choose a flashcard to study or return to main menu");
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("-------------------------------")
                .PageSize(10)
                .AddChoices(flashcardSelectionOptions));

            if (selection == "Return to Manage Flashcards Menu")
            {
                Console.Clear();
                ShowMenu();
            }

            int studySessionScore = 0;
            int questionsAnswered = 0;
            // study card
            while (true)
            {
                Console.Clear();
                string front = selection.Split('\t')[1];
                string back = selection.Split('\t')[2];
                Console.WriteLine($"+ {stackName}");
                Console.WriteLine("| Front |");
                Console.WriteLine("+ ------ +");
                Console.WriteLine($"| {front} |");
                Console.WriteLine("+ ------ +");

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("Input your answer to this card");
                Console.WriteLine("Or 0 to exit");

                string userSelection = Console.ReadLine();
                if (userSelection == "0")
                {
                    // exit study session and add to db
                    string insertStudyQuery = $"INSERT INTO study (Date, Score, StackId) VALUES (@Date, @Score, @StackId)";

                    using (SqlCommand command = new SqlCommand(insertStudyQuery, connection))
                    {
                        command.Parameters.AddWithValue("@StackName", stackName);
                        DateTime time = DateTime.Now;
                        string format = "yyyy-MM-dd HH:mm:ss.ffffff";
                        command.Parameters.Add("@Date", SqlDbType.DateTime).Value = time.ToString(format);
                        command.Parameters.AddWithValue("@Score", studySessionScore);
                        command.Parameters.AddWithValue("@StackId", selectedStackId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Study Session added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to save Study Session.");
                        }
                        Console.WriteLine("Exiting Study session");
                        Console.WriteLine($"You got {studySessionScore} right out of {questionsAnswered}");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    break;
                }
                else
                {
                    if (back.ToLower() == userSelection.ToLower())
                    {
                        // user got correct answer if case insensitive answer is correct
                        studySessionScore++;
                        // go back to displaying flashcard in stack, maybe put in another method for DRY improvement
                        Console.WriteLine("Choose a flashcard to study or return to main menu");
                        selection = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("-------------------------------")
                            .PageSize(10)
                            .AddChoices(flashcardSelectionOptions));
                    }
                    questionsAnswered++;
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