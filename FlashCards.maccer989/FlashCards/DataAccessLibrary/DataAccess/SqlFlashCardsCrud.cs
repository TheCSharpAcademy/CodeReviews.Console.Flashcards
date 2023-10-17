using DataAccessLibrary.Models;

namespace DataAccessLibrary.DataAccess
{
    public class SqlFlashCardsCrud
    {
        private readonly string _connectionString;
        private SqlDataAccess db = new SqlDataAccess();
        private List<FlashCardsModel> output;

        public SqlFlashCardsCrud(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void CreateFlashCard(FlashCardsModel card)
        {
            string sql = "insert into FlashCards (StackName, FlashCardId, Question, Answer) values (@StackName, @FlashCardId, @Question, @Answer);";
            db.SaveData(sql,
                        new { card.StackName, card.FlashCardId, card.Question, card.Answer },
                        _connectionString);
        }
        public List<FlashCardsModel> GetAllFlashCardsByStackId()
        {
            string sql = "select * from FlashCards";
            var checkQuery = db.LoadData<FlashCardsModel, dynamic>(sql, new { }, _connectionString);
            if (checkQuery.Count > 0)
            {
                output = db.LoadData<FlashCardsModel, dynamic>(sql, new { }, _connectionString);
            }
            else
            {
                Console.WriteLine($"There are no Flashcards.");
            }
            return output;            
        }
        public bool CheckStackExists(int stackName)
        {
            bool output = false;
            string sql = $"select * from Stacks where StackName = {stackName};";
            var checkQuery = db.LoadData<StacksModel, dynamic>(sql,
                new { StackName = stackName, },
                _connectionString);

            if (checkQuery.Count > 0)
            {
                Console.WriteLine($"Flashcard with Stack name {stackName} exists.");
                output = true;
            }
            else
            {
                Console.WriteLine($"Flashcard with Stack name {stackName} does not exist.");
            }
            return output;
        }
        public bool CheckRecordExists(int stackName, int flashCardId)
        {
            bool output = false;
            string sql = $"select * from FlashCards where StackName = {stackName} and FlashCardId = {flashCardId};";
            var checkQuery = db.LoadData<StacksModel, dynamic>(sql,
                new { StackName = stackName, FlashCardId = flashCardId },
                _connectionString);

            if (checkQuery.Count > 0)
            {
                Console.WriteLine($"Flashcard with Stack name {stackName} and Flashcard Id {flashCardId} exists.");
                output = true;
            }
            else
            {
                Console.WriteLine($"Flashcard with Stack name {stackName} and Flashcard Id {flashCardId} does not exist.");
            }
            return output;
        }
        public void UpdateFlashCard(int stackName, int flashCardId, string cardQuestion, string cardAnswer)
        {
            string sql = $"update FlashCards set StackName = '{stackName}', FlashCardId = '{flashCardId}', Question = '{cardQuestion}', Answer = '{cardAnswer}' where StackName = '{stackName}' and FlashCardId = '{flashCardId}'";
            db.SaveData(sql, new { StackName = stackName, FlashCardId = flashCardId, Question = cardQuestion, Answer = cardAnswer }, _connectionString);
        }
        public void RemoveFlashCard(int stackName, int cardId)
        {
            string sql = $"select * from FlashCards where StackName = {stackName} and FlashCardId = {cardId};";
            var checkQuery = db.LoadData<FlashCardsModel, dynamic>(sql,
                new { StackName = stackName, FlashCardId = cardId },
                _connectionString);

            if (checkQuery.Count == 0)
            {
                Console.WriteLine($"\n\nCard with Id {cardId} doesnt exist.\n\n");
            }
            else
            {
                sql = $"delete from FlashCards where StackName = {stackName} and FlashCardId =  {cardId} update FlashCards set FlashCardId = FlashCardId - 1 WHERE FlashCardId > {cardId}";
                db.SaveData(sql, new { StackName = stackName, FlashCardId = cardId }, _connectionString);
            }
        }
    }
}