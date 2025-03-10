using Dapper;
using FlashCards.Models;
using FlashCards.Models.FlashCards;
using FlashCards.Models.Stack;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace FlashCards.Controllers;

public class FlashCardsController : DbController
{
    public IEnumerable<FlashCardDTO> GetAllCardsDTO(StackBO stack = null)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var sql = "SELECT Name1,Name2 FROM cards";

            if (stack != null)
            {
                sql = "SELECT Name1,Name2 FROM cards WHERE StackId=@Id";
            }

            var cardsList = connection.Query<FlashCardDTO>(sql, stack);
            return cardsList;
        }
    }

    public IEnumerable<FlashCardBO> GetAllCardsBO()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var sql = "SELECT * FROM cards";

            var cardsList = connection.Query<FlashCardBO>(sql);
            return cardsList;
        }
    }

    public IEnumerable<FlashCardBO> GetAllCardsFromStack(StackBO stack)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var sql = "SELECT * FROM cards WHERE StackId=@Id";
            var stackList = connection.Query<FlashCardBO>(sql, stack).ToList();
            return stackList;
        }
    }

    public void Insert(StackBO cardStack, FlashCardBO card)
    {
        card.StackId = cardStack.Id;
        using (var connection = new SqlConnection(connectionString))
        {
            var sql = "INSERT INTO cards(Name1,Name2,StackId) VALUES(@Name1,@Name2,@StackId)";
            connection.Execute(sql, card);
        }
    }

    public FlashCardBO GetUserCardSelection(IEnumerable<FlashCardBO> cards)
    {
        var selectedCard = AnsiConsole.Prompt(new SelectionPrompt<FlashCardBO>()
            .Title("Select the card")
            .AddChoices(cards)
            .UseConverter(card => $"{card.Name1} || {card.Name2}")

            );
        return selectedCard;
    }

    public void Delete(FlashCardBO cardStack)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var sql = "DELETE FROM cards WHERE Id=@Id";
            try
            {
                connection.Execute(sql, cardStack);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[Red]{ex.Message}[/]");
            }
        }
    }

    public void Update(FlashCardBO cardToEdit, FlashCardBO newCard)
    {
        newCard.Id = cardToEdit.Id;
        using (var connection = new SqlConnection(connectionString))
        {
            var sql = "UPDATE cards " +
                "SET Name1=@Name1,Name2=@Name2 " +
                "WHERE Id=@Id ";
            try
            {
                connection.Execute(sql, newCard);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[Red]{ex.Message}[/]");
                return;
            }
            AnsiConsole.MarkupLine("[Green] Succesfully edited[/]");
        }
    }
}