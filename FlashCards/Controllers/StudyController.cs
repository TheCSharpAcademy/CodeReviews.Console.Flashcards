using FlashCards.Models;
using FlashCards.Models.FlashCards;
using Spectre.Console;

namespace FlashCards.Controllers
{
    public class StudyController : DbController
    {
       public SessionBO StartSession(IEnumerable<FlashCardBO> cards)
        {
            int score=0;
            int currRound=1;
            int rounds=cards.Count();
            AnsiConsole.MarkupLine($"");
            foreach (var card in cards)
            {
                AnsiConsole.MarkupLine($"[Green]Round {currRound}/{rounds}[/]");
                string userResponse=AnsiConsole.Ask<string>($"[Blue]{card.Name1}[/]");
                if (userResponse.ToLower() == card.Name2.ToLower())
                {
                    AnsiConsole.MarkupLine($"[Green]Good![/]");
                    score++;

                }
                else
                {
                    AnsiConsole.MarkupLine($"[Red] False![/]");
                }
                currRound++;

            }
            SessionBO session = new SessionBO()
            {
                Score = score,
                Date = DateTime.Now,
                MaxScore = rounds,
                
            };
            return session;
        }

        
    }

    
}
