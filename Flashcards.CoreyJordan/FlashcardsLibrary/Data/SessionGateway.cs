using FlashcardsLibrary.Models;
using System.Data.SqlClient;

namespace FlashcardsLibrary.Data;
public class SessionGateway : ConnManager
{
    public static void InsertSession(SessionModel sessionModel)
    {
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand addSession = connection.CreateCommand();
            addSession.CommandText = @"INSERT INTO dbo.study_session (user_id, pack_id, pack_size, date,
                                                                      cards_shown, score)
                                       VALUES ((SELECT id FROM dbo.users WHERE name = @PlayerName),
                                               (SELECT id FROM dbo.decks WHERE name = @Name),
                                               @PackSize, @Date, @Shown, @Score);";

            addSession.Parameters.AddWithValue("@PlayerName", sessionModel.Player);
            addSession.Parameters.AddWithValue("@Name", sessionModel.Pack);
            addSession.Parameters.AddWithValue("@PackSize", sessionModel.PackSize);
            addSession.Parameters.AddWithValue("@Date", sessionModel.Date);
            addSession.Parameters.AddWithValue("@Shown", sessionModel.CardsShown);
            addSession.Parameters.AddWithValue("@Score", sessionModel.Score);
            addSession.ExecuteNonQuery();
        }
    }

    public static List<SessionModel> GetSessions()
    {
        List<SessionModel> sessions = new();
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand getAllSessions = connection.CreateCommand();
            getAllSessions.CommandText = @"SELECT study_session.id, users.name, decks.name, study_session.pack_size,
                                                  study_session.date, study_session.cards_shown
                                           FROM dbo.study_session
                                           INNER JOIN users ON study_session.user_id = users.id
                                           INNER JOIN decks ON study_session.pack_id = decks.id";

            SqlDataReader reader = getAllSessions.ExecuteReader();
            while (reader.Read())
            {
                sessions.Add(new SessionModel
                {
                    Id = reader.GetInt32(0),
                    Player = reader.GetString(1),
                    Pack = reader.GetString(2),
                    PackSize = reader.GetInt32(3),
                    Date = reader.GetDateTime(4),
                    CardsShown = reader.GetInt32(5)
                });
            }
        }
        return sessions;
    }

    public static List<SessionModel> GetSessions(string name)
    {
        List<SessionModel> sessions = new();
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand getSessions = connection.CreateCommand();
            getSessions.CommandText = @"SELECT study_session.id, users.name, decks.name, study_session.pack_size,
                                               study_session.date, study_session.cards_shown
                                           FROM dbo.study_session
                                           INNER JOIN users ON study_session.user_id = users.id
                                           INNER JOIN decks ON study_session.pack_id = decks.id
                                           WHERE users.name = @Name;";
            getSessions.Parameters.AddWithValue("@Name", name);
            SqlDataReader reader = getSessions.ExecuteReader();
            while (reader.Read())
            {
                sessions.Add(new SessionModel
                {
                    Id = reader.GetInt32(0),
                    Player = reader.GetString(1),
                    Pack = reader.GetString(2),
                    PackSize = reader.GetInt32(3),
                    Date = reader.GetDateTime(4),
                    CardsShown = reader.GetInt32(5)
                });
            }
        }
        return sessions;
    }

    public static List<SessionModel> GetSessions(DateTime startRange, DateTime endRange)
    {
        List< SessionModel> sessions = new();
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand getSessions = connection.CreateCommand();
            getSessions.CommandText = @"SELECT study_session.id, users.name, decks.name, study_session.pack_size,
                                               study_session.date, study_session.cards_shown
                                        FROM dbo.study_session
                                        INNER JOIN users ON study_session.user_id = users.id
                                        INNER JOIN decks ON study_session.pack_id = decks.id
                                        WHERE study_session.date BETWEEN @Start AND @End;";
            getSessions.Parameters.AddWithValue("@Start", startRange);
            getSessions.Parameters.AddWithValue("@End", endRange);
            SqlDataReader reader = getSessions.ExecuteReader();
            while(reader.Read())
            {
                sessions.Add(new SessionModel
                {
                    Id = reader.GetInt32(0),
                    Player = reader.GetString(1),
                    Pack = reader.GetString(2),
                    PackSize = reader.GetInt32(3),
                    Date = reader.GetDateTime(4),
                    CardsShown = reader.GetInt32(5)
                });
            }
        }
        return sessions;
    }

    public static List<SessionModel> GetSessions(DateTime startRange, DateTime endRange, string user)
    {
        List<SessionModel> sessions = new();
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand getSessions = connection.CreateCommand();
            getSessions.CommandText = @"SELECT study_session.id, users.name, decks.name, study_session.pack_size,
                                               study_session.date, study_session.cards_shown
                                        FROM dbo.study_session
                                        INNER JOIN users ON study_session.user_id = users.id
                                        INNER JOIN decks ON study_session.pack_id = decks.id
                                        WHERE study_session.date BETWEEN @Start AND @End
                                        AND users.name = @Name;";
            getSessions.Parameters.AddWithValue("@Start", startRange);
            getSessions.Parameters.AddWithValue("@End", endRange);
            getSessions.Parameters.AddWithValue("@Name", user);
            SqlDataReader reader = getSessions.ExecuteReader();
            while (reader.Read())
            {
                sessions.Add(new SessionModel
                {
                    Id = reader.GetInt32(0),
                    Player = reader.GetString(1),
                    Pack = reader.GetString(2),
                    PackSize = reader.GetInt32(3),
                    Date = reader.GetDateTime(4),
                    CardsShown = reader.GetInt32(5)
                });
            }
        }
        return sessions;
    }

    public static List<SessionModel> GetSessions(PackModel pack)
    {
        List<SessionModel> sessions = new();
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand getSessions = connection.CreateCommand();
            getSessions.CommandText = @"SELECT study_session.id, users.name, decks.name, study_session.pack_size,
                                               study_session.date, study_session.cards_shown
                                        FROM dbo.study_session
                                        INNER JOIN users ON study_session.user_id = users.id
                                        INNER JOIN decks ON study_session.pack_id = decks.id
                                        WHERE decks.name = @Pack;";
            getSessions.Parameters.AddWithValue("@Pack", pack.Name);
            SqlDataReader reader = getSessions.ExecuteReader();
            while (reader.Read())
            {
                sessions.Add(new SessionModel
                {
                    Id = reader.GetInt32(0),
                    Player = reader.GetString(1),
                    Pack = reader.GetString(2),
                    PackSize = reader.GetInt32(3),
                    Date = reader.GetDateTime(4),
                    CardsShown = reader.GetInt32(5)
                });
            }
        }
        return sessions;
    }

    public static List<SessionModel> GetSessions(PackModel pack, string user)
    {
        List<SessionModel> sessions = new();
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand getSessions = connection.CreateCommand();
            getSessions.CommandText = @"SELECT study_session.id, users.name, decks.name, study_session.pack_size,
                                               study_session.date, study_session.cards_shown
                                        FROM dbo.study_session
                                        INNER JOIN users ON study_session.user_id = users.id
                                        INNER JOIN decks ON study_session.pack_id = decks.id
                                        WHERE decks.name = @Pack
                                        AND users.name = @Name;";
            getSessions.Parameters.AddWithValue("@Pack", pack.Name);
            getSessions.Parameters.AddWithValue("@Name", user);
            SqlDataReader reader = getSessions.ExecuteReader();
            while (reader.Read())
            {
                sessions.Add(new SessionModel
                {
                    Id = reader.GetInt32(0),
                    Player = reader.GetString(1),
                    Pack = reader.GetString(2),
                    PackSize = reader.GetInt32(3),
                    Date = reader.GetDateTime(4),
                    CardsShown = reader.GetInt32(5)
                });
            }
        }
        return sessions;
    }
}
