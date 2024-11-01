using Spectre.Console;
using System.Collections.Generic;
using System.Threading;

namespace Kakurokan.Flashcards
{
    internal static class StudySessionCreator
    {
        public static async void NewSession()
        {
            DataAcess dataAcess = new DataAcess();
            AnsiConsole.Clear();
            Stacks stack = dataAcess.SelectStack();
            List<Flashcards> flashcards = dataAcess.GetFlashcards(stack);
            StudySessions studySession = new StudySessions(stack.StackId);

            foreach (Flashcards flashcard in flashcards)
            {
                string answer = AnsiConsole.Ask<string>(flashcard.Question);
                if (answer == flashcard.Answer) studySession.Score++;
                AnsiConsole.Clear();
            }
            AnsiConsole.Markup("[green]Congratulations![/] Your score was " + studySession.Score.ToString());
            dataAcess.InsertStudySession(studySession);
            Thread.Sleep(1000);
            Program.DisplayReturningTomenu();
        }
    }
}
