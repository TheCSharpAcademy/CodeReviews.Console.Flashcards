using FlashCardsLibrary;
using FlashCardsLibrary.Models;
using Spectre.Console;

namespace FlashCards
{
    public class StudySessionBuilder
    {
        public static void StartSession()
        {
            var date = DateTime.Now;
            var stacks = StackController.GetStackNames();
            
            var sessionName = AnsiConsole.Prompt(new TextPrompt<string>("Enter Session Name: ")).Trim();
             
            foreach (var name in stacks) 
            {
                Console.WriteLine($"{stacks.IndexOf(name)+1}- {name.Name}");
            }
            var stack = stacks[InputManager.GetOption(1, stacks.Count, quantity: "Stack No. ") - 1];
            int score = 0;
            foreach (var card in FlashCardController.GetFlashCards(stack)) 
            {
                Console.Clear();
                var panel = new Panel($"{card.Front}");
                panel.Header("Front");
                panel.Expand();
                
                AnsiConsole.Write(panel);
                string name = AnsiConsole.Prompt(new TextPrompt<string>("Enter Card's Back:"));
                if (name.Trim().ToLower() == card.Back.Trim().ToLower())
                {
                    AnsiConsole.MarkupLine("[green]Good Job[/]");                    
                    score++;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Sorry Incorrect.Check Answer[/]");
                    panel = new Panel($"{card.Back}");
                    panel.Header("Back");
                    panel.BorderColor(Color.Green);
                    panel.Expand();
                    AnsiConsole.Write(panel);
                }
                MenuManager.EnterPause();
            }
            var session = new StudySession(sessionName,stack,date,score);
            StudySessionController.AddSession(session);
        }
    }
}
