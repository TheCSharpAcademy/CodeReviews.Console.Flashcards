using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;

namespace FlashCardsLibrary
{
    public static class Database
    {
         internal static string _connectionString = string.Empty;
         public static void InitialiseDB()
         {
            // SQL command to insert a new stack
            SetConnetionString();
            if(_connectionString == string.Empty)
            {
                throw new Exception("Error: Connection string is empty check. Couldn't find \"config.xml\"");
            }
            CheckDatabase();
         }
        private static void SetConnetionString()
        {
            var file = XDocument.Load("../../../config.xml");
            _connectionString = file.Element("Database")?.Element("ConnectionString")?.Value ?? string.Empty ;
        }
        private static void CheckDatabase()
        {
            try
            {

                SqlConnection conn = new SqlConnection(_connectionString);
                using (conn)
                {
                    conn.Open();
                    var tableCmd = conn.CreateCommand();
                    tableCmd.CommandText =
                        $@"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FlashCardsDB')
                           BEGIN
                             CREATE DATABASE FlashCardsDB;
                           END;
                         ";
                    tableCmd.ExecuteNonQuery();
                    conn.Close();
                }

                CreateTables();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void CreateTables()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            using (conn)
            {
                conn.Open();
                var tableCmd = conn.CreateCommand();
                //Stack table
                tableCmd.CommandText =
                    $@" IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                      CREATE TABLE [dbo].[Stacks] (
                        [StackName] NVARCHAR (100) NOT NULL,
                        CONSTRAINT [PK_Stacks_1] PRIMARY KEY CLUSTERED ([StackName] ASC)
                    );
                      ";
                tableCmd.ExecuteNonQuery();
                //FlashCards table
                tableCmd.CommandText =
                    $@" IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FlashCards')
                        CREATE TABLE [dbo].[FlashCards] (
                            [ID]        INT            IDENTITY (1, 1) NOT NULL,
                            [StackName] NVARCHAR (100) NOT NULL,
                            [Front]     NVARCHAR (255) NOT NULL,
                            [Back]      NVARCHAR (255) NULL,
                            CONSTRAINT [PK_FlashCards] PRIMARY KEY CLUSTERED ([ID] ASC),
                            CONSTRAINT [StackFlashCardsRelation] FOREIGN KEY ([StackName]) REFERENCES [dbo].[Stacks] ([StackName]) ON DELETE CASCADE ON UPDATE CASCADE
                        );
                         ";
                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText = $@"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sessions')
                CREATE TABLE [dbo].[Sessions] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50)  NULL,
    [StackName] NVARCHAR (100) NOT NULL,
    [Date]      DATE           NOT NULL,
    [Answers]   INT            NOT NULL,
    [Total]     INT            NOT NULL,
    CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Sessions_Stacks] FOREIGN KEY ([StackName]) REFERENCES [dbo].[Stacks] ([StackName]) ON DELETE CASCADE ON UPDATE CASCADE
);
"; tableCmd.ExecuteNonQuery();

                conn.Close();

                Console.WriteLine("Good to Go");
            }
        }
    }

}
