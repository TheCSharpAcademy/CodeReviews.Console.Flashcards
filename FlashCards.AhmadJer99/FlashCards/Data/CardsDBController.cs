using Dapper;
using FlashCards.Models;

namespace FlashCards.Data;

internal class CardsDBController : DataBaseController<Card>
{
    public CardsDBController()
    {
        InitDataBase();
    }
    public override void DeleteRow(int _cardnumber)
    {
        using (var connection = CreateConnection())
        {
            var deleteQuery = "DELETE FROM cards WHERE cardnumber = @cardnumber";
            connection.Execute(deleteQuery, new { cardnumber = _cardnumber });
        }
    }

    public override void InsertRow(Card newCard)
    {
        using (var connection = CreateConnection())
        {
            var insertQuery = "INSERT INTO cards (FK_stack_id,front,back) VALUES (@FK_stack_id,@front,@back)";
            connection.Execute(insertQuery, newCard);
        }

    }

    public override List<Card> ReadAllRows()
    {
        using (var connection = CreateConnection())
        {
            var readQuery = "SELECT * FROM cards";
            List<Card> cards = connection.Query<Card>(readQuery).ToList();
            return cards;
        }
    }
    public List<Card> ReadAllRows(int _FK_stack_id, bool sequenced = false)
    {

        if (!sequenced)
            using (var connection = CreateConnection())
            {
                var readQuery = "SELECT  * FROM cards where FK_stack_id = @FK_stack_id";
                List<Card> cards = connection.Query<Card>(readQuery, new { FK_stack_id = _FK_stack_id }).ToList();
                return cards;
            }
        else
            using (var connection = CreateConnection())
            {
                var readQuery = "SELECT ROW_NUMBER() OVER (ORDER BY FK_stack_id) AS cardnumber,FK_stack_id,front, back FROM cards WHERE FK_stack_id = @FK_stack_id;";
                List<Card> cards = connection.Query<Card>(readQuery, new { FK_stack_id = _FK_stack_id }).ToList();
                return cards;
            }
    }
    public int RowsCount(int _FK_stack_id)
    {
        using (var connection = CreateConnection())
        {
            var readQuery = "SELECT COUNT(*) FROM cards where FK_stack_id = @FK_stack_id";
            return connection.ExecuteScalar<int>(readQuery, new { FK_stack_id = _FK_stack_id });
        }

    }

    public override void UpdateRow(Card card)
    {
        using (var connection = CreateConnection())
        {
            var updateQuery = "UPDATE cards SET content = @content WHERE cardnumber = @cardnumber";
            connection.Execute(updateQuery, card);
        }
    }
}

