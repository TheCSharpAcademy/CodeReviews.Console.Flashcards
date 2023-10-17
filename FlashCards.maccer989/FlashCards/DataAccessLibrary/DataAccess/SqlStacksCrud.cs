using DataAccessLibrary.Models;

namespace DataAccessLibrary.DataAccess
{
    public class SqlStacksCrud
    {
        private readonly string _connectionString;
        private SqlDataAccess db = new SqlDataAccess();
        private List<StacksModel> output;
        public SqlStacksCrud(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void CreateStack(StacksModel stack)
        {
            string sql = "insert into Stacks (StackName) values (@StackName);";
            db.SaveData(sql,
                        new { stack.StackName},
                        _connectionString);
        }
        public List<StacksModel> GetAllStacks()
        {            
            string sql = "select * from Stacks";
            var checkQuery = db.LoadData<StacksModel, dynamic>(sql, new { }, _connectionString);
            if (checkQuery.Count > 0)
            {
                output = db.LoadData<StacksModel, dynamic>(sql, new { }, _connectionString);
            }
            else
            {
                Console.WriteLine($"There are no stacks.");
            }
            return output;
        }
        public bool CheckRecordExists(int recordId)
        {
            bool output = false;
            string sql = $"select StackName from Stacks where StackName = {recordId};";
            var checkQuery = db.LoadData<StacksModel, dynamic>(sql,
                new { Id = recordId },
                _connectionString);

            if (checkQuery.Count > 0)
            {
                Console.WriteLine($"Stack with name {recordId} exists.");
                output = true;
            }
            else
            {
                Console.WriteLine($"Stack with name {recordId} does not exist.");
            }
            return output;
        }
        public void UpdateStack(int recordId, int stackName)
        {
            string sql = $"select * from Stacks where StackName = {recordId};";
            var checkQuery = db.LoadData<StacksModel, dynamic>(sql,
                new { Id = recordId },
                _connectionString);

            if (checkQuery.Count == 0)
            {
                Console.WriteLine($"\nRecord with StackName {recordId} doesnt exist.\n");
            }
            else
            {
                sql = $"update Stacks set StackName = '{stackName}' where StackName = '{recordId}'";
                
                db.SaveData(sql, recordId, _connectionString);
            }
        }
        public void RemoveStack(int stackId)
        {
            string sql = $"select * from Stacks where StackName = {stackId};";
            var checkQuery = db.LoadData<StacksModel, dynamic>(sql,
                new { StackName = stackId },
                _connectionString);

            if (checkQuery.Count == 0)
            {
                Console.WriteLine($"\nStack with StackName {stackId} doesnt exist.\n");
            }
            else
            {
                sql = $"delete from FlashCards where StackName = {stackId} delete from Stacks where StackName = {stackId} delete from StudySessions where StackName = {stackId}";
                db.SaveData(sql, new { StackName = stackId }, _connectionString);
            }
        }
    }
}