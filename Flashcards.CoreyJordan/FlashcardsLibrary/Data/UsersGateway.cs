using System.Data.SqlClient;

namespace FlashcardsLibrary.Data;
public class UsersGateway : ConnManager
{
    public static void CreateUser(string player)
    {
        using (SqlConnection connection = new(FlashCardDb))
        {
            
                connection.Open();
                SqlCommand createUser = connection.CreateCommand();
                createUser.CommandText = @"IF NOT EXISTS (SELECT name FROM dbo.users
                                        WHERE name = @Name)
                                        INSERT INTO dbo.users (name)
                                        VALUES (@Name);";
                createUser.Parameters.AddWithValue("@Name", player);
                createUser.ExecuteNonQuery();
        }
    }

    public static List<string> GetAllUsers()
    {
        List<string> users = new();
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand getAllUsers = connection.CreateCommand();
            getAllUsers.CommandText = @"SELECT name FROM dbo.users;";
            SqlDataReader reader = getAllUsers.ExecuteReader();
            while (reader.Read())
            {
                users.Add(reader.GetString(0).TrimEnd());
            }
        }
        return users;
    }
}
