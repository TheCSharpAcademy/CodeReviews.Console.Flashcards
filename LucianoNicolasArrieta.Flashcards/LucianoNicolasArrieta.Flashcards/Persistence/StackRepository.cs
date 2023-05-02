using LucianoNicolasArrieta.Flashcards.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucianoNicolasArrieta.Flashcards.Persistence
{
    public class StackRepository
    {
        public void Insert(Stack stack)
        {

            using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                string query = "INSERT INTO Stack (Subject) VALUES (@subject)";
                SqlCommand command = new SqlCommand(query, myConnection);

                myConnection.Open();

                command.Parameters.AddWithValue("@subject", stack.Subject);
                command.ExecuteNonQuery();    
            }

            Console.Clear();
            Console.WriteLine("Stack added to database successfully!");
        }
    }
}
