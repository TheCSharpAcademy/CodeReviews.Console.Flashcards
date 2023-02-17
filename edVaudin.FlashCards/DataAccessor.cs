using Flashcards.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Flashcards
{
    internal class DataAccessor
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["flashcardApp"].ConnectionString;

        public void AddStack(string name)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "INSERT INTO stacks (name) VALUES (@name);";
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();
        }

        public void DeleteStack(string name)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "DELETE FROM stacks WHERE name = @name;";
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();
        }

        public void DeleteFlashcard(string prompt)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "DELETE FROM flashcards WHERE prompt = @prompt;";
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@prompt", prompt);
            cmd.ExecuteNonQuery();
        }

        public List<StackDTO> GetStacks()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "SELECT name FROM stacks;";
            SqlCommand cmd = new(sql, conn);
            return GetQueriedList(cmd, reader => new StackDTO(reader));
        }

        public StackDTO GetStackById(int id)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "SELECT TOP 1 name FROM stacks WHERE id = @id;";
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            return GetQueriedResult(cmd, reader => new StackDTO(reader));
        }

        public List<FlashcardDTO> GetFlashcardsInStack(string name)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = @"SELECT prompt, answer FROM flashcards 
                               JOIN stacks ON stacks.id = flashcards.stack_id 
                               WHERE stacks.name = @name;";
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@name", name);
            return GetQueriedList(cmd, reader => new FlashcardDTO(reader));
        }

        protected static List<T> GetQueriedList<T>(SqlCommand cmd, Func<SqlDataReader, T> creator)
        {
            List<T> results = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(creator(reader));
                }
            }
            return results;
        }

        protected static T? GetQueriedResult<T>(SqlCommand cmd, Func<SqlDataReader, T> creator) where T : new()
        {
            T result = new();
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows) { return default(T); }
                while (reader.Read())
                {
                    result = creator(reader);
                }
            }
            return result;
        }

        public void AddFlashcard(int id, string prompt, string answer)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "INSERT INTO flashcards (stack_id, prompt, answer) VALUES (@stack_id, @prompt, @answer);";
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@stack_id", id);
            cmd.Parameters.AddWithValue("@prompt", prompt);
            cmd.Parameters.AddWithValue("@answer", answer);
            cmd.ExecuteNonQuery();
        }

        public void AddStudy(string stackName, DateTime date, float score)
        {
            int stackId = GetStackId(stackName);
            if (stackId == -1)
            {
                Console.WriteLine($"We could not find a stack by the name of {stackName}");
                return;
            }
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "INSERT INTO studies (stack_id, date, score) VALUES (@stack_id, @date, @score);";
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@stack_id", stackId);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@score", score);
            cmd.ExecuteNonQuery();
        }

        public int GetStackId(string stackName)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id FROM stacks WHERE name = @name;";
                SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@name", stackName);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return reader.GetInt32(0);
                }
            }
            return -1;
        }

        public List<StudyDTO> GetStudies()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "SELECT stacks.name, studies.date, studies.score FROM studies " +
                         "JOIN stacks ON stacks.id = studies.stack_id;";
            SqlCommand cmd = new(sql, conn);
            return GetQueriedList(cmd, reader => new StudyDTO(reader));
        }

        internal void UpdateStackName(string newName, string oldName)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "UPDATE stacks SET name = @newName WHERE name = @oldName;";
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@newName", newName);
            cmd.Parameters.AddWithValue("@oldName", oldName);
            cmd.ExecuteNonQuery();
        }

        internal void UpdateFlashcard(string newPrompt, string newAnswer, string oldPrompt)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "UPDATE flashcards SET prompt = @newPrompt, answer = @newAnswer WHERE prompt = @oldPrompt;";
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@newPrompt", newPrompt);
            cmd.Parameters.AddWithValue("@oldPrompt", oldPrompt);
            cmd.Parameters.AddWithValue("@newAnswer", newAnswer);
            cmd.ExecuteNonQuery();
        }
    }
}
