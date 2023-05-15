using System.Data.SqlClient;

namespace Flashcards.Models;

public class StackDTO
{
    public string Name { get; set; } = string.Empty;

    public StackDTO() { }
    public StackDTO(SqlDataReader reader)
    {
        Name = reader.GetString(0);
    }
}
