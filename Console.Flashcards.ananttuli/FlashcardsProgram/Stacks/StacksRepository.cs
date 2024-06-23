using Dapper;
using FlashcardsProgram.Database;
using Microsoft.Data.SqlClient;

namespace FlashcardsProgram.Stacks;

public class StacksRepository : BaseRepository<StackDAO>
{

    public StacksRepository(string tableName) : base(tableName)
    {
    }

    public StackDAO? FindByName(string name)
    {
        try
        {
            string sql = $@"
                SELECT * FROM {StackDAO.TableName}
                WHERE LOWER(Name) = LOWER(@Name)
            ";

            return ConnectionManager.Connection.QuerySingleOrDefault<StackDAO>(sql, new { Name = name });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not find Stack by name. ERROR: {ex.Message}");
        }

        return null;
    }
}