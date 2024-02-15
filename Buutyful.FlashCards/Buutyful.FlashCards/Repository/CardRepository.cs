using Buutyful.FlashCards.Data;
using Buutyful.FlashCards.Models;
using Dapper;
using Infrastructure.Repositoreis.Interfaces;
using System.Linq.Expressions;

namespace Buutyful.FlashCards.Repository;

public class CardRepository : IRepository<FlashCard>
{
    public void Add(FlashCard entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(FlashCard entity)
    {
        throw new NotImplementedException();
    }

    public FlashCard Find(Expression<Func<FlashCard, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public IList<FlashCard> GetAll()
    {
        throw new NotImplementedException();
    }
    public IList<FlashCard> GetByDeckId(int deckId)
    {
        using var connection = SqlConnectionFactory.Create();
        const string sql = @"Select Id, FrontQuestion, BackAnswer
                             From FlashCards Where DeckId = @Id";
        var cards = connection.Query<FlashCard>(sql, new { Id = deckId });
        return cards.ToList();
    }

    public FlashCard GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(FlashCard entity)
    {
        throw new NotImplementedException();
    }
}
