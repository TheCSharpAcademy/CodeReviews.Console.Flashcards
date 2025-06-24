using Dapper;
using Utilities.GoldRino456;
using Flashcards.GoldRino456.Database.Models;

namespace Flashcards.GoldRino456.Database.Controllers
{
    internal class FlashcardController : IDatabaseTable<Flashcard>
    {
        private readonly string _connectionString;

        public FlashcardController(string connectionString)
        {
            _connectionString = connectionString;
            CreateTable();
        }

        private void CreateTable()
        {
            string createQuery = "IF OBJECT_ID(N'Cards', N'U') IS NULL CREATE TABLE Cards (cardId INT IDENTITY(1,1) PRIMARY KEY, stackId INT FOREIGN KEY REFERENCES Stacks(stackID), frontOfCard VARCHAR(500), backOfCard VARCHAR(500));";

            DatabaseUtils.ExecuteNonQueryCommand(_connectionString, createQuery);
        }

        public void CreateEntry(Flashcard newEntry)
        {
            string createQuery = "INSERT INTO Cards (stackId, frontOfCard, backOfCard) VALUES (@StackId, @FrontOfCard, @BackOfCard);";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@StackId", newEntry.StackId);
            parameters.Add("@FrontOfCard", newEntry.FrontOfCard);
            parameters.Add("@BackOfCard", newEntry.BackOfCard);

            DatabaseUtils.ExecuteNonQueryCommand(_connectionString, createQuery, parameters);
        }

        public void DeleteEntry(int id)
        {
            string deleteQuery = "DELETE FROM Cards WHERE cardId = @Id;";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            DatabaseUtils.ExecuteNonQueryCommand(_connectionString, deleteQuery, parameters);
        }

        public List<Flashcard> ReadAllEntries()
        {
            string readQuery = "SELECT * FROM Cards";

            return DatabaseUtils.ExecuteQueryCommand<Flashcard>(_connectionString, readQuery);
        }

        public List<Flashcard> ReadAllEntriesFromStack(int stackId)
        {
            string readQuery = "SELECT * FROM Cards WHERE stackId = @StackId";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@StackId", stackId);

            return DatabaseUtils.ExecuteQueryCommand<Flashcard>(_connectionString, readQuery, parameters);
        }

        public Flashcard ReadEntry(int id)
        {
            string readQuery = "SELECT * FROM Cards WHERE cardId = @Id";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return DatabaseUtils.ExecuteQueryCommand<Flashcard>(_connectionString, readQuery, parameters).First();
        }

        public void UpdateEntry(int id, Flashcard updatedEntry)
        {
            string updateQuery = "UPDATE Cards SET stackId = @StackId, frontOfCard = @FrontOfCard, backOfCard = @BackOfCard WHERE cardId = @Id";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@StackId", updatedEntry.StackId);
            parameters.Add("@FrontOfCard", updatedEntry.FrontOfCard);
            parameters.Add("@BackOfCard", updatedEntry.BackOfCard);

            DatabaseUtils.ExecuteNonQueryCommand(_connectionString, updateQuery, parameters);
        }
    }
}
