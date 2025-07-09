namespace DotNETConsole.Flashcards.Controllers;

using Helper;
using DTO;
using Models;
using Database;
using Dapper;
using Views;
using Microsoft.Data.SqlClient;

public class CardController
{
    private DbContext _dbContext = new DbContext();
    private UserViews _userViews = new UserViews();
    public List<CardViewDto> GetAllCardsView()
    {
        List<CardViewDto> cards = new List<CardViewDto>();

        using (var connection = _dbContext.DBConnection())
        {
            string query = @"SELECT f.ID, f.Question, s.Name AS Stack
                                 FROM flashcards f
                                 JOIN stacks s ON f.STACK_ID = s.ID";
            cards = connection.Query<CardViewDto>(query).ToList();
        }
        return cards;
    }

    public void CreateNewCard(Stack? stack)
    {
        var input = new UserInput();
        (string question, string answer) = input.GetCardInfo();

        using (var connection = _dbContext.DBConnection())
        {
            string query = @"INSERT INTO flashcards (Question, Answer, STACK_ID) VALUES (@Question, @Answer, @StackID)";
            var queryValues = new { Question = question, Answer = answer, StackID = stack.ID };
            try
            {
                connection.Execute(query, queryValues);
                _userViews.Tost("Card Added Sucessfully!!");

            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    _userViews.Tost($"SQL Error: {ex.Message}.", "error");
                }
                else
                {
                    _userViews.Tost($"An error occurred: {ex.Message}", "error");
                }
            }
        }
    }

    public void ModifyCard(int id)
    {
        var input = new UserInput();
        (string question, string answer) = input.GetCardInfo();

        using (var connection = _dbContext.DBConnection())
        {
            string query = @"UPDATE flashcards SET Question=@Question, Answer=@Answer WHERE ID=@ID";
            var queryValues = new { Question = question, Answer = answer, ID = id };
            try
            {
                connection.Execute(query, queryValues);
                _userViews.Tost("Card Updated Sucessfully!!");

            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    _userViews.Tost($"SQL Error: {ex.Message}.", "error");
                }
                else
                {
                    _userViews.Tost($"An error occurred: {ex.Message}", "error");
                }
            }
        }
    }

    public void DeleteCard(int id)
    {
        using (var connection = _dbContext.DBConnection())
        {
            string query = @"DELETE FROM flashcards WHERE ID=@ID";
            var queryParams = new { ID = id };
            try
            {
                connection.ExecuteAsync(query, queryParams);
                _userViews.Tost($"Card deleted successfully.", "success");
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    Console.WriteLine("This Card not found in Database.", "error");
                }
                else
                {
                    _userViews.Tost($"An error occurred: {ex.Message}", "error");
                }
            }
        }
    }
}
