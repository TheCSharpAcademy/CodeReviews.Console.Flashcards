using System.Data.SqlClient;

namespace Flashcards.Models;

public class FlashcardDTO
{
    public string Prompt { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;

    public FlashcardDTO(SqlDataReader reader)
    {
        Prompt = reader.GetString(reader.GetOrdinal("prompt"));
        Answer = reader.GetString(reader.GetOrdinal("answer"));
    }
}
