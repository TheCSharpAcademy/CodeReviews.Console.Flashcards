
using Microsoft.Data.SqlClient;
using System.Data;


namespace Flashcards
{
    internal class Data
    {
        private static readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

   
        internal static void CheckCreateDatabase()
        {
            string databaseName = "FlashCards";
            string projectPath = Directory.GetCurrentDirectory();
            bool databaseExists = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    databaseExists = CheckIfDatabaseExists(connection, databaseName);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking database: " + ex.Message);
            }


            if (databaseExists)
            {
                Console.WriteLine("Database Exits.");
            }
            else if (!databaseExists)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = @$"BEGIN
                                                    CREATE DATABASE {databaseName} ON PRIMARY
                                                    (NAME = N'{databaseName}',
                                                    FILENAME = N'{projectPath}\\{databaseName}Data.mdf',
                                                    SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)
                                                    LOG ON (NAME = N'{databaseName}_Log',
                                                    FILENAME = N'{projectPath}\\{databaseName}Log.ldf',
                                                    SIZE = 1MB,
                                                    MAXSIZE = 5MB,
                                                    FILEGROWTH = 10%)
                                                    END";
                        command.ExecuteNonQuery();

                        connection.Close();


                        Console.WriteLine("Database created successfully.");


                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating database: " + ex.Message);
                }
            }
        }
        public static void CreateTables()
        {
            Settings setting = new Settings();
            CreateStacksTable();
            CreateCardsTable();
            CreateSudySessionsTable();
            CreateVersionTable();
            setting =  GetSettings();
            if (setting.Version < 2)
            {
                Stack.LoadSeedDataStacks();
                Card.LoadSeedDataCards();
                StudySession.LoadSeedDataStudySessions();
                UpdateVersion(2);
            }
        }

        private static void CreateSudySessionsTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"USE FlashCards;
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySessions')
                    BEGIN
                    CREATE TABLE dbo.StudySessions (
                    StudySessionID INT IDENTITY(1,1) PRIMARY KEY, 
                    StackID INT,
                    Date DATE,
                    StackName VARCHAR(50),
                    Correct INT,
                    Total INT,
                    Score DECIMAL (5,2)
                    CONSTRAINT fk_study_stack_id
                        FOREIGN KEY (StackID)
                        REFERENCES Stacks (StackID)
                        ON DELETE CASCADE)

                    END";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Study Sessions Table");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally { connection.Close(); }
            }

        }

        private static void CreateCardsTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"USE FlashCards;
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Cards')
                    BEGIN
                    CREATE TABLE dbo.Cards (
                    CardID INT IDENTITY(1,1) PRIMARY KEY,
                    StackID INT,
                    NoInStack INT,
                    Front VARCHAR(50),
                    Back VARCHAR(50)
                    CONSTRAINT fk_card_stack_id
                        FOREIGN KEY (StackID)
                        REFERENCES Stacks (StackID)
                        ON DELETE CASCADE)
                    END";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Cards Table");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally { connection.Close(); }
            }
        }


        private static void CreateStacksTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"USE FlashCards;
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks')
                    BEGIN
                    CREATE TABLE dbo.Stacks (
                    StackID INT IDENTITY(1,1) PRIMARY KEY,
                    StackName VARCHAR(50)); 
                    END";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Stacks Table");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally { connection.Close(); }
            }

        }
        private static void CreateVersionTable()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"Use FlashCards;
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Version')
                    BEGIN
                    CREATE TABLE dbo.Version(
                    VersionID  INT IDENTITY(1,1) PRIMARY KEY,
                    Version INT);
                    END";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Version Table Create");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally { connection.Close(); }
            }
        }
        private static Settings GetSettings()
        {
            Settings settings = new Settings();
            while (settings.Version < 1)
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    SqlDataReader reader = null;
                    try
                    {
                        tableCmd.CommandText =
                            @"Use FlashCards;
                    SELECT * FROM Version WHERE VersionID = 1";

                        reader = tableCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                {
                                    settings.VersionID = reader.GetInt32(0);
                                    settings.Version = reader.GetInt32(1);
                                };
                            }
                        }
                        else
                        {
                            EnterVersion(1);
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Error at GetSettings.");
                        Console.WriteLine(exception.Message);
                        Console.ReadLine();
                    }
                    finally
                    {
                        reader?.Close();
                        connection.Close();
                    }
                }

            }
            return settings;
        }
        private static void EnterVersion(int version)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "INSERT INTO FlashCards.dbo.Version (Version) VALUES (@Version); SELECT SCOPE_IDENTITY();";
                tableCmd.Parameters.Add(new SqlParameter("@Version", SqlDbType.VarChar) { Value = version });

                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                    Console.WriteLine($@"Error at Enter Version: {version}");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        private static void UpdateVersion(int version)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "UPDATE FlashCards.dbo.Version SET Version = (@Version) WHERE VersionID = 1; SELECT SCOPE_IDENTITY();";
                tableCmd.Parameters.Add(new SqlParameter("@Version", SqlDbType.VarChar) { Value = version });

                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                    Console.WriteLine($@"Error at Update Version: {version}");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        private static bool CheckIfDatabaseExists(SqlConnection connection, string databaseName)
        {
            bool databaseExists = false;
            string query = "SELECT COUNT(*) FROM master.dbo.sysdatabases WHERE name = @DatabaseName";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DatabaseName", databaseName);
                    connection.Open();
                    int databaseCount = Convert.ToInt32(command.ExecuteScalar());
                    databaseExists = (databaseCount > 0);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine("Error at Check if Database Exists");
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return databaseExists;
        }

        internal static bool DoesStackExist(string Name)
        {
            bool doesStackExist = true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"SELECT COUNT(*)
                    FROM FlashCards.dbo.Stacks
                    WHERE Upper(StackName) = '{Name}'";
                try
                {
                    int count = (int)tableCmd.ExecuteScalar();
                    doesStackExist = (count > 0);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error in DoesStackExist");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                connection.Close();
            }

            return doesStackExist;
        }
        internal static int EnterStack(string stackName)
        {
            int stackID = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "INSERT INTO FlashCards.dbo.Stacks (StackName) VALUES (@StackName); SELECT SCOPE_IDENTITY();";
                tableCmd.Parameters.Add(new SqlParameter("@StackName", SqlDbType.VarChar) { Value = stackName });

                try
                {
                    stackID = Convert.ToInt32(tableCmd.ExecuteScalar());

                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error at LoadStack");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                connection.Close();
            }
            return stackID;
        }

        internal static int EnterCard(int stackID, int noInStack, string front, string back)
        {
            int cardID = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "INSERT INTO FlashCards.dbo.Cards (StackID,NoInStack,Front,Back) VALUES (@StackID,@NoInStack,@Front,@Back); SELECT SCOPE_IDENTITY();";
                tableCmd.Parameters.Add(new SqlParameter("@StackID", SqlDbType.Int) { Value = stackID});
                tableCmd.Parameters.Add(new SqlParameter("@NoInStack", SqlDbType.Int) { Value = noInStack});
                tableCmd.Parameters.Add(new SqlParameter("@Front", SqlDbType.VarChar) { Value = front});
                tableCmd.Parameters.Add(new SqlParameter("@Back", SqlDbType.VarChar) { Value = back});

                try
                {
                    cardID = Convert.ToInt32(tableCmd.ExecuteScalar());

                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error at LoadStack");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally
                {
                    connection.Close();
                }
            }
            return cardID;
        }

        internal static int EnterStudySession(StudySession sessionToLoad)
        {
            int studySessionID = -1;
            using(SqlConnection connection = new SqlConnection(connectionString)) 
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "INSERT INTO FlashCards.dbo.StudySessions (StackID,Date,StackName,Correct,Total,Score) VALUES (@StackID,@Date,@StackName,@Correct,@Total,@Score); SELECT SCOPE_IDENTITY();";
                tableCmd.Parameters.Add(new SqlParameter("@StackID", SqlDbType.Int) { Value = sessionToLoad.StackID });
                tableCmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date) { Value = sessionToLoad.Date });
                tableCmd.Parameters.Add(new SqlParameter("@StackName", SqlDbType.VarChar) { Value = sessionToLoad.StackName });
                tableCmd.Parameters.Add(new SqlParameter("@Correct", SqlDbType.Int) { Value = sessionToLoad.Correct });
                tableCmd.Parameters.Add(new SqlParameter("@Total", SqlDbType.Int) { Value = sessionToLoad.Total });
                tableCmd.Parameters.Add(new SqlParameter("@Score",SqlDbType.Decimal) { Precision = 5, Scale = 2, Value = sessionToLoad.Score });

                try
                {
                    studySessionID = Convert.ToInt32(tableCmd.ExecuteScalar());
                }
                catch (Exception exception) 
                {
                    Console.WriteLine("Error at Session to Load");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally
                {
                    connection.Close();
                }
            }
            return studySessionID;
        }

        internal static void UpdateCard(Card cardToUpdate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"Update FlashCards.dbo.Cards
                    SET 
                    NoInStack ='{cardToUpdate.NoInStack}',
                    Front ='{cardToUpdate.Front}',
                    Back ='{cardToUpdate.Back}'
                    WHERE CardID = '{cardToUpdate.CardID}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error in UpdateCard");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        internal static void DeleteCard(Card cardToDelete)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"DELETE
                    FROM FlashCards.dbo.Cards
                    WHERE CardID = '{cardToDelete.CardID}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error in DeleteCard");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        internal static List<Stack> LoadStacks()
        {
            List<Stack> stacks = new List<Stack>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                SqlDataReader reader = null; 
                try
                {
                    tableCmd.CommandText = "SELECT * FROM FlashCards.dbo.Stacks";
                    reader = tableCmd.ExecuteReader(); 
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            stacks.Add(
                                new Stack
                                {
                                    StackID = reader.GetInt32(0),
                                    StackName = reader.GetString(1)
                                });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                        Console.ReadKey();
                    }
                }
                catch (Exception exception) 
                {
                    Console.WriteLine("Error at LoadStacks.");
                    Console.WriteLine(exception.Message); 
                    Console.ReadLine();
                }
                finally
                {
                    reader?.Close(); 
                    connection.Close();
                }
            }
            return stacks;
        }

        internal static List<Card> LoadCards(int stackID)
        {
            List<Card> cards = new List<Card>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                SqlDataReader reader = null;
                try
                {
                    if (stackID == 0)
                    {
                        tableCmd.CommandText = "SELECT * FROM FlashCards.dbo.Cards ORDER BY CardID";
                    }
                    else
                    {
                        tableCmd.CommandText = "SELECT * FROM FlashCards.dbo.Cards WHERE StackID = @StackID ORDER BY CardID";
                        tableCmd.Parameters.Add(new SqlParameter("@StackID", SqlDbType.Int) { Value = stackID });
                    }
                    reader = tableCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cards.Add(
                                new Card
                                {
                                    CardID = reader.GetInt32(0),
                                    StackID = reader.GetInt32(1),
                                    NoInStack = reader.GetInt32(2),
                                    Front = reader.GetString(3),
                                    Back = reader.GetString(4),
                                });
                        }
                    }
                    else
                    {
                        //Console.WriteLine("No rows found");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error at LoadCards.");
                    Console.WriteLine(exception.Message);
                    Console.ReadLine();
                }
                finally
                {
                    reader?.Close();
                    connection.Close();
                }
            }
            return cards;
        }

        internal static List<StudySession> LoadStudySessions(Stack stack)
        {
            List<StudySession> studySessions = new List<StudySession>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                SqlDataReader reader = null;

                tableCmd.CommandText =
                    $@"SELECT * FROM [FlashCards].[dbo].[StudySessions]
                        WHERE StackID = '{stack.StackID}'";

                reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {

                            studySessions.Add(
                                new StudySession
                                {
                                    StudySessionID = reader.GetInt32(0),
                                    StackID = reader.GetInt32(1),
                                    Date = reader.GetFieldValue<DateOnly>(2),
                                    StackName = reader.GetString(3),
                                    Correct = reader.GetInt32(4),
                                    Total = reader.GetInt32(5),
                                    Score = (double)reader.GetDecimal(6)
                                });
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine("Error at LoadStudySession.");
                            Console.WriteLine(exception.Message);
                            Console.ReadLine();
                        }

                    }
                    reader?.Close();
                    connection.Close();
                }

                else
                {
                    Console.WriteLine("No Rows Found");
                    Console.ReadKey();
                }              
            }
            return studySessions;
        }

        internal static void DelStack(Stack stackToDel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"DELETE
                    FROM FlashCards.dbo.Stacks
                    WHERE StackID = '{stackToDel.StackID}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error in DelStack");
                    Console.WriteLine(exception);
                    Console.ReadLine();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        internal static List<StudySessionReport> GetReports()
        {
            List<StudySessionReport> reports = new List<StudySessionReport>();
            StudySessionReport report = new StudySessionReport();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                SqlDataReader reader = null;

                tableCmd.CommandText =
                    $@"WITH CTE AS (
                    SELECT [StudySessionID],[StackID], [StackName], YEAR([Date]) AS [Year], MONTH([Date]) AS [Month]
                    FROM [FlashCards].[dbo].[StudySessions]
                    )
                    SELECT *
                    FROM CTE
                    PIVOT (
                    Count([StudySessionID])
                    FOR [Month] IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                    ) AS PivotTable";
                
                reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        reports.Add(
                        new StudySessionReport
                        {
                            StackID = reader.GetInt32(0),
                            StackName = reader.GetString(1),
                            YEAR = reader.GetInt32(2),
                            January = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            February = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            March = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                            April = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                            May = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                            June = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                            July = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                            August = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                            September = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                            October = reader.IsDBNull(12) ? 0 : reader.GetInt32(12),
                            November = reader.IsDBNull(13) ? 0 : reader.GetInt32(13),
                            December = reader.IsDBNull(14) ? 0 : reader.GetInt32(14)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                    Console.ReadKey();
                }
                connection.Close();
            }
            return reports;
        }
     }
}

