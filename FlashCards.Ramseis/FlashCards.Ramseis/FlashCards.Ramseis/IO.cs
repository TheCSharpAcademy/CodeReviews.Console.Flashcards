using System.Data.SqlClient;

namespace FlashCards.Ramseis
{
    internal class IO
    {
        public static int GetInteger()
        {
            int.TryParse((Console.ReadLine() ?? string.Empty).Trim(), out int input);

            return input;
        }
        public static List<CardStack> SqlGetStacks()
        {
            List<CardStack> data = new();
            using (SqlConnection connection = new SqlConnection(Options.connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = $"select * from stacks";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CardStack stack = new CardStack
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        data.Add(stack);
                    }
                }
                else
                {
                    Console.Write($"\n No rows found. Verify database command.\n Connection: {Options.connectionString}\n Command: {command}");
                    Console.ReadKey();
                }
                connection.Close();
            }
            return data;
        }
        public static int SqlGetStackID(string stackName)
        {
            int stackID = 0;
            using (SqlConnection connection = new SqlConnection(Options.connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"select id from stacks where stackname = '{stackName}'";
                stackID = (int)command.ExecuteScalar();
                connection.Close();
            }
            return stackID;
        }
        public static int SqLStackCount()
        {
            int stackCount = -1;
            using (SqlConnection connection = new SqlConnection(Options.connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"select count(id) from stacks";
                stackCount = (int) command.ExecuteScalar();
                connection.Close();
            }
            return stackCount;
        }
        public static List<List<object>> SqlGetHistory()
        {
            List<List<object>> table = new();
            using (SqlConnection connection = new SqlConnection(Options.connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"select * from history";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        List<object> row = new();
                        row.Add((object)reader.GetString(1));
                        row.Add((object)reader.GetInt32(2));
                        row.Add((object)reader.GetInt32(3));
                        row.Add((object)reader.GetString(4));
                        table.Add(row);
                    }
                }
                else
                {
                    Console.Write($"\n No history found. Verify database command.\n Connection: {Options.connectionString}");
                    Console.ReadKey();
                }
                connection.Close();
            }
            return table;
        }

        public static List<Card> SqlGetCards(int stackID)
        {
            List<Card> cards = new();
            using (SqlConnection connection = new SqlConnection(Options.connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"select * from cards where stackid = {stackID}";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Card card = new Card
                        {
                            ID = reader.GetInt32(0),
                            Question = reader.GetString(2),
                            Answer = reader.GetString(3)
                        };
                        cards.Add(card);
                    }
                }
                else
                {
                    Console.Write($"\n No cards found. Verify database command.\n Connection: {Options.connectionString}\n StackID: {stackID}");
                    Console.ReadKey();
                }
                connection.Close();
            }
            return cards;
        }

        public static void SqlAddScore(int score, int count, string name, int stackID)
        {
            if (SqlNonQuery($"insert into history(name, score, count, date, stackid) values('{name}', {score}, {count}, '{DateTime.Now}', {stackID})") == 0)
            {
                Console.WriteLine(" Sql error adding score.");
                Console.ReadKey();
            }
        }
        public static void SqlInitialize()
        {
            string command =
                "if not exists (select * from sys.tables where name = 'stacks') " +
                "begin create table stacks (" +
                "ID integer primary key identity(1,1) not null, " +
                "stackname varchar(255) not null) " +
                "end";
            if (SqlNonQuery(command) == 0)
            {
                Console.WriteLine(" Sql error checking or creating stacks table. Unexpected program behavior may result.");
                Console.ReadKey();
            }
            command =
                "if not exists (select * from sys.tables where name = 'cards') " +
                "begin create table cards (" +
                "ID integer primary key identity(1,1) not null, " +
                "stackid integer foreign key references stacks(id), " +
                "question varchar(255) not null, " +
                "answer varchar(255) not null) " +
                "end";
            if (SqlNonQuery(command) == 0)
            {
                Console.WriteLine(" Sql error checking or creating cards table. Unexpected program behavior may result.");
                Console.ReadKey();
            }
            command =
                "if not exists (select * from sys.tables where name = 'history') " +
                "begin create table history (" +
                "ID integer primary key identity(1,1) not null, " +
                "name varchar(255) not null, " +
                "score integer not null, " +
                "count integer not null, " +
                "date varchar(255) not null, " +
                "stackid integer foreign key references stacks(id))" +
                "end";
            if (SqlNonQuery(command) == 0)
            {
                Console.WriteLine(" Sql error checking or creating history table. Unexpected program behavior may result.");
                Console.ReadKey();
            }
        }
        public static void SqlAddCard(int stackID, string question, string answer)
        {
            if (SqlNonQuery($"insert into cards(stackid, question, answer) values({stackID}, '{question}', '{answer}')") == 0)
            {
                Console.WriteLine(" Sql error adding card.");
                Console.ReadKey();
            }
        }
        public static void SqlDeleteCard(int cardID)
        {
            if (SqlNonQuery($"delete from cards where id = {cardID}") == 0)
            {
                Console.WriteLine(" Sql error deleting card.");
                Console.ReadKey();
            }
        }
        public static void SqlDeleteStack(int stackID)
        {
            if (SqlNonQuery(
                $"delete from cards where stackid = {stackID}; " +
                $"delete from history where stackid = {stackID}; " +
                $"delete from stacks where id = {stackID};") == 0)
            {
                Console.WriteLine(" Sql error deleting stack & cards.");
                Console.ReadKey();
            }
        }
        public static void SqlRenameStack(int stackID, string newName)
        {
            if (SqlNonQuery($"update stacks set stackname = '{newName}' where id = {stackID}") == 0)
            {
                Console.Write($"\n No records changed. Stack rename failed.\n Connection: {Options.connectionString}\n stackID: {stackID}\n newName: {newName}");
                Console.ReadKey();
            }
        }
        public static void SqlAddStack(string stackName)
        {
            if (SqlNonQuery($"insert into stacks values('{stackName}')") == 0)
            {
                Console.WriteLine(" Sql error adding card stack.");
                Console.ReadKey();
            }
        }
        static int SqlNonQuery(string commandIn)
        {
            int output = 0;
            using (SqlConnection connection = new SqlConnection(Options.connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = commandIn;
                output = command.ExecuteNonQuery();
                connection.Close();
            }
            return output;
        }
    }
}
