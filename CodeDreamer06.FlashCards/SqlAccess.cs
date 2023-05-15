using System;
using System.Configuration;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FlashStudy.Models;

namespace FlashStudy
{
  class SqlAccess
  {
    public static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
    public static string defaultConnStr = ConfigurationManager.ConnectionStrings["defaultConnStr"].ConnectionString;

    public static void CheckIfDbExists()
    {
      using(MySqlConnection con = new MySqlConnection(defaultConnStr))
      {
        try
        {
          con.Open();

          string query = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'Study'";
          using var cmd = new MySqlCommand(query, con);
          MySqlDataReader reader = cmd.ExecuteReader();

          if(reader.HasRows) return;
        }

        catch (MySqlException ex)
        {
          Console.WriteLine("An error occurred.");
          if(ex.Number == 1042)
            Console.WriteLine("Unable to connect to MySQL server. Make sure MySQL is running.");
          else Console.WriteLine(ex);
          System.Environment.Exit(1);
        }
      }
      using(MySqlConnection con = new MySqlConnection(defaultConnStr))
      {
        Console.WriteLine("Welcome to Flash Study! \nCreating the database..");
        con.Open();

        var initialQueries = new string[] {
          @"CREATE DATABASE Study;",
          @"USE Study;",
          @"CREATE TABLE Stacks(
            StackId INT PRIMARY KEY AUTO_INCREMENT,
            StackName VARCHAR(64) NOT NULL
          );",
          @"CREATE TABLE FlashCards(
            CardId INT PRIMARY KEY AUTO_INCREMENT,
            Title VARCHAR(255) NOT NULL,
            Answer VARCHAR(64),
            StackId INT NOT NULL,
            FOREIGN KEY (StackId)
              REFERENCES Stacks(StackId)
              ON DELETE CASCADE
          );",
          @"CREATE TABLE Sessions(
            SessionId INT PRIMARY KEY AUTO_INCREMENT,
            CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
            Score INT NOT NULL,
            StackId INT NOT NULL,
            FOREIGN KEY (StackId)
              REFERENCES Stacks(StackId)
              ON DELETE CASCADE
          );"
        };

        foreach (string line in initialQueries)
        {
          using var lineQuery = new MySqlCommand(line, con);
          lineQuery.ExecuteNonQuery();
        }
      }
    }

    protected static void Execute(string query)
    {
      try
      {
        using(MySqlConnection con = new MySqlConnection(connStr))
        {
          con.Open();
          using var cmd = new MySqlCommand(query, con);
          cmd.ExecuteNonQuery();
        }
      }

      catch (MySqlException ex)
      {
        if(ex.Number == 1452)
          Console.WriteLine("Invalid Foreign key entered.");

        else
          Console.WriteLine($"Exception code: {ex.Number} \nMessage: {ex.Message}");
      }
    }

    // Read Methods
    protected static List<List<string>> Read(string query, string[] columns)
    {
      try
      {
        using(MySqlConnection con = new MySqlConnection(connStr))
        {
          con.Open();

          using var cmd = new MySqlCommand(query, con);
          MySqlDataReader reader = cmd.ExecuteReader();

          if(!reader.HasRows)
            Console.WriteLine("No data found");

          var output = new List<List<string>>();
          var row = new List<string>();

          while (reader.Read()) {
            foreach (string column in columns)
              row.Add(reader[column].ToString());

            output.Add(row.GetRange(0, row.Count));
            row.Clear();
          }

          return output;
        }
      }

      catch (Exception ex)
      {
        Console.WriteLine(ex);
        return null;
      }
    }

    public static List<FlashCard> ReadFlashCards()
    {
      var rawCards = Read($"SELECT * FROM FlashCards", new string[] { "Title", "Answer", "StackId", "CardId" });
      var cards = new List<FlashCard>();

      foreach (var card in rawCards)
        cards.Add(new FlashCard(card.ToArray()));

       return cards;
    }

    public static List<FlashCard> ReadFlashCards(int stackId)
    {
      var rawCards = Read($"SELECT * FROM FlashCards WHERE StackId = {stackId}", new string[] { "Title", "Answer", "StackId", "CardId" });
      var cards = new List<FlashCard>();

      foreach (var card in rawCards)
        cards.Add(new FlashCard(card.ToArray()));

       return cards;
    }

    public static List<Stack> ReadStacks()
    {
      var rawStacks = Read($"SELECT * FROM Stacks", new string[] { "StackId", "StackName" });
      var stacks = new List<Stack>();

      foreach (var stack in rawStacks)
        stacks.Add(new Stack(stack));

       return stacks;
    }

    public static List<Session> ReadSessions()
    {
      var rawSessions = Read($"SELECT * FROM Sessions", new string[] { "SessionId", "CreatedOn", "Score", "StackId" });
      var sessions = new List<Session>();

      foreach (var session in rawSessions)
        sessions.Add(new Session(session.ToArray()));

       return sessions;
    }

    // Write Methods
    public static void AddStack(Stack properties)
    {
      Execute($"INSERT INTO Stacks(StackName) VALUES('{properties.StackName}');");
      Console.WriteLine("Successfully added " +  properties.StackName);
    }

    public static void AddCard(FlashCard properties)
    {
      Execute($"INSERT INTO FlashCards(Title, Answer, StackId) VALUES('{properties.Title}', '{properties.Answer}', '{properties.StackId}');");
      Console.WriteLine($"Successfully inserted {properties.Title}");
    }

    public static void AddSession(Session properties)
    {
      Execute($"INSERT INTO Sessions(score, StackId) VALUES('{properties.Score}', '{properties.StackId}');");
    }

    public static void EditStack(ReplaceStack properties){
      Execute($"UPDATE Stacks SET StackName = '{properties.NewStackName}' WHERE StackName = '{properties.OldStackName}';");
      Console.WriteLine($"Successfully changed {properties.OldStackName} to {properties.NewStackName}");
    }


    // Delete Methods
    public static void RemoveStack(Stack properties)
    {
      Execute($"DELETE FROM Stacks WHERE StackName='{properties.StackName}';");
      Console.WriteLine("Successfully removed " + properties.StackName);
    }

    public static void RemoveCard(int cardId)
    {
      var flashCard = SqlAccess.ReadFlashCards()[cardId - 1];
      Execute($"DELETE FROM FlashCards WHERE cardId='{flashCard.CardId}';");
      Console.WriteLine($"Successfully removed {flashCard.Title}");
    }
  }
}
