using FlashCards.Models;
using Dapper;

namespace FlashCards.Data;

internal class StacksDBController : DataBaseController<Stack>
{
    public StacksDBController()
    {
        InitDataBase();
    }
    public override void DeleteRow(int _id)
    {
        using (var connection = CreateConnection())
        {
            var deleteQuery = "DELETE FROM stacks WHERE id = @id";
            connection.Execute(deleteQuery, new { id = _id });
        }

        
    }

    public override void InsertRow(Stack stack)
    {
        using (var connection = CreateConnection())
        {
            var insertQuery = "INSERT INTO stacks (name) VALUES (@name)";
            connection.Execute(insertQuery, new { stack.name });
        }

    }

    public override List<Stack> ReadAllRows()
    {
        using (var connection = CreateConnection())
        {
            var readQuery = "SELECT * FROM stacks";
            List<Stack> stacks = connection.Query<Stack>(readQuery).ToList();
            return stacks;
        }
    }

    public override void UpdateRow(Stack stack)
    {
        using (var connection = CreateConnection())
        {
            var updateQuery = "UPDATE stacks SET name = @name WHERE id = @id";
            connection.Execute(updateQuery, stack);
        }

    }

}

