using DataAccess;
using Library.Models;
using Spectre.Console;
using System.ComponentModel;

namespace Library;

public class StudySessionController
{
    private readonly IDataAccess dataAccess;
    private readonly InputValidation inputValidation;
    
    public StudySessionController(IDataAccess dataAccess, InputValidation inputValidation)
    {
        this.dataAccess = dataAccess;
        this.inputValidation = inputValidation;
    }

    public StackDTO GetCardsWithStack(List<CardDTO> cards, StackDTO stack)
    {
        foreach (CardDTO card in cards)
        {
            stack.AddFlashcard(card);
        }
        return stack;
    }

    public void PerformStudySession(StackDTO stack)
    {
        int score = 0;
        int card = 1;

        AnsiConsole.Clear();

        AnsiConsole.WriteLine($"Currently studying: {stack.Name}");
        AnsiConsole.WriteLine($"Total questions = {stack.Flashcards.Count}\n");

        foreach (CardDTO flashcards in stack.Flashcards)
        {
            string answer = AnsiConsole.Ask<string>($"Card number {card}: {flashcards.Question} = ");

            if (answer.Trim().ToLower() == flashcards.Answer.Trim().ToLower())
            {
                score++;
                AnsiConsole.WriteLine($"The answer is correct.\n");
            }
            else
            {
                AnsiConsole.WriteLine($"Your answer was incorrect, Correct answer is {flashcards.Answer}\n");
            }
            card++;
            answer = string.Empty;
        }

        StudySessionModel currentSession = new StudySessionModel();
        currentSession.StackId = stack.stackId;
        currentSession.StackName = stack.Name;
        currentSession.SessionDateTime = DateTime.Now;
        currentSession.Score = score;

        LogStudySession(currentSession);

        AnsiConsole.WriteLine($"\n\nStudy session is over, your score is {score}");
        AnsiConsole.WriteLine("Press any key to return to Study Menu");
        Console.ReadLine();
    }

    private void LogStudySession(StudySessionModel session)
    {
        int rowsAffected = dataAccess.InsertStudySession(session);
        if(rowsAffected > 0)
        {
            AnsiConsole.Markup("\n\n[green]Session saved successfully![/]");
        }
        else
        {
            AnsiConsole.Markup("[red]Session saving operation was not successful[/] :( ...");
        }
    }

    public List<StudySessionModel> FetchAllStudySession()
    {
        List<StudySessionModel> studySessions = dataAccess.GetStudySessions();
        return studySessions;
    }

}
