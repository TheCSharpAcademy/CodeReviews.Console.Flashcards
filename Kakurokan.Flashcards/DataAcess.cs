using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Spectre.Console;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Kakurokan.Flashcards
{
    public class DataAcess
    {
        private readonly string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Stacks"].ConnectionString;

        public DataAcess()
        {
        }

        public Stacks SelectStack()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var stacks = connection.Query<Stacks>("dbo.GetStacks").ToList();

                var select_stack = new SelectionPrompt<string>().Title("Choose a [green]stack[/].").PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more stacks)[/]");
                foreach (Stacks stack in stacks)
                {
                    select_stack.AddChoice(stack.Name);
                }
                var name_chose_stack = AnsiConsole.Prompt(select_stack);

                foreach (Stacks stack in stacks)
                {
                    if (stack.Name == name_chose_stack) return stack;
                }
                throw new Exception();
            }
        }

        public void InsertStack(Stacks stack)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO Stacks (Name) VALUES (@Name)", stack);
            }
        }

        public void DeleteStack(Stacks stack)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(@"DELETE FROM Stacks WHERE StackId = @StackId", new { StackId = stack.StackId });
            }
        }

        public void ViewFlashcards(Stacks stack)
        {
            var flashcards = GetFlashcards(stack);

            var table = new Table();
            table.AddColumn((new TableColumn("Id").Centered()));
            table.AddColumn((new TableColumn("Question").Centered()));
            table.AddColumn((new TableColumn("Asnwer").Centered()));
            table.Border(TableBorder.Rounded);

            int n1 = 1;
            foreach (Flashcards flashcards1 in flashcards)
            {
                FlashcardsDto clean_flashcard = new FlashcardsDto(flashcards1.Id, flashcards1.Question, flashcards1.Answer);
                table.AddRow(n1.ToString(), clean_flashcard.Question, clean_flashcard.Answer);
                n1 += 1;
            }

            AnsiConsole.Write(table);

            var input = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
            .AddChoices(new[] {"Insert new flashcard",  "Delete a flashcard", "Return to menu"
            }));
            AnsiConsole.Clear();
            switch (input)
            {
                case "Insert new flashcard":
                    Program.CreateFlashcard(stack.StackId);
                    ViewFlashcards(stack);
                    break;
                case "Delete a flashcard":
                    DeleteFlashcards(flashcards);
                    break;
                case "Return to menu":
                    Program.DisplayReturningTomenu();
                    break;
            }

        }

        public List<Flashcards> GetFlashcards(Stacks stack)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var flashcards = connection.Query<Flashcards>("dbo.GetFlashcards").ToList();
                List<Flashcards> clean_flashcards = new List<Flashcards>();
                foreach (Flashcards flashcard in flashcards) if (flashcard.StackId == stack.StackId) clean_flashcards.Add(flashcard);
                return clean_flashcards;
            }
        }
        public void InsertFlashcard(Flashcards flashcard)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO Flashcards (StackId, Question, Answer) VALUES (@StackId, @Question, @Answer)", flashcard);
            }
        }

        public void DeleteFlashcards(List<Flashcards> flashcards)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var options_of_flashcards =
    new MultiSelectionPrompt<string>()
        .Title("Which flashcards do you want to delete?")
        .NotRequired()
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more flashcards)[/]")
        .InstructionsText(
            "[grey](Press [blue]<space>[/] to toggle a flashcard, " +
            "[green]<enter>[/] to accept)[/]");

                foreach (Flashcards flashcard in flashcards) options_of_flashcards.AddChoice(flashcard.ToString());

                List<string> selected_flashcards = AnsiConsole.Prompt(options_of_flashcards);
                foreach (string flashcard in selected_flashcards) connection.Execute("dbo.DeleteFlashcard @Id", new Flashcards(int.Parse(Regex.Match(flashcard, @"\d{1,5}").Value)));
            }
        }

        public void InsertStudySession(StudySessions studySession)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO StudySessions (Score, Date, StackId) VALUES (@Score, @Date, @StackId)", new { studySession.Score, studySession.Date, studySession.StackId });
            }
        }

        public void ViewStudySessions()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                var sessions = connection.Query<StudySessions>("dbo.GetStudySessions").ToList();

                var table = new Table();
                table.AddColumn((new TableColumn("Score").Centered()));
                table.AddColumn((new TableColumn("Stack").Centered()));
                table.AddColumn((new TableColumn("Date").Centered()));
                table.Border(TableBorder.Rounded);

                foreach (StudySessions session in sessions)
                {
                    var stack_name = connection.Query<string>("dbo.StackNameById @param1", new { param1 = session.StackId });
                    table.AddRow(session.Score.ToString(), stack_name.First().ToString(), session.Date);
                }

                AnsiConsole.Write(table);

                var input = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
            .AddChoices(new[] {"New StudySession",  "Return to menu"
            }));
                AnsiConsole.Clear();
                switch (input)
                {
                    case "New StudySession":
                        StudySessionCreator.NewSession();
                        break;
                    case "Return to menu":
                        Program.DisplayReturningTomenu();
                        break;
                }

            }
        }
    }
}
