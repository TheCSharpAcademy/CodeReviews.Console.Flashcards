using ConsoleTableExt;
using Flashcards.Model;
using System.Data;
using System.Data.SqlClient;


namespace Flashcards.CRUD {
    internal static class DbOperations {

        private static SqlDataReader reader;
        private static SqlDataAdapter adapter = new SqlDataAdapter();

        static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("DbConnection");
        static string dbFilePath = System.Configuration.ConfigurationManager.AppSettings.Get("DbFilePath");
        static string dbName = System.Configuration.ConfigurationManager.AppSettings.Get("DbName");

        static SqlConnection cnn = new SqlConnection(connectionString);

        public static void Initialize() {
            CreateDatabase();
            CreateTables();
        }

        private static void CreateTables() {

            string createStacksTable =
                $@"IF NOT EXISTS (SELECT * FROM sysobjects
            WHERE name='Stack' and xtype='U')
            CREATE TABLE Stack (
                stack_id int NOT NULL IDENTITY PRIMARY KEY,
                name nvarchar(50) NOT NULL)";

            SqlCommand sqlCommandStacks = new SqlCommand(createStacksTable, cnn);

            string createCardsTable =
                $@"IF NOT EXISTS (SELECT * FROM sysobjects
            WHERE name='Cards' and xtype='U')
            CREATE TABLE Cards (
                card_id int NOT NULL IDENTITY PRIMARY KEY,
                front Text NOT NULL,
                back Text NOT NULL,
                stack_id int NOT NULL,
                CONSTRAINT FK_StackCard FOREIGN KEY (stack_id)
                REFERENCES Stack(stack_id)
                ON DELETE CASCADE
                ON UPDATE CASCADE)";

            SqlCommand sqlCommandCards = new SqlCommand(createCardsTable, cnn);

