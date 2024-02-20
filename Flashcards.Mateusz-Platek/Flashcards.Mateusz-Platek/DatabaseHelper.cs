using System.Data.SqlClient;

namespace Flashcards.Mateusz_Platek;

public static class DatabaseHelper
{
    public static void CreateDatabase()
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=master;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = @"
                IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'flashcards')
                    CREATE DATABASE flashcards";
        sqlCommand.ExecuteNonQuery();
    }
    
    public static void CreateTables()
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = @"
            IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='stacks' AND xtype='U')
                CREATE TABLE stacks
                (
                    stack_id int identity
                        constraint stacks_pk
                            primary key,
                    name     varchar(255) not null
                        constraint stacks_pk_2
                            unique
                )
            
            IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='flashcards' AND xtype='U')
                CREATE TABLE flashcards
                (
                    flashcard_id int identity
                        constraint flashcards_pk
                            primary key,
                    stack_id     int           not null
                        constraint flashcards_stacks_stack_id_fk
                            references stacks
                            on delete cascade,
                    word         nvarchar(255) not null,
                    translation  nvarchar(255) not null
                )

            IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='sessions' AND xtype='U')
                CREATE TABLE sessions
                (
                    session_id int identity
                        constraint sessions_pk
                            primary key,
                    stack_id   int      not null
                        constraint sessions_stacks_stack_id_fk
                            references stacks
                            on delete cascade,
                    score      int      not null,
                    datetime   datetime not null,
                    max_score  int      not null
                )";
        sqlCommand.ExecuteNonQuery();
    }

    public static void InsertData()
    {
        using SqlConnection sqlConnection = new SqlConnection(@"Server=(localdb)\Flashcards;Database=flashcards;Trusted_Connection=True;");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = @"
            DELETE FROM flashcards;
            IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = 'flashcards' AND last_value IS NOT NULL)
                DBCC CHECKIDENT (flashcards, RESEED, 0);
                
            DELETE FROM sessions;
            IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = 'sessions' AND last_value IS NOT NULL)
                DBCC CHECKIDENT (sessions, RESEED, 0);
                        
            DELETE FROM stacks;
            IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = 'stacks' AND last_value IS NOT NULL)
                DBCC CHECKIDENT (stacks, RESEED, 0);
                
            INSERT INTO stacks (name) 
            VALUES ('polish'),
                   ('german'),
                   ('french');
            
            INSERT INTO flashcards (stack_id, word, translation)
            VALUES (1, 'car', N'samochód'),
                   (1, 'sausage', N'kiełbasa'),
                   (1, 'keyboard', N'klawiatura'),
                   (2, 'coffee', 'der Kaffee'),
                   (2, 'sword', 'das Schwert'),
                   (2, 'rifle', 'das Gewehr'),
                   (3, 'tie', 'la cravate'),
                   (3, 'monitor', 'le moniteur'),
                   (3, 'chicken', 'le poulette');
                   
            INSERT INTO sessions (stack_id, score, datetime, max_score)
            VALUES (1, 2, '2023-06-18 14:35:15', 3),
                   (1, 3, '2023-08-22 18:32:18', 3),
                   (1, 2, '2023-01-03 09:55:08', 3),
                   (1, 1, '2023-06-25 16:00:44', 3),
                   (2, 1, '2023-08-09 11:12:34', 3),
                   (2, 2, '2023-03-05 07:49:08', 3),
                   (3, 1, '2023-12-28 14:13:39', 3),
                   (3, 3, '2023-11-23 23:13:33', 3);";
        sqlCommand.ExecuteNonQuery();
    }
}