using System.Data;
using Dapper;
using FlashcardGame.Helpers;
using FlashcardGame.Models;

namespace FlashcardGame
{

    internal class DataAccess
    {
        public static List<Flashcard> GetFlashcards(int stackId)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("FlashcardDatabase")))
            {
                var output = connection.Query<Flashcard>($"select * from Stack{stackId}Table").ToList();
                return output;
            }
        }
        public static List<Stack> GetStacks() 
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("FlashcardDatabase")))
            {
                var output = connection.Query<Stack>($"select * from StacksTable").ToList();
                return output;
            }
        }
        public static List<StudySession> GetStudySessions()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("FlashcardDatabase")))
            {
                var output = connection.Query<StudySession>($"select * from StudySessionsTable").ToList();
                return output;
            }
        }

    } 
}
