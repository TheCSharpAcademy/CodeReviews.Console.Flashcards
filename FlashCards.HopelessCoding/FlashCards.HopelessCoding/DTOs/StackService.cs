using Spectre.Console;
using System.Data.SqlClient;

namespace FlashCards.HopelessCoding.DTOs;

public class StackService
{
    private readonly string _connectionString;

    public StackService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<StackDTO> GetStacks()
    {
        List<StackDTO> stacks = new List<StackDTO>();

        string query = "SELECT * FROM Stacks;";

        using (var connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StackDTO stack = new StackDTO
                        {
                            StackName = reader.GetString(0)
                        };

                        stacks.Add(stack);
                    }
                }
            }
        }
        return stacks;
    }

    public void PrintAllStacks()
    {
        List<StackDTO> stacks = GetStacks();

        if (stacks.Count > 0)
        {
            var stacksTable = new Table();
            stacksTable.Title = new TableTitle($"[yellow1]All stacks[/]");
            stacksTable.Border = TableBorder.Rounded;
            stacksTable.AddColumn("[gold1]Name[/]");
            stacksTable.Columns[0].Padding(1, 0);

            foreach (var stack in stacks)
            {
                stacksTable.AddRow($"{stack.StackName}");
            }

            AnsiConsole.Write(stacksTable);
        }
        else
        {
            Console.WriteLine("No stacks found.");
        }
    }
}