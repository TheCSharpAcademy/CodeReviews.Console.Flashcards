using Microsoft.Data.SqlClient;
using ObjectsLibrary;

namespace DbCommandsLibrary;

public class Returning
{
    string connectionString, cardsTableName, stacksTableName, studySessionsTableName;

    public Returning(string connectionString, string cardsTableName ,string stacksTableName, string studySessionsTableName )
    {
        this.connectionString = connectionString;
        this.cardsTableName = cardsTableName;
        this.stacksTableName = stacksTableName;
        this.studySessionsTableName = studySessionsTableName;
    }
    public List<Card> AllCards()
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"SELECT * FROM {this.cardsTableName}";

                SqlDataReader reader;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    reader = command.ExecuteReader();
                }
                if (reader.HasRows)
                {
                    var returnList = new List<Card>();
                    while (reader.Read())
                    {
                        returnList.Add(
                            new Card
                            {
                                Id = reader.GetInt32(0),
                                ViewId = reader.GetInt32(1),
                                Prompt = reader.GetString(2),
                                Answer= reader.GetString(3),
                                StackId= reader.GetInt32(4)
                            });
                    }
                    connection.Close();
                    return returnList;
                }
                else return null;
            }
        }
        catch (SqlException) { return null; }
    }

    public List<Card> CardsByStackId(int stackId)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"SELECT * FROM {this.cardsTableName} WHERE StackID = {stackId}";

                SqlDataReader reader;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    reader = command.ExecuteReader();
                }
                if (reader.HasRows)
                {
                    var returnList = new List<Card>();
                    while (reader.Read())
                    {
                        returnList.Add(
                            new Card
                            {
                                Id = reader.GetInt32(0),
                                ViewId = reader.GetInt32(1),
                                Prompt = reader.GetString(2),
                                Answer = reader.GetString(3),
                                StackId = reader.GetInt32(4)
                            });
                    }
                    connection.Close();
                    return returnList;
                }
                else return null;
            }
        }
        catch (SqlException) { return null; }
    }

    public List<Stack> AllStacks()
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"SELECT * FROM {this.stacksTableName}";

                SqlDataReader reader;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    reader = command.ExecuteReader();
                }
                if (reader.HasRows)
                {
                    var returnList = new List<Stack>();
                    while (reader.Read())
                    {
                        returnList.Add(
                            new Stack
                            {
                                Id = reader.GetInt32(0),
                                ViewId = reader.GetInt32(1),
                                Name = reader.GetString(2)
                            });
                    }
                    connection.Close();
                    return returnList;
                }
                else return null;
            }
        }
        catch (SqlException) { return null; }
    }

    public List<StudySession> AllSessions()
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"SELECT * FROM {this.studySessionsTableName}";

                SqlDataReader reader;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    reader = command.ExecuteReader();
                }
                if (reader.HasRows)
                {
                    var returnList = new List<StudySession>();
                    while (reader.Read())
                    {
                        returnList.Add(
                            new StudySession
                            {
                                Id = reader.GetInt32(0),
                                Date = reader.GetDateTime(1),
                                Score = reader.GetInt32(2),
                                RoundsPlayed = reader.GetInt32(3),
                                StackId= reader.GetInt32(4),
                            });
                    }
                    connection.Close();
                    return returnList;
                }
                else return null;
            }
        }
        catch (SqlException) { return null; }
    }

    public Card CardByIndex(int index) 
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"SELECT * FROM {this.cardsTableName} WHERE Id={index}";

                SqlDataReader reader;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    reader = command.ExecuteReader();
                }
                if (reader.HasRows)
                {
                    Card returnCard = new();
                    while (reader.Read())
                    {
                        returnCard =
                            new Card
                            {
                                Id = reader.GetInt32(0),
                                ViewId = reader.GetInt32(1),
                                Prompt = reader.GetString(2),
                                Answer = reader.GetString(3),
                                StackId = reader.GetInt32(4)
                            };
                    }
                    connection.Close();
                    return returnCard;
                }
                else return null;
            }
        }
        catch (SqlException) { return null; }
    }

    public Stack StackByIndex(int index) 
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"SELECT * FROM {this.stacksTableName} WHERE Id={index}";

                SqlDataReader reader;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    reader = command.ExecuteReader();
                }
                if (reader.HasRows)
                {
                    Stack returnStack = new();
                    while (reader.Read())
                    {
                        returnStack = 
                            new Stack
                            {
                                Id = reader.GetInt32(0),
                                ViewId = reader.GetInt32(1),
                                Name = reader.GetString(2)
                            };
                    }
                    connection.Close();
                    return returnStack;
                }
                else return null;
            }
        }
        catch (SqlException) { return null; }
    }


    public int ViewIdFromId(string table, int id)
    {
        if (table == "Stacks") return StackByIndex(id).ViewId;
        else if (table == "Cards") return CardByIndex(id).ViewId;
        else throw new Exception("Invalid mode for ViewIdFromId");
    }

    public int IdFromViewId(string tableName, int viewId)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"SELECT * FROM {tableName} WHERE ViewId={viewId}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            int id = reader.GetInt32(0);
                            connection.Close();
                            return id;
                        }
                        else return 0;
                    }
                }
            }
        }
        catch (SqlException) { return 0; }
    }

    public int LastViewId(string tableName)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.ConnectionString = connectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql =
                $"SELECT TOP (1) * FROM {tableName} ORDER BY [ViewId] DESC";
                    //$"SELECT TOP (1) ViewId FROM {tableName} ORDER BY ViewId DESC";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            int viewId = reader.GetInt32(1);
                            connection.Close();
                            return viewId;
                        }
                        else return 0;
                    }
                }
            }
        }
        catch (SqlException) { return -1; }
    }
}
