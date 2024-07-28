using System.Text.RegularExpressions;
using FlashCards.kwm0304.Models;
using Spectre.Console;

namespace FlashCards.kwm0304.Utils;

public partial class QuizUtils
{
  public int TakeQuiz(List<FlashCard> flashcards)
  {
    int score = 0;
    foreach (FlashCard card in flashcards)
    {
      Console.Clear();
      string answer = AskQuestion(card);
      string correctAnswer = card.Answer;
      bool isCorrect = CheckAnswer(answer, correctAnswer);
      if (isCorrect)
      {
        score++;
        AnsiConsole.MarkupLine($"[bold green]CORRECT![/] Score: {score}");
        Thread.Sleep(1500);
      }
      else
      {
        AnsiConsole.MarkupLine($"[bold red]INCORRECT[/] Score: {score}");
        AnsiConsole.WriteLine($"The correct answer was: {correctAnswer}");
        Thread.Sleep(1500);
      }
    }
    return score;
  }

  private static bool CheckAnswer(string answer, string correctAnswer)
  {
    string[] answerArr = Punctuation().Replace(answer, "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
    string[] correctArr = Punctuation().Replace(correctAnswer, "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var matchingSet = new HashSet<string>(correctArr, StringComparer.OrdinalIgnoreCase);
    foreach (string word in answerArr)
    {
      if (!matchingSet.Contains(word))
      {
        return false;
      }
    }
    return true;
  }

  public void DisplayInstructions()
  {
    AnsiConsole.WriteLine("Your attempt will be correct if the correct answer contains all of the words in your attempt.");
    Thread.Sleep(2000);
  }

  public static string AskQuestion(FlashCard card)
  {
    AnsiConsole.MarkupLine("[bold blue]What is your answer[/]");
    return AnsiConsole.Ask<string>($"{card.Question}\n");
  }

  [GeneratedRegex(@"[^\w\s]")]
  private static partial Regex Punctuation();
}