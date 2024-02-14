using Buutyful.FlashCards.Data;
using Buutyful.FlashCards.Models;
using Dapper;
using Infrastructure.Repositoreis.Interfaces;
using System.Linq.Expressions;

namespace Buutyful.FlashCards.Repository;

public class DeckRepository : IRepository<Deck>
{    
    public void Add(Deck entity)
    {

        using var connection = SqlConnectionFactory.Create();
        var sql ="INSERT INTO Decks (Name, Category) VALUES (@Name, @Category);";
        var result = connection.Execute(sql,
            new
            {
                Name = entity.Name,
                Category = entity.Category
            });
    }

    public void Delete(Deck entity)
    {
        throw new NotImplementedException();
    }

    public Deck Find(Expression<Func<Deck, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public IList<Deck> GetAll()
    {
        using var connection = SqlConnectionFactory.Create();
        var sql = @"Select Id, Name, Category
                    From Decks;";

        var cardsSql = @"Select Id, FrontQuestion, BackAnswers, CorrectAnswer
                         FROM FlashCards f
                         WHERE f.DeckId = @Id";

        var decks = connection.Query<Deck>(sql);

        foreach (var deck in decks)
        {
            var cards = connection.Query<FlashCard>(cardsSql, new { Id = deck.Id});
            deck.FlashCards.AddRange(cards);
        }
        return decks.ToList();
    }

    public Deck GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Deck entity)
    {
        throw new NotImplementedException();
    }
}
