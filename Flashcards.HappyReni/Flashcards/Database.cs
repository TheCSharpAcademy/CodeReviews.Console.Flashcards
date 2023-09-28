using System.Data;
using System.Data.SqlClient;

namespace Flashcards
{
    internal class Database
    {
        public bool isConnected;
        public string currentDirectory;
        public string dbName;
        public string connInfo;

        public Database()
        {
            currentDirectory = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, @"..\..\..\"));
            dbName = "MyDB.mdf";
            connInfo = $@"Data Source = (localdb)\MSSQLLocalDB; AttachDbFilename=""{currentDirectory}{dbName}""; Integrated Security = True; Connect Timeout = 10;";
            isConnected = Init();
        }
        public bool Init()
        {
            var res = Connect();

            CreateTable("Stack");
            CreateTable("Flashcards");
            CreateTable("Session");
            return res;
        }
        public bool Connect()
        {
            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public Dictionary<string, Stack>? GetStacksFromDatabase()
        {
            Dictionary<string, Stack> stacks = new Dictionary<string, Stack>();
            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();
                    string selectQuery = "SELECT * FROM Stack";

                    using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("Id");
                                string name = reader.GetString("Name");
                                Stack stack = new(id, name);
                                stacks[name] = stack;
                            }
                        }
                    }
                }

                return stacks;
            }
            catch
            {
                return null;
            }
        }

        public bool CreateTable(string name)
        {
            string createTableQuery = name == "Stack" ?
                        $"CREATE TABLE {name} (" +
                        $"ID INT PRIMARY KEY IDENTITY (1,1)," +
                        $"Name NVARCHAR(20))" :
                        name == "Flashcards" ?
                        $"CREATE TABLE {name} (" +
                        $"ID INT PRIMARY KEY," +
                        $"StackId INT FOREIGN KEY REFERENCES Stack(ID) ON DELETE CASCADE," +
                        $"Front NVARCHAR(20)," +
                        $"Back NVARCHAR(20))" :
                        $"CREATE TABLE {name} (" +
                        $"ID INT PRIMARY KEY IDENTITY (1,1)," +
                        $"StackId INT FOREIGN KEY REFERENCES Stack(ID) ON DELETE CASCADE," +
                        $"StartTime DATETIME," +
                        $"EndTime DATETIME," +
                        $"Score INT," +
                        $"QuestionCount INT)";

            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(createTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int Insert(string name)
        {
            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();

                    string insertQuery = $"INSERT INTO Stack (Name) VALUES ('{name}')";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return GetIndexFromStack(name, conn);
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        private static int GetIndexFromStack(string name, SqlConnection conn)
        {
            var id = 0;
            string selectQuery = $"SELECT * FROM Stack WHERE Name = '{name}'";
            using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }
            }
            return id;
        }

        public bool Insert(Flashcard card)
        {
            var Id = card.Id;
            var stackId = card.StackId;
            var Front = card.Front;
            var Back = card.Back;
            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();

                    string insertQuery = $"INSERT INTO dbo.Flashcards (Id,StackId,Front,Back) VALUES ({Id}, {stackId}, '{Front}','{Back}')";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                throw new Exception("Insert failed.");
            }
        }
        public bool Insert(Session session)
        {
            var stackId = session.StackId;
            var startTime = session.StartTime;
            var endTime = session.EndTime;
            var score = session.Score;
            var questionCount = session.QuestionCount;

            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();

                    string insertQuery = $"INSERT INTO Session (StackId,StartTime,EndTime,Score,QuestionCount) VALUES ({stackId}, '{startTime}','{endTime}', {score}, {questionCount})";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string SearchStackName(int id, string table)
        {
            try
            {
                var name = "";
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();
                    var searchQuery = $"SELECT * FROM {table} WHERE id = {id}";
                    using (SqlCommand cmd = new SqlCommand(searchQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                name = reader.GetString("Name");
                            }
                            return name;
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("no such a stack.");
            }
        }

        public bool Update(Flashcard card)
        {
            try 
            {
                var id = card.Id;
                var back = card.Back;
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();

                    string updateQuery = $"UPDATE Flashcards SET Back='{back}' WHERE ID = {id}";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                throw new Exception("Update failed.");
            }
        }
        public bool Delete(string name)
        {
            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();

                    string deleteQuery = $"DELETE FROM Stack WHERE Name = '{name}'";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                throw new Exception("Delete failed.");
            }
        }
        public bool Delete(int idx)
        {
            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();

                    string deleteQuery = $"DELETE From Flashcards WHERE ID = {idx}";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    UpdateID();
                }
                return true;
            }
            catch
            {
                throw new Exception("Delete failed.");
            }
        }

        public bool UpdateID()
        {
            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();

                    string selectQuery = $"SELECT * FROM Flashcards";
                    using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                    {
                        List<(int, int)> tempIdx = new List<(int, int)>();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            var new_idx = 1;
                            while (reader.Read())
                            {
                                var original_idx = reader.GetInt32(0);
                                var idx = (original_idx, new_idx);
                                tempIdx.Add(idx);
                                new_idx++;
                            }
                        }
                        foreach (var idx in tempIdx)
                        {
                            string updateQuery = $"UPDATE Flashcards SET ID={idx.Item2} WHERE ID = {idx.Item1}";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                throw new Exception("Update failed.");
            }
        }

        public List<Flashcard> SetFlashcardsInStack(int stackId, string arg)
        {
            List<Flashcard> cards = new List<Flashcard>();

            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();

                    string selectQuery = $"SELECT * FROM Flashcards WHERE StackId = {stackId}";
                    using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string front = reader.GetString("Front");
                                string back = reader.GetString("Back");
                                var card = new Flashcard(id, stackId, front, back);
                                if (arg == "View") Flashcard.DownCount();
                                cards.Add(card);
                            }
                        }
                    }
                }
                return cards;
            }
            catch
            {
                throw new Exception("The stack is empty.");
            }
        }
        public List<Session>? GetSessions()
        {
            List<Session> Sessions = new List<Session>();
            var format = "yyyy-MM-dd HH:mm:ss";

            try
            {
                using (SqlConnection conn = new(connInfo))
                {
                    conn.Open();

                    string selectQuery = $"SELECT * FROM Session";
                    using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int stackId = reader.GetInt32(1);
                                DateTime startTime = reader.GetDateTime(2);
                                DateTime endTime = reader.GetDateTime(3);
                                int score = reader.GetInt32(4);
                                int questionCount = reader.GetInt32(5);
                                var session = new Session(stackId, startTime.ToString(format), endTime.ToString(format), score, questionCount);
                                Sessions.Add(session);
                            }
                        }
                    }
                }
                return Sessions;
            }
            catch
            {
                return Sessions;
            }
        }

    }

}
