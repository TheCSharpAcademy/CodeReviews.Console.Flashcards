
using Flashcards.StanimalTheMan.DTOs;
using Spectre.Console;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace Flashcards.StanimalTheMan;

internal enum StudyReportsOption
{
    ViewAll, 
    AverageSessionsPerMonthPerStack,
    AverageScorePerMonthPerStack,
    ReturnToMainMenu
}

internal enum ReportType
{
    NumberSessionsPerMonthPerStack,
    AverageScorePerMonthPerStack
}

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

            string selectStacksQuery = $"SELECT * FROM Stacks";

            using (SqlCommand command = new SqlCommand(selectStacksQuery, connection))
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
                        MainMenu.ShowMenu();
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

    internal static void ShowViewMenu()
    {
        Console.WriteLine("Select what you want to view regarding study sessions");
        var selection = AnsiConsole.Prompt(
                new SelectionPrompt<StudyReportsOption>()
                .Title("-------------------------------")
                .PageSize(10)
                .AddChoices(StudyReportsOption.ViewAll, StudyReportsOption.AverageSessionsPerMonthPerStack, StudyReportsOption.AverageScorePerMonthPerStack, StudyReportsOption.ReturnToMainMenu));

        switch (selection)
        {
            case StudyReportsOption.ViewAll:
                ViewAllStudySessions();
                break;
            case StudyReportsOption.AverageSessionsPerMonthPerStack:
                GetPivotReport(ReportType.NumberSessionsPerMonthPerStack);
                break;
            case StudyReportsOption.AverageScorePerMonthPerStack:
                GetPivotReport(ReportType.AverageScorePerMonthPerStack);
                break;
            case StudyReportsOption.ReturnToMainMenu:
                MainMenu.ShowMenu();
                break;
        }
    }

    private static void GetPivotReport(ReportType reportType)
    {
        SqlConnection connection = null;
        Console.WriteLine("--------------");
        Console.WriteLine("Input a year in format YYYY");
        Console.WriteLine("--------------");

        int year;
        string userInput = Console.ReadLine();
        while (!Int32.TryParse(userInput, out year) || userInput.Length != 4)
        {
            Console.WriteLine("Invalid format. Input a year in format YYYY");
            userInput = Console.ReadLine();
        }

        try
        {
            connection = DatabaseHelper.GetOpenConnection();
            StudyPivotDTO pivotData = null;
            String query;
            
            if (reportType == ReportType.NumberSessionsPerMonthPerStack)
            {
                query = $@"SELECT *
        FROM(
    SELECT stacks.StackName, DATENAME(MONTH, s.Date) AS Month, COUNT(s.Score) AS SessionCount
    FROM Study s
    JOIN Stacks stacks ON stacks.StackId = s.StackId
    WHERE YEAR(s.Date) = @Year
    GROUP BY stacks.StackName, DATENAME(MONTH, s.Date)
) AS SourceTable
PIVOT(
        SUM(SessionCount)
    FOR Month IN([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])
        ) AS PivotTable;";
            }
            else
            {
                query = $@"SELECT *
FROM(
    SELECT stacks.StackName, DATENAME(MONTH, s.Date) AS Month, AVG(s.Score) AS AverageScorePerMonthPerStack
    FROM Study s
    JOIN Stacks stacks ON stacks.StackId = s.StackId
    WHERE YEAR(s.Date) = 2024
    GROUP BY stacks.StackName, DATENAME(MONTH, s.Date)
) AS SourceTable
PIVOT(
    MAX(AverageScorePerMonthPerStack)
    FOR Month IN([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])
) AS PivotTable;";
            }

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Year", year);
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        List<int> monthlySessionsCount = new();
                        string stackName = reader.GetString(0);
                        for (int i = 1; i <= 12; i++)
                        {
                            int count = reader.IsDBNull(i) ? 0 : reader.GetInt32(i);
                            monthlySessionsCount.Add(count);
                        }

                        pivotData = new StudyPivotDTO(stackName, monthlySessionsCount);
                    }

                    if (pivotData == null)
                    {
                        Console.WriteLine("No study sessions");
                        Console.WriteLine("Press any key to Continue...");
                        Console.ReadLine();
                        Console.Clear();
                        MainMenu.ShowMenu();
                    }

                    Console.WriteLine($"+---------Average per month for: {year} --|");
                    Console.WriteLine("| StackName | January | February | March | April | May | June | July | August | September | October | November | December |");
                    Console.Write($"{pivotData.StackName}");
                    foreach (int value in pivotData.MonthlyValues)
                    {
                        Console.Write($" |    {value}   ");
                    }
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    Console.Clear();
                    ShowViewMenu();
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
    private static void ViewAllStudySessions()
    {
        SqlConnection connection = null;
        List<StudyDTO> studyDTOs = new();
        try
        {

            connection = DatabaseHelper.GetOpenConnection();

            // Perform database operations here

            //Console.WriteLine("Connection successful!");

            string selectStudySessionsQuery = $"SELECT Study.Date AS date, Study.Score, Stacks.StackName FROM Study LEFT JOIN Stacks ON Study.StackId = Stacks.StackId";

            using (SqlCommand command = new SqlCommand(selectStudySessionsQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        DateTime dateTimeValue = reader.GetDateTime(reader.GetOrdinal("date"));
                        int score = reader.GetInt32(1);
                        string stackName = reader.GetString(2);

                        studyDTOs.Add(new StudyDTO(dateTimeValue, score, stackName));
                    }

                    Console.WriteLine("+---------Date--------- | Score | StackName --|");
                    foreach (StudyDTO studyDTO in studyDTOs)
                    {
                        Console.WriteLine($"| {studyDTO.Date} | {studyDTO.Score} | {studyDTO.StackName} |");
                        //Console.WriteLine(studyDTO.Date);
                        //Console.WriteLine(studyDTO.Score);
                        //Console.WriteLine(studyDTO.StackName);
                    }

                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    Console.Clear();
                    ShowViewMenu();
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
                MainMenu.ShowMenu();
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
                        command.Parameters.AddWithValue("@Score", (int)(studySessionScore * 100.0 / questionsAnswered));
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
                        // reset study session score and questions answered for next session
                        studySessionScore = 0;
                        questionsAnswered = 0;
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
                    }
                    else
                    {
                        Console.WriteLine("Your answer was wrong.");
                        Console.WriteLine();
                        Console.WriteLine($"You answered {userSelection}");
                        Console.WriteLine($"The Correct answer was {back}");
                    }

                    Console.WriteLine("Press any key to continue");
                    Console.Clear();
                    selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("-------------------------------")
                .PageSize(10)
                .AddChoices(flashcardSelectionOptions));

                    if (selection == "Return to Manage Flashcards Menu")
                    {
                        Console.Clear();
                        MainMenu.ShowMenu();
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