            string createStudySessionsTable =
                $@"IF NOT EXISTS (SELECT * FROM sysobjects
            WHERE name='StudySessions' and xtype='U')
            CREATE TABLE StudySessions (
                study_id int NOT NULL IDENTITY PRIMARY KEY,
                date Text NOT NULL,
                score Text NOT NULL,
                stack_id int NOT NULL,
                CONSTRAINT FK_StackStudySession FOREIGN KEY (stack_id)
                REFERENCES Stack(stack_id)
                ON DELETE CASCADE
                ON UPDATE CASCADE)";

            SqlCommand sqlCommandStudySessions = new SqlCommand(createStudySessionsTable, cnn);

            cnn.Open();
            sqlCommandStacks.ExecuteNonQuery();
            sqlCommandCards.ExecuteNonQuery();
            sqlCommandStudySessions.ExecuteNonQuery();

            if (cnn.State == ConnectionState.Open) {
                cnn.Close();
            }
        }

        private static void CreateDatabase() {

            string sqlString =
                $@"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{dbName}')
            BEGIN
                CREATE DATABASE {dbName} ON PRIMARY
                (NAME = {dbName}_Data, FILENAME = '{dbFilePath}{dbName}.mdf',
                SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)
                LOG ON (NAME = {dbName}_log,
                FILENAME = '{dbFilePath}{dbName}.ldf',
                SIZE = 1MB, MAXSIZE = 5MB, FILEGROWTH = 10%)
            END";

            SqlCommand sqlCommand = new SqlCommand(sqlString, cnn);
            cnn.Open();
            sqlCommand.ExecuteNonQuery();

            if (cnn.State == ConnectionState.Open) {
                cnn.Close();
            }
        }

        public static void AddStack( string op ) {
            Console.Clear();
            cnn.Open();


            string sql = $"Insert into Stack (name) values('{op}')";
            var command = new SqlCommand(sql, cnn);
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();

            command.Dispose();
            cnn.Close();
        }

        public static List<Stack> ShowStacksList() {

            List<Stack> stacks = new List<Stack>();

            cnn.Open();

            string sql = "SELECT * from Stack";

            var command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read()) {

                string id = reader.GetValue(0).ToString();
                string name = reader.GetValue(1).ToString();

                var stack = new Stack(id, name);
                stacks.Add(stack);
            }

            cnn.Close();
            return stacks;
        }

        public static void DeleteStack( Stack stack ) {
            cnn.Open();

            string sql = $"DELETE Cards where stack_id = {stack.Id}";

            var command = new SqlCommand(sql, cnn);
            adapter.DeleteCommand = command;
            adapter.DeleteCommand.ExecuteNonQuery();

            sql = $"DELETE StudySessions where stack_id = {stack.Id}";

            command = new SqlCommand(sql, cnn);
            adapter.DeleteCommand = command;
            adapter.DeleteCommand.ExecuteNonQuery();

            sql = $"DELETE Stack where stack_id = {stack.Id}";

            command = new SqlCommand(sql, cnn);
            adapter.DeleteCommand = command;
            adapter.DeleteCommand.ExecuteNonQuery();

            command.Dispose();
            cnn.Close();
        }

        public static void CreateCard( string front, string back, Stack stack ) {
            cnn.Open();

            string sql = $"Insert into Cards (front, back, stack_id) values('{front}', '{back}', {stack.Id})";
            var command = new SqlCommand(sql, cnn);
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();

            command.Dispose();
            cnn.Close();
        }

        public static List<CardDto> GetFlashcards( Stack stack ) {

            cnn.Open();

            List<CardDto> cards = new List<CardDto>();

            string sql = $"SELECT * from Cards WHERE stack_id = {stack.Id}";

            var command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read()) {
                var id = reader.GetValue(0).ToString();
                var front = reader.GetValue(1).ToString();
                var back = reader.GetValue(2).ToString();

                CardDto card = new CardDto(id, front, back);
                cards.Add(card);
            }
            cnn.Close();
            return cards;
        }

        internal static List<CardDto> GetFlashcardsWithId( Stack stack ) {
            int id = 1;

            Console.Clear();
            List<CardDto> cards = new List<CardDto>();

            cnn.Open();

            string sql = $"SELECT * from Cards WHERE stack_id = {stack.Id}";

            var command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read()) {

                var front = reader.GetValue(1).ToString();
                var back = reader.GetValue(2).ToString();

                CardDto card = new CardDto(id.ToString(), front, back);
                cards.Add(card);
                id++;
            }

            cnn.Close();
            return cards;
        }

        internal static void GetCard( CardDto card ) {

            cnn.Open();
            List<List<object>> table;

            string sql = $"SELECT * from Cards WHERE card_id = {card.Id}";

            var command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read()) {
                var front = reader.GetValue(1).ToString();
                var back = reader.GetValue(2).ToString();

                table = new List<List<object>> {
                    new List<object> { front, back }
                };

                ConsoleTableBuilder
               .From(table)
               .WithColumn("Front", "Back")
               .ExportAndWriteLine();
            }
            cnn.Close();
        }

        internal static void UpdateCardText( bool front, string nextText, CardDto card ) {

            cnn.Open();
            string sql;

            if (front == true) {
                sql = $"UPDATE Cards SET front = '{nextText}'  WHERE card_id = {card.Id}";
            } else {
                sql = $"UPDATE Cards SET back = '{nextText}'  WHERE card_id = {card.Id}";
            }

            var command = new SqlCommand(sql, cnn);
            adapter.UpdateCommand = command;
            adapter.UpdateCommand.ExecuteNonQuery();

            command.Dispose();

            cnn.Close();
        }

        internal static void DeleteCard( string id ) {

            cnn.Open();

            string sql = $"DELETE Cards where card_id = {id}";

            var command = new SqlCommand(sql, cnn);
            adapter.DeleteCommand = command;
            adapter.DeleteCommand.ExecuteNonQuery();

            command.Dispose();
            cnn.Close();
        }

        internal static void SaveStudySessions( string stackId, int score ) {

            cnn.Open();

            string sql = $"Insert into StudySessions (date, score, stack_id) values('{DateTime.Now.Date}', '{score}', {stackId})";
            var command = new SqlCommand(sql, cnn);
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();

            command.Dispose();
            cnn.Close();
        }

        internal static List<StudySessionDto> GetStudySessions( Stack stack ) {


            cnn.Open();
            List<StudySessionDto> table = new List<StudySessionDto>();

            string sql = $"SELECT * from StudySessions WHERE stack_id = {stack.Id}";

            var command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read()) {
                var date = reader.GetValue(1).ToString();
                var score = reader.GetValue(2).ToString();

                StudySessionDto session = new StudySessionDto(date.Split(" ")[0], int.Parse(score), stack.Name);

                table.Add(session);
            }
            cnn.Close();

            return table;
        }

        internal static List<List<Object>> ChallengeReport( Stack stack, string year, bool total ) {

            cnn.Open();
            List<List<Object>> table = new List<List<Object>>();

            string sql = GetChallengeSql(stack.Id, year, total);

            var command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read()) {
                var name = reader.GetValue(0).ToString();
                var january = reader.GetValue(1).ToString();
                var february = reader.GetValue(2).ToString();
                var march = reader.GetValue(3).ToString();
                var april = reader.GetValue(4).ToString();
                var may = reader.GetValue(5).ToString();
                var june = reader.GetValue(6).ToString();
                var july = reader.GetValue(7).ToString();
                var august = reader.GetValue(8).ToString();
                var september = reader.GetValue(9).ToString();
                var october = reader.GetValue(10).ToString();
                var november = reader.GetValue(11).ToString();
                var december = reader.GetValue(12).ToString();

                table.Add(
                    new List<object> { name, january, february, march, april, may, june, july,
                        august, september, october, november, december});
            }
            cnn.Close();
            return table;
        }

        internal static string GetChallengeSql( string id, string year, bool total ) {

            string filter =
                total ? "COUNT" : "AVG";

            return @$"SELECT 
            name as StackName,
            [1] as January,
            [2] as February,
            [3] as March,
            [4] as April,
            [5] as May,
            [6] as June,
            [7] as July,
            [8] as August,
            [9] as September,
            [10] as October,
            [11] as November,
            [12] as December
            FROM (SELECT name, score, Day(date) as monthDone 
            FROM StudySessions s
            JOIN Stack t ON s.stack_id = t.stack_id AND YEAR(date) = '{year}' AND t.stack_id = {id}
            ) source
            PIVOT(
            	{filter}(score) FOR monthDone IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
            ) AS pivottable";
        }
    }
}
