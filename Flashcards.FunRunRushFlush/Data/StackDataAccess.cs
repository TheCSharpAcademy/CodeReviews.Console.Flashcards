using Dapper;
using Flashcards.FunRunRushFlush.Data.Interfaces;
using Flashcards.FunRunRushFlush.Data.Model;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Flashcards.FunRunRushFlush.Data;

public class StackDataAccess : IStackDataAccess
{
    private readonly IDbConnection _connection;

    public StackDataAccess(IDbConnection connection)
    {
        _connection = connection;
    }

    // In this approach I try to combine the best of both worlds:
    // static queries, defined at compile time for better performance, 
    // while keeping them parameterized to maintain flexibility when the model changes.
    //(I didnt like re-interpolating my strings every time the query was called in my previous Acadamy-Projects)

    private static readonly string QueryGetAllStack = $"""
                SELECT * FROM {StackTable.TableName};
                """;
    public List<Stack> GetAllStacks()
    {
        List<Stack> stacks = _connection.Query<Stack>(QueryGetAllStack).ToList();
        return stacks;
    }


    private static readonly string QueryCreateStack = $"""
                INSERT INTO {StackTable.TableName} 
                    ({StackTable.Name})
                VALUES (@{StackTable.Name});
                """;
    public void CreateStack(Stack stack)
    {
        _connection.Execute(QueryCreateStack, new { stack.Name });
    }


    private static readonly string QueryUpdateStack = $"""
                UPDATE {StackTable.TableName}
                SET {StackTable.Name} = @{StackTable.Name}
                WHERE {StackTable.Id} = @{StackTable.Id}
                """;
    public void UpdateStack(Stack stack)
    {
        _connection.Execute(QueryUpdateStack, new { stack.Name, stack.Id });
    }



    private static readonly string QueryDeleteStack = $"""
                    Delete From {StackTable.TableName}
                    WHERE {StackTable.Id} = @Id;
                """;
    public void DeleteStack(Stack stack)
    {
        _connection.Execute(QueryDeleteStack, new { stack.Id });
    }
}
