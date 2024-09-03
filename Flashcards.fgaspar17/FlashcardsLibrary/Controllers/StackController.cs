using CodingTrackerLibrary;

namespace FlashcardsLibrary;
public class StackController
{
    public static List<Stack> GetStacks()
    {
        string sql = "SELECT StackId, Name FROM dbo.Stacks ";
        return SqlExecutionService.GetListModels<Stack>(sql);
    }

    public static Stack GetStackByName(string name)
    {
        string sql = "SELECT StackId, Name FROM dbo.Stacks WHERE Name = @Name ";
        return SqlExecutionService.GetModelByKey<string, Stack>(sql, field: "Name", name);
    }

    public static bool InsertStack(Stack stack)
    {
        string sql = $@"INSERT INTO dbo.Stacks (Name) 
                                        VALUES (@Name);";

        return SqlExecutionService.ExecuteCommand<Stack>(sql, stack);
    }

    public static bool UpdateStack(Stack stack)
    {
        string sql = $@"UPDATE dbo.Stacks SET
                                Name = @Name
                                WHERE StackId = @StackId;";

        return SqlExecutionService.ExecuteCommand<Stack>(sql, stack);
    }

    public static bool DeleteStack(Stack stack)
    {
        string sql = $@"DELETE FROM dbo.Stacks
                                WHERE StackId = @StackId;";

        return SqlExecutionService.ExecuteCommand<Stack>(sql, stack);
    }
}