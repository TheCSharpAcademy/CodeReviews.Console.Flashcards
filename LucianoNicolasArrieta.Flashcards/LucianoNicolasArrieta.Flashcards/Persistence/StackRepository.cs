﻿using ConsoleTableExt;
using System.Data.SqlClient;
using Stack = LucianoNicolasArrieta.Flashcards.Model.Stack;

namespace LucianoNicolasArrieta.Flashcards.Persistence
{
    public class StackRepository
    {
        private string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");

        public void Insert(Stack stack)
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Stack (Subject) VALUES (@subject)";
                SqlCommand command = new SqlCommand(query, myConnection);

                myConnection.Open();

                command.Parameters.AddWithValue("@subject", stack.Subject);
                command.ExecuteNonQuery();    
            }

            Console.Clear();
            Console.WriteLine($"{stack.Subject} Stack created successfully!");
        }

        public void PrintAll()
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Stack";
                SqlCommand command = new SqlCommand(query, myConnection);

                myConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                var tableData = new List<List<object>>();
                int i = 1;
                while (reader.Read())
                {
                    Stack aux = new Stack(reader[1].ToString());
                    aux.Id = Convert.ToInt32(reader[0]);

                    List<object> row = new List<object> { $"{i}", reader[1].ToString() };
                    tableData.Add(row);
                    i++;
                }

                ConsoleTableBuilder.From(tableData)
                    .WithCharMapDefinition(
                        CharMapDefinition.FramePipDefinition,
                        new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                        })
                    .WithTitle("STACKS")
                    .ExportAndWriteLine(TableAligntment.Left);
            }
        }

        public Stack GetUIStack(int id)
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Stack";
                SqlCommand command = new SqlCommand(query, myConnection);

                myConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                List<Stack> stacks = new List<Stack>();
                while (reader.Read())
                {
                    Stack aux = new Stack(reader[1].ToString());
                    aux.Id = Convert.ToInt32(reader[0]);
                    stacks.Add(aux);
                }

                if (stacks.Count <= id-1)
                {
                    Menu menu = new Menu();
                    Console.Clear();
                    Console.WriteLine("Please enter a valid ID. Try again");
                    menu.ManageStacks();
                }
                return stacks[id - 1];
            }
        }

        public void Delete(int id)
        {
            Stack stack = GetUIStack(id);

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                string query = $"DELETE FROM Stack WHERE Id='{stack.Id}'";
                SqlCommand command = new SqlCommand(query, myConnection);

                myConnection.Open();

                command.ExecuteNonQuery();
            }

            Console.Clear();
            Console.WriteLine("Stack deleted successfully!");
        }

        public Stack GetStack(int id)
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                string query = $"SELECT * FROM Stack WHERE Id='{id}'";
                SqlCommand command = new SqlCommand(query, myConnection);

                myConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Stack selected = new Stack();
                while (reader.Read())
                {
                    selected.Subject = reader[1].ToString();
                    selected.Id = Convert.ToInt32(reader[0]);
                }

                return selected;
            }
        }
    }
}
