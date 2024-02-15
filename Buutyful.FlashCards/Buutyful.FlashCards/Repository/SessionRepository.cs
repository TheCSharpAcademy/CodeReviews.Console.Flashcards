using Buutyful.Coding_Tracker;
using Buutyful.FlashCards.Data;
using Buutyful.FlashCards.Models;
using Dapper;
using Infrastructure.Repositoreis.Interfaces;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Buutyful.FlashCards.Repository;

public class SessionRepository : IRepository<StudySession>
{
    public void Add(StudySession entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(StudySession entity)
    {
        throw new NotImplementedException();
    }

    public bool Find(Expression<Func<StudySession, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public IList<StudySession> GetAll()
    {
        using var connection = SqlConnectionFactory.Create();
        var sql = @$"Select * From {Constants.StudySessionsTable};";
        const string sqlDeck = @"Select Id, Name, Category
                             From Decks
                             WHERE Id = @Id;";        
        var sessions = connection.Query<StudySession>(sql).ToList();
        foreach (var session in sessions)
        {
            var deck = connection.QueryFirstOrDefault<Deck>(sqlDeck, new { Id = session.DeckId });
            session.Deck = deck;
        }
        return sessions;
    }

    public StudySession GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(StudySession entity)
    {
        throw new NotImplementedException();
    }
}
