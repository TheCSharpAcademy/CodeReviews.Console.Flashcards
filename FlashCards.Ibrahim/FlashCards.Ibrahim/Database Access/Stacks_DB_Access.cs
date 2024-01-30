using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Ibrahim.Models;
using static System.Net.Mime.MediaTypeNames;

namespace FlashCards.Ibrahim.Database_Access
{
    public class Stacks_DB_Access
    {
        static string _connectionString;
        public Stacks_DB_Access(string connectionString)
        {
            _connectionString = connectionString;
        }
        public static void Insert_Stack(string name)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = @"
                        INSERT INTO Stacks_Table (name) 
                        VALUES (@Name)";
                command.Parameters.AddWithValue("@Name", name);
                command.ExecuteNonQuery();
            }
        }
        public static void Update_Stack(int Id, string name)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();

                command.CommandText = @"
                        UPDATE Stacks_Table Set Name = @Name WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Name", name);
                command.ExecuteNonQuery();
            }      
        }
        public static void Delete_Stack(int Id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                {
                    command.CommandText = @"
                        DELETE FROM Stacks_Table  WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static List<Stacks> GetAllStacks()
        {
            List<Stacks> stacks = new List<Stacks>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = @"SELECT * FROM Stacks_Table";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Stacks stack = new Stacks();
                        stack.ID = reader.GetInt32(0);
                        stack.Name = reader.GetString(1);
                        stacks.Add(stack);
                    }
                }
            }
            return stacks;
        }
        public static Stacks GetOneStack(string name)
        {
            Stacks stack = new Stacks();

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = @"SELECT * FROM Stacks_Table WHERE Name = @Name";
                command.Parameters.AddWithValue("@Name", name);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stack.ID = reader.GetInt32(0);
                        stack.Name = reader.GetString(1);
                    }
                }
            }
            return stack;
        }
    }
}
