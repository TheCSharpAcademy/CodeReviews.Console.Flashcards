namespace DotNETConsole.Flashcards.Controllers;

using DTO;
using Database;
using Dapper;
using Microsoft.Data.SqlClient;
using Views;
using Models;

public class StackController
{
    private DbContext _dbContext = new DbContext();
    private UserViews userViews = new UserViews();

    public List<Stack> GetStacks()
    {
        List<Stack> stacks = new List<Stack>();

        using (var connection = _dbContext.DBConnection())
        {
            string query = @"Select * from Stacks";
            connection.Open();
            stacks = connection.Query<Stack>(query).ToList();
        }
        return stacks;
    }

    public List<StackViewDto> GetStacksView()
    {
        List<StackViewDto> stacks = new List<StackViewDto>();

        using (var connection = _dbContext.DBConnection())
        {
            string query = @"Select Name from Stacks";
            connection.Open();
            stacks = connection.Query<StackViewDto>(query).ToList();
        }
        return stacks;
    }

    public void CeateStack(string name)
    {
        using (var connection = _dbContext.DBConnection())
        {
            string query = @"INSERT INTO Stacks (Name) VALUES (@Name)";
            var value = new { Name = name };
            try
            {
                connection.Execute(query, value);
                userViews.Tost($"{name} stack created successfully.", "success");
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    Console.WriteLine("A stack with this name already exists.", "error");
                }
                else
                {
                    userViews.Tost($"An error occurred: {ex.Message}", "error");
                }
            }
        }
    }

    public void UpdateStack(string name, Stack stack)
    {
        using (var connection = _dbContext.DBConnection())
        {
            string query = @"UPDATE Stacks SET Name=@Name WHERE ID=@ID";
            var value = new { Name = name, ID = stack.ID };
            try
            {
                connection.Execute(query, value);
                userViews.Tost($"[[{stack.Name} -> {name}]] updated!.", "success");
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    userViews.Tost("A stack with this name already exists.", "error");
                }
                else
                {
                    userViews.Tost($"An error occurred: {ex.Message}", "error");
                }
            }
        }
    }

    public void DeleteStack(Stack stack)
    {
        using (var connection = _dbContext.DBConnection())
        {
            string query = @"DELETE FROM Stacks WHERE ID=@ID";
            var queryParams = new { ID = stack.ID };
            try
            {
                connection.Execute(query, queryParams);
                userViews.Tost($"{stack.Name} deleted successfully.", "success");
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    Console.WriteLine("This Stack Not found in Database.", "error");
                }
                else
                {
                    userViews.Tost($"An error occurred: {ex.Message}", "error");
                }
            }
        }
    }
}
