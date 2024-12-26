using FlashCards.FlashCardsManager.Models;
using Spectre.Console;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;

using FlashCards.StudySessions;

namespace FlashCards.Database
{
    internal class DataTools
    {
        internal string ConnectionString { get; set;}

        internal void Initialization()
        {
            try
            {
                string sqlCommandText;
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                try
                {
                    if (config.AppSettings.Settings["connectionString"].Value == "")
                    {
                        string connectionString = @$"Data Source=localhost;Integrated Security=SSPI;Initial Catalog=;TrustServerCertificate=True;";
                        config.AppSettings.Settings["connectionString"].Value = connectionString;
                        config.Save();
                        using (SqlConnection connection = new(connectionString))
                        {
                            if (connection.State == ConnectionState.Closed) connection.Open();
                            sqlCommandText = $@"CREATE DATABASE FlashCardsDB";
                        sqlCommandText = "nejwrnw";
                            SqlCommand command = connection.CreateCommand();
                            command.CommandText = sqlCommandText;
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
                catch(Exception ex) { throw ex; }

                try
                {
                    ConnectionString = $"Data Source=localhost;Initial Catalog=FlashCardsDB;Integrated Security=True;TrustServerCertificate=True;";
                    using (SqlConnection connection = new(ConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = connection.CreateCommand();
                        sqlCommandText = @"IF OBJECT_ID(N'dbo.Stacks', N'U') IS NULL CREATE TABLE Stacks (Name nvarchar(20) PRIMARY KEY,NumberOfCards INTEGER,Id INTEGER);

                                           IF OBJECT_ID(N'dbo.FlashCards', N'U') IS NULL CREATE TABLE FlashCards (Stack nvarchar(20),Id INTEGER, Question TEXT,Answer TEXT);
                                           
                                           IF NOT (EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_FlashCards_Stacks')) ALTER TABLE [dbo].[FlashCards]
                                           WITH CHECK ADD CONSTRAINT [FK_FlashCards_Stacks] FOREIGN KEY ([Stack])
                                           REFERENCES [dbo].[Stacks] ([Name]) ON UPDATE CASCADE ON DELETE CASCADE;
                                           
                                           IF OBJECT_ID(N'dbo.StudySessions', N'U') IS NULL CREATE TABLE StudySessions (Stack nvarchar(20), Date DATE,QuestionMode TEXT,QuestionCount INTEGER,Score INTEGER,Time TIME);
                                           
                                           IF NOT (EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_StudySessions_Stacks')) ALTER TABLE [dbo].[StudySessions]
                                           WITH CHECK ADD CONSTRAINT [FK_StudySessions_Stacks] FOREIGN KEY ([Stack])
                                           REFERENCES [dbo].[Stacks] ([Name]) ON UPDATE CASCADE ON DELETE CASCADE;";
                        command.CommandText = sqlCommandText;
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                catch(Exception ex) { throw ex; }
            }
            catch (Exception ex)
            {
                Console.ReadKey();
            }
        }

        internal void ExecuteQuery(string sqlCommand,string stack = "", string question = "",string answer = "",int numberOfCards = 0,int id = 0,string date = "",string time = "",string questionMode = "",int? questionCount = 0, int score = 0)
        {
            try
            {
                using (SqlConnection connection = new(ConnectionString))
                {
                    DynamicParameters param = new();
                    param.Add("@stack", stack);
                    param.Add("@question", question);
                    param.Add("@answer", answer);
                    param.Add("@numberOfCards", numberOfCards);
                    param.Add("@id", id);
                    param.Add("@date", date);
                    param.Add("@time", time);
                    param.Add("@questionMode", questionMode);
                    param.Add("@questionCount", questionCount);
                    param.Add("@score", score);
                    connection.Query(sqlCommand,param);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine(ex.Message);
                Console.Read();
            }
        }

        internal void DeleteCard(FlashCard card,string stack)
        {
            string sqlCommand = @$"DELETE FROM FlashCards WHERE Id = @id AND Stack = @stack;
                                UPDATE FlashCards SET Id = Id - 1 WHERE Id > @id AND Stack = @stack";
            ExecuteQuery(sqlCommand, stack: stack, id:card.Id);
            UpdateStack(stack);
        }

        internal void DeleteStack(Stacks stack)
        {
            string sqlCommand = $"DELETE FROM Stacks WHERE Name = @stack";
            ExecuteQuery(sqlCommand, stack:stack.Name);
            sqlCommand = "UPDATE Stacks Set Id = Id - 1 WHERE Id > @id;";
            ExecuteQuery(sqlCommand, id:stack.Id);
        }

        internal List<FlashCard> GetFlashCards(string stack)
        {
            using (SqlConnection connection = new(ConnectionString)) 
            {
                DynamicParameters param = new();
                param.Add("@stack", stack);
            string sqlCommand = $"SELECT Id,* FROM FlashCards WHERE Stack = @stack ORDER BY FlashCards.Id";
                return connection.Query<FlashCard>(sqlCommand, param ).ToList();
            }
        }

        internal List<Stacks> GetStacks()
        {

            using (SqlConnection connection = new(ConnectionString))
            {
                string sqlCommand = $"SELECT Id,* FROM Stacks";
                List<Stacks> stacks = connection.Query<Stacks>(sqlCommand).ToList();
                return stacks;
            } 
        }

        internal void UpdateCard(FlashCard card,string option, string value,string stack)
        {
            string? sqlCommand;
            switch (option)
            {
                case "Question":
                    sqlCommand = "UPDATE FlashCards SET Question = @question WHERE Id = @id";
                    ExecuteQuery(sqlCommand, stack:stack, question: value,id:card.Id);
                    UpdateStack(stack);
                    break;
                case "Answer":
                    sqlCommand = "UPDATE FlashCards SET Answer = @answer WHERE Id = @id and Stack = @stack";
                    ExecuteQuery(sqlCommand, stack: stack, answer: value,id:card.Id);
                    UpdateStack(stack);
                    break;
                case "Stack":
                    sqlCommand = @"UPDATE FlashCards SET Stack = @answer,
                                 Id = (SELECT ISNULL(MAX(Id)+1,1) FROM FlashCards WHERE Stack = @answer)
                                 WHERE Id = @id AND Stack = @stack;
                                 UPDATE FlashCards Set Id = Id - 1 WHERE Id > @id AND Stack = @stack";
                    ExecuteQuery(sqlCommand, stack: stack, answer:value,id:card.Id);
                    UpdateStack(stack);
                    break;
            }
        }

        internal void UpdateStack(string stack)
        {
            string sqlCommand = @$"UPDATE Stacks SET NumberOfCards = (SELECT Count(Stack) FROM FlashCards WHERE Stack = @stack)
                                WHERE Name = @stack;";
            ExecuteQuery(sqlCommand,stack:stack);
        }

        internal void AddNewStack(Stacks stack)
        {
            try
            {
                string sqlCommand = @$"INSERT INTO Stacks (Name,NumberOfCards,Id) VALUES(@stack,@numberOfCards,
                                    (SELECT ISNULL(MAX(Id)+1,1) FROM Stacks))";
                ExecuteQuery(sqlCommand, stack.Name, numberOfCards: stack.NumberOfCards);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"The Stack '{stack.Name}' already exists");
            }
        }

        internal void AddCard(FlashCard card,string stack)
        {
            string sqlCommand = @$"INSERT INTO FlashCards (Question,Answer,Stack,Id) 
                                VALUES(@question,@answer,@stack,
                                (SELECT ISNULL(MAX(Id)+1,1) FROM FlashCards WHERE Stack = @stack))";
            ExecuteQuery(sqlCommand, question: card.Question, answer: card.Answer, stack: stack);
            UpdateStack(stack);
        }

        internal void AddStudySession(StudyModel studySession)
        {
            string sqlCommand = @$"INSERT INTO StudySessions (Date,Stack,QuestionMode,QuestionCount,Score,Time)
                                VALUES(@date,@stack,@questionMode,@questionCount,@score,@time)";
            ExecuteQuery(sqlCommand, date: studySession.Date, stack: studySession.Stack,questionMode:studySession.QuestionMode, questionCount: studySession.QuestionCount, score: studySession.Score,time: studySession.Time);
        }

        internal List<StudyModel> GetAllStudySessions()
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                string sqlCommand = "SELECT Stack,Date,QuestionMode,QuestionCount,Score,(FORMAT(Time,'hh')+':'+FORMAT(Time,'mm')+':'+FORMAT(Time,'ss')) as Time FROM StudySessions ORDER BY Stack,Date;";
                List<StudyModel> sessions = connection.Query<StudyModel>(sqlCommand).ToList();
                return sessions;
            }
        }

        internal List<StudyModel> GetOneStackStudySessions(string stack)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                string sqlCommand = @"SELECT Stack,QuestionMode,QuestionCount,Score,
                               (FORMAT(Time,'hh')+':'+FORMAT(Time,'mm')+':'+FORMAT(Time,'ss')) as Time,
                               (FORMAT(Date,'yyyy')+'-'+FORMAT(Date,'MM')+'-'+FORMAT(Date,'dd')) as Date FROM StudySessions ORDER BY Date;";
                List<StudyModel> sessions = connection.Query<StudyModel>(sqlCommand).Where(x => x.Stack == stack).ToList();
                return sessions;
            }
        }

        internal List<StudyMonthly> GetMonthlyReports(string stack)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                string sqlCommand = $@"SELECT Format(Date,'yyyy-MM') as Month,
                                count(Format(Date,'yyyy-MM')) as QuestionCount ,
                                sum(Score) as TotalScore
                                FROM StudySessions WHERE Stack = @stack GROUP by Format(Date,'yyyy-MM') ORDER BY FORMAT(Date,'yyyy-MM');";
                List<StudyMonthly> monthly = new();
                monthly = connection.Query<StudyMonthly>(sqlCommand, new { stack }).ToList();
                return monthly;
            }
        }
    }
}
