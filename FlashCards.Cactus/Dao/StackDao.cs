using FlashCards.Cactus.DataModel;
using Microsoft.Data.SqlClient;

namespace FlashCards.Cactus.Dao;

public class StackDao
{
    public StackDao(string DBConnStr)
    {
        DBConnectionStr = DBConnStr;
    }

    public string DBConnectionStr { get; set; }

    public int Insert(Stack stack)
    {
        int rowsAffected = -1;
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO Stack(name) VALUES('{stack.Name}');";
            rowsAffected = command.ExecuteNonQuery();
        }
        return rowsAffected;
    }

    public List<Stack> FindAll()
    {
        List<Stack> stacks = new List<Stack>();
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Stack";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Stack stack = new Stack(reader.GetInt32(0), reader.GetString(1));
                    stacks.Add(stack);
                }
            }
        }
        return stacks;
    }

    public int Delete(Stack stack)
    {
        int rowsAffected = -1;
        using (var connection = new SqlConnection(DBConnectionStr))
        {
            connection.Open();
            string deleteSql = @"DELETE FROM Stack WHERE name = @name";
            SqlCommand deleteCommand = new SqlCommand(deleteSql, connection);
            deleteCommand.Parameters.AddWithValue("@name", stack.Name);
            rowsAffected = deleteCommand.ExecuteNonQuery();
        }
        return rowsAffected;
    }
}

