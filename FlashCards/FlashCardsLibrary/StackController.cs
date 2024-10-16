using FlashCardsLibrary.Models;
using System.Data.SqlClient;

namespace FlashCardsLibrary
{
    public static class StackController
    {
        private static string _connectionString = Database._connectionString;
        public static List<Stack> GetStackNames()
        {
            var list = new List<Stack>();
            string command = @"SELECT * FROM Stacks ";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(command, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new Stack((string)reader["StackName"]));
                    }
                }
            }
            return list;
        }
        public static void DeleteStack(Stack stack)
        {
            if (!GetStackNames().Contains(stack))
            {
                Console.WriteLine($"You dont have stack with name {stack.Name}");
                Console.ReadLine();
            }
            string deleteCmd = @"DELETE FROM Stacks WHERE StackName = @name";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(deleteCmd, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("name", stack.Name);
                        cmd.ExecuteReader();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error deleting stack {e}");
                        Console.ReadLine();
                    }
                }
            }
        }
        public static void UpdateStack(Stack oldStack, Stack newStack)
        {
            if (!GetStackNames().Contains(oldStack))
            {
                Console.WriteLine($"No Stack with name :{oldStack.Name}");
                Console.ReadLine();
                return;
            }
            string updateCmd = @"UPDATE Stacks SET StackName = @newName WHERE StackName = @name";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(updateCmd, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("name", oldStack.Name);
                        cmd.Parameters.AddWithValue("newName", newStack.Name);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error Updating stack : {e}");
                        Console.ReadLine();
                    }
                }
            }
        }
        public static void AddStack(Stack stack)
        {
            string insert = @"INSERT INTO Stacks(StackName) VALUES (@name)";
            if (GetStackNames().Contains(stack))
            {
                Console.WriteLine("Already have it");
                Console.ReadLine();
                return;
            }
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(insert, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("name", stack.Name);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error Adding Stack: {e}");
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}
