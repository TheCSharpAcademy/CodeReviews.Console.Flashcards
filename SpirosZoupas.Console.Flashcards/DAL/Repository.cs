using Dapper;
using Flashcards.DAL.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace Flashcards.DAL
{
    public class Repository
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["flashcards"].ConnectionString;

        public bool ExecuteQuery(string sql, object? parameters = null)
        {
            int success;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                success = connection.Execute(sql, parameters);
                connection.Close();
            }

            return success != 0;
        }

        public bool Insert<T>(string tableName, T model)
        {
            var properties = typeof(T)
                .GetProperties()
                .ToList();

            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var paramNames = string.Join(", ", properties.Select(p => "@" + p.Name));

            string sql = $@"
            INSERT INTO 
                {tableName} ({columnNames})
            VALUES 
                ({paramNames})";

            return ExecuteQuery(sql, model);
        }

        public bool Update<T>(string tableName, T model)
        {
            var properties = typeof(T)
                .GetProperties()
                .ToList();

            var keyProperty = properties.First(p => p.Name.Equals("ID"));

            properties = properties
                .Where(p => p != keyProperty)
                .ToList();

            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            string sql = $@"
            UPDATE 
                {tableName}
            SET 
                {setClause}
            WHERE
                {keyProperty.Name} = @{keyProperty.Name}";

            return ExecuteQuery(sql, model);
        }

        public bool Delete(string tableName, int id)
        {
            var parameters = new { Id = id };

            string sql = $@"
            DELETE FROM
                {tableName}
            WHERE
                ID = @ID";

            return ExecuteQuery(sql, parameters);
        }

        public T ExecuteQuerySingle<T>(string sql, object parameters)
        {
            T result;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                result = connection.QuerySingle<T>(sql, parameters);
                connection.Close();
            }

            return result;
        }

        public FlashcardStackDTO GetFlashcardByID(int id)
        {
            var parameters = new { Id = id };

            string sql = $@"
            SELECT
                Flashcard.ID,
                Flashcard.Front,
                Flashcard.Back,
                Stack.Name AS StackName
            FROM
                Flashcard INNER JOIN
                Stack ON Stack.ID = Flashcard.StackID
            WHERE
                Flashcard.ID = @ID";


            return ExecuteQuerySingle<FlashcardStackDTO>(sql, parameters);
        }

        public StackDTO GetStackByName(string name)
        {
            List<FlashcardStackDTO> flashCards = new List<FlashcardStackDTO>();
            var parameters = new { Name = name };
            string sql = $@"
            SELECT
                Flashcard.Front,
                Flashcard.Back,
                Stack.Name AS StackName
            FROM
                Flashcard INNER JOIN
                Stack ON Stack.ID = Flashcard.StackID
            WHERE
                Stack.Name = @Name";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                flashCards = connection.Query<FlashcardStackDTO>(sql).ToList();
                connection.Close();
            }

            return new StackDTO
            {
                Name = flashCards.First().StackName,
                FlashCards = flashCards
            };
        }

        public int GetStackIDByName(string name)
        {
            int id;
            var parameters = new { Name = name };
            string sql = $@"
            SELECT
                Stack.ID
            FROM
                Stack
            WHERE
                Stack.Name = @Name";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                id = connection.ExecuteScalar<int>(sql);
                connection.Close();
            }

            return id;
        }

        public List<FlashcardStackDTO> GetAllFlashcards()
        {
            List<FlashcardStackDTO> flashCards = new List<FlashcardStackDTO>();
            string sql = $@"
            SELECT
                Flashcard.Front,
                Flashcard.Back,
                Stack.Name AS StackName
            FROM
                Flashcard INNER JOIN
                Stack ON Stack.ID = Flashcard.StackID";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                flashCards = connection.Query<FlashcardStackDTO>(sql).ToList();
                connection.Close();
            }

            return flashCards;
        }

        public List<StackDTO> GetAllStacks()
        {
            List<FlashcardStackDTO> flashCards = new List<FlashcardStackDTO>();
            List<StackDTO> stacks = new List<StackDTO>();
            string sql = $@"
            SELECT
                Flashcard.Front,
                Flashcard.Back,
                Stack.Name AS StackName
            FROM
                Flashcard INNER JOIN
                Stack ON Stack.ID = Flashcard.StackID";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                flashCards = connection.Query<FlashcardStackDTO>(sql).ToList();
                connection.Close();
            }

            List<FlashcardStackDTO> flashCardsInStack = new List<FlashcardStackDTO>();
            foreach (var fc in flashCards)
            {
                if (stacks.Where(s => s.Name == fc.StackName).IsNullOrEmpty())
                {
                    stacks.Add(new StackDTO { Name = fc.StackName, FlashCards = new List<FlashcardStackDTO>() });
                }

                stacks.Where(s => s.Name == fc.StackName).ToList().ForEach(s => s.FlashCards.Add(fc));
            }

            return stacks;
        }
    }
}
