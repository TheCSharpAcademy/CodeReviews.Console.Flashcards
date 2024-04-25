using System.Configuration;
using System.Data.SqlClient;

namespace Flashcards
{
    class Database
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        internal void CreateTable()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var tableCmd = conn.CreateCommand();

                tableCmd.CommandText = @"if not exists(select * from sysobjects where name = 'stacks' and xtype = 'U')
                                        create table Stacks(
                                        StackId int IDENTITY(1,1) PRIMARY KEY not null,
                                        Name varchar(64) not null
                                                            )";

                tableCmd.ExecuteNonQuery();

                conn.Close();
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var tableCmd = conn.CreateCommand();

                tableCmd.CommandText = @"if not exists(select * from sysobjects where name = 'flashcards' and xtype = 'U')
                                            create table Flashcards(
                                            FlashcardId int IDENTITY(1,1) primary key not null,
                                            StackId int,
                                            foreign key (StackId) references Stacks(StackId) on delete CASCADE,
                                            Front text,
                                            Back text
                                            )";

                tableCmd.ExecuteNonQuery();

                conn.Close();
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var tableCmd = conn.CreateCommand();

                tableCmd.CommandText = @"if not exists(select * from sysobjects where name = 'study' and xtype = 'U')
                                            create table Study(
                                            Id int IDENTITY(1, 1) primary key not null,
                                            StackId int,
                                            foreign key(StackId) references Stacks(StackId) on delete CASCADE,
                                            Date varchar(32),
                                            Score int
                                            )";

                tableCmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}