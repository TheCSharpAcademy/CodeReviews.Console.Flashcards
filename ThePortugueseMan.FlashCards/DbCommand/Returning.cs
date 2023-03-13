using Microsoft.Data.SqlClient;
using ObjectsLibrary;

namespace DbCommandsLibrary;

public class Returning
{
    string connectionString, cardsTableName, stacksTableName;

    public Returning(string connectionString, string cardsTableName ,string stacksTableName )
    {
        this.cardsTableName = cardsTableName;
        this.stacksTableName = stacksTableName;
        this.connectionString = connectionString;

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
                                Prompt = reader.GetString(1),
                                Answer= reader.GetString(2),
                                StackId= reader.GetInt32(3)
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
                                Name = reader.GetString(1)
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
                                Prompt = reader.GetString(1),
                                Answer = reader.GetString(2)
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
                                Name = reader.GetString(1)
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
}
