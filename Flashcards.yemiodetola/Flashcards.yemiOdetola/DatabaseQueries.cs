using Microsoft.Data.SqlClient;

namespace Flashcards.yemiOdetola;

public class DatabaseQueries
{
  private readonly string ConnectionString;
  public DatabaseQueries(string connectionString)
  {
    ConnectionString = connectionString;
  }
  public void ConnectDatabase()
  {
    try
    {
      using (SqlConnection connection = new SqlConnection(ConnectionString))
      {
        connection.Open();
        Console.WriteLine("Connection successful!");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Connect db Error: {ex.Message}");
    }
  }

  public void CreateTables()
  {
    try
    {
      using (SqlConnection connection = new SqlConnection(ConnectionString))
      {
        connection.Open();
        SqlCommand sqlCommand = connection.CreateCommand();
        sqlCommand.CommandText = @"
            IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='stacks' AND xtype='U')
                CREATE TABLE stacks
                (
                  id int identity
                    constraint stacks_pk
                      primary key,
                  name varchar(255) not null
                    constraint stacks_pk_2
                      unique,
                )
            
            IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='flashcards' AND xtype='U')
                CREATE TABLE flashcards
                (
                  id int identity
                    constraint flashcards_pk
                      primary key,
                  stack_id int not null
                    constraint flashcards_stacks_stack_id_fk
                      references stacks
                      on delete cascade,
                  word varchar(255) not null,
                  category varchar(255) not null
                )

            IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='sessions' AND xtype='U')
                CREATE TABLE sessions
                (
                    id int identity
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
    }
    catch (Exception ex)
    {
      Console.WriteLine($"CreateTables Error: {ex.Message}");
    }
  }
}