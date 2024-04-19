using Spectre.Console;
using System.Data.SqlClient;

namespace FlashCards.HopelessCoding.DTOs;

public class FlashcardService
{
    private readonly string _connectionString;
    private static Dictionary<int, int> _displayIdToActualId = new Dictionary<int, int>();

    public FlashcardService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Dictionary<int, int> GetDisplayIdToActualId()
    {
        return _displayIdToActualId;
    }

    public List<FlashcardDTO> GetFlashcards(string stackName, int? amount)
    {
        List<FlashcardDTO> flashcards = new List<FlashcardDTO>();

        string query = "SELECT " + (amount != null ? $"TOP {amount}" : "") + " * FROM FlashCards " +
                       "WHERE Stack = @StackName;";

        using (var connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@StackName", stackName);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        int displayedId = 1;
                        while (reader.Read())
                        {
                            FlashcardDTO flashcard = new FlashcardDTO()
                            {
                                Id = reader.GetInt32(0),
                                Stack = reader.GetString(1),
                                Front = reader.GetString(2),
                                Back = reader.GetString(3)
                            };

                            flashcards.Add(flashcard);
                            _displayIdToActualId[displayedId] = flashcard.Id;
                            displayedId++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                }
            }
        }
        return flashcards;
    }

    public void PrintFlashcards(string stackName, int? amount)
    {
        Console.Clear();
        List<FlashcardDTO> flashcards = GetFlashcards(stackName, amount);

        if (flashcards.Count > 0)
        {
            var flashcardsTable = new Table();
            flashcardsTable.Title = new TableTitle($"[yellow1]{(amount == null ? "All" : "" )} flashcards in {stackName}[/]");
            flashcardsTable.Border = TableBorder.Rounded;
            flashcardsTable.AddColumn("[gold1]Id[/]");
            flashcardsTable.AddColumn("[gold1]Front[/]");
            flashcardsTable.AddColumn("[gold1]Back[/]");

            flashcardsTable.Columns[1].Padding(3, 0);
            flashcardsTable.Columns[2].Padding(3, 0);

            foreach (var flashcard in flashcards)
            {
                flashcardsTable.AddRow($"{GetDisplayedId(flashcard.Id)}", $"{flashcard.Front}", $"{flashcard.Back}");
            }

            AnsiConsole.Write(flashcardsTable);
        }
        Console.WriteLine("\n-------------------");
    }

    public int GetDisplayedId(int actualId)
    {
        foreach (var kvp in _displayIdToActualId)
        {
            if (kvp.Value == actualId)
            {
                return kvp.Key;
            }
        }
        return -1;
    }
}