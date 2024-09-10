using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Flashcards;


namespace Flashcards
{
    public class FlashcardsApp
    {
        public string connectionString = "Server=localhost;Database=FLASHCARDS;User ID=SA;Password=Password123;";

        public List<FlashcardsDTO> GetFlashcardsByStack(int stackID){

                using(SqlConnection connection = new SqlConnection(connectionString)){
                    connection.Open();

                    string query = "SELECT Question, Answer FROM dbo.FLASHCARD WHERE StackID = @StackID";

                    var flashcards = connection.Query<FlashcardsDTO>(query, new{StackID = stackID}).ToList();

                    return flashcards;
                }
        }
        
    }
}