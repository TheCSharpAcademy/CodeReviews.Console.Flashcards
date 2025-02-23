using Dapper;
using FlashCards.Models;
using FlashCards.Models.Stack;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace FlashCards.Controllers
{
    public  class StackController : DbController
    {
        public void Insert( StackBO stack)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = " INSERT INTO stacks (Name) VALUES (@Name);";
                try
                {
                    connection.Execute(sql, stack);
                }
                catch (SqlException ex) when (ex.Number == 2627)
                {
                    AnsiConsole.MarkupLine("[Red] This stack already exists![/]");

                    return;
                }
                AnsiConsole.MarkupLine("[Green] Added succesfully [/]");
            }
        }

        public IEnumerable<StackDTO> GetAllDTO()
        {
            using(var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT Name FROM  stacks";
                var stackList=connection.Query<StackDTO>(sql).ToList();
                return stackList;
            }
        }
        public IEnumerable<StackBO> GetAllBO()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT Id,Name FROM stacks";
                var stackList = connection.Query<StackBO>(sql).ToList();
                return stackList;
            }
        }

     
        public void Remove(StackBO stack)
        {
            using var connection = new SqlConnection(connectionString);
            {
                
                var sql = "DELETE FROM stacks WHERE Id=@Id";
                try
                {
                    connection.Execute(sql, stack);
                }
                catch(Exception ex)
                {
                    AnsiConsole.Markup($"[Red]{ex.Message} [/]");
                    return;
                }
                AnsiConsole.MarkupLine("[Green] Removed succesfully [/]");

            }
        }

        public StackBO GetUserSelection(IEnumerable<StackBO> stackList)
        {
            var selectedStack = AnsiConsole.Prompt(new SelectionPrompt<StackBO>()
                .Title("Select the stack")
                .AddChoices(stackList)
                .UseConverter(stack=>stack.Name)

                );
               
            return selectedStack;
        }

        public void Update(StackBO stackToUpdate,StackBO newStack)
        {
            newStack.Id = stackToUpdate.Id;
            using(var connection = new SqlConnection(connectionString))
            {
                var sql = @"UPDATE stacks
                         SET Name=@Name
                         WHERE Id=@Id ";
                try
                {
                    connection.Execute(sql, newStack);
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[Red]{ex.Message}[/]");
                    return;
                }
                AnsiConsole.MarkupLine("[Green]Succesfully Updated[/]");
            }
        }

    }
}
