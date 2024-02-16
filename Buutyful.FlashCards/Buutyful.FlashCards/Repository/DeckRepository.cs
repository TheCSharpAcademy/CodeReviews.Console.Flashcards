using Buutyful.Coding_Tracker;
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
        var sql = @$"INSERT INTO {Constants.Tables.DecksTable}
                    (Name, Category) VALUES (@Name, @Category);";
        var result = connection.Execute(sql,
            new
            {
                Name = entity.Name,
                Category = entity.Category
            });
    }
    public bool Find(Expression<Func<Deck, bool>> predicate)
    {
        using var con = SqlConnectionFactory.Create();

        string sql = @$"SELECT * FROM {Constants.Tables.DecksTable}";

        var result = con.Query<Deck>(sql);

        return result.Any(predicate.Compile());
    }
    public List<Deck> GetDecksOnly()
    {
        using var connection = SqlConnectionFactory.Create();
        var sql = @$"Select Id, Name, Category From {Constants.Tables.DecksTable};";
        return connection.Query<Deck>(sql).ToList();
    }
    public IList<Deck> GetAll()
    {
        using var connection = SqlConnectionFactory.Create();
        var sql = @$"Select Id, Name, Category
                    From {Constants.Tables.DecksTable};";

        var cardsSql = @$"Select Id, FrontQuestion, BackAnswer
                         FROM {Constants.Tables.FlashCardsTable} f
                         WHERE f.DeckId = @Id";

        var decks = connection.Query<Deck>(sql).ToList();

        foreach (var deck in decks)
        {
            var cards = connection.Query<FlashCard>(cardsSql, new { Id = deck.Id });
            deck.FlashCards.AddRange(cards);
        }
        return decks;
    }
    public async Task<IList<Deck>> GetAllAsync()
    {
        using var connection = SqlConnectionFactory.Create();
        const string sql = @"
            SELECT d.Id, d.Name, d.Category,
                   f.Id AS FlashCardId, f.FrontQuestion, f.BackAnswer
            FROM Decks d
            LEFT JOIN FlashCards f ON d.Id = f.DeckId";

        var results = await connection.QueryAsync<Deck, FlashCard, Deck>(
            sql,
            (deck, card) =>
            {
                deck.FlashCards ??= new();
                if (card != null)
                {
                    deck.FlashCards.Add(card);
                }
                return deck;
            },
            splitOn: "FlashCardId"
        );

        return results.Distinct().ToList();
    }
    public Deck GetById(int id)
    {
        using var connection = SqlConnectionFactory.Create();
        var sql = @$"Select Id, Name, Category
                             From {Constants.Tables.DecksTable}
                             WHERE Id = @Id;";
        var deck = connection.QueryFirstOrDefault<Deck>(sql, new { Id = id });
        return deck ?? throw new Exception($"deck {id} not found");
    }
    public void Update(Deck entity)
    {
        using var connection = SqlConnectionFactory.Create();
        var sql = @$"UPDATE {Constants.Tables.DecksTable}
                     SET Name = @Name, Category = @Category
                     WHERE Id = @Id";

        var affectedRows = connection.Execute(sql, entity);

        if (affectedRows == 0)
        {
            throw new Exception($"Deck with Id {entity.Id} not found");
        }
    }
    public void Delete(Deck entity)
    {
        using var connection = SqlConnectionFactory.Create();
        {
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Delete FlashCards associated with the Deck
                const string deleteFlashCardsSql = "DELETE FROM FlashCards WHERE DeckId = @DeckId";
                connection.Execute(deleteFlashCardsSql, new { DeckId = entity.Id }, transaction);

                // Delete StudySessions associated with the Deck            
                const string deleteStudySessionsSql = "DELETE FROM StudySessions WHERE DeckId = @DeckId";
                connection.Execute(deleteStudySessionsSql, new { DeckId = entity.Id }, transaction);

                // Delete the Deck 
                const string deleteDeckSql = "DELETE FROM Decks WHERE Id = @Id";
                var affectedRows = connection.Execute(deleteDeckSql, new { Id = entity.Id }, transaction);

                if (affectedRows == 0)
                {
                    throw new Exception($"Deck with Id {entity.Id} not found");
                }

                // Commit the transaction if everything is successful
                transaction.Commit();
            }
            catch (Exception)
            {
                // Rollback the transaction in case of an exception
                transaction.Rollback();
                throw;
            }
            connection.Close();
        }
    }

}
