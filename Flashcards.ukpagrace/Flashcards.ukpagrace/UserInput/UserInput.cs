using Flashcards.ukpagrace.Database;
using Flashcards.ukpagrace.Entity;
using Spectre.Console;

namespace Flashcards.ukpagrace.Utility
{
    class UserInput
    {
        StackDatabase stackDatabase  = new ();
        public string GetStackInput()
        {
            var stackName = AnsiConsole.Ask<string>("Enter a [green]stack name[/]?");
            return stackName;
        }

        public string GetQuestionInput()
        {
            var question = AnsiConsole.Ask<string>("Create a [green]question[/]?");
            return question;
        }


        public string GetAnswerInput()
        {
            var answer = AnsiConsole.Ask<string>("Create a [green]Answer[/]?");
            return answer;
        }
        public string GetStackOption()
        {
            List<string> stackList = new();
            foreach (StackEntity record in stackDatabase.Get())
            {
                stackList.Add(record.StackName);

            }

            var stackName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a [green]stack[/]")
                    .PageSize(7)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(stackList.ToArray<string>())
                );
            return stackName;
        }


        public int GetFlashCardInput()
        {
            var flashcardId = AnsiConsole.Ask<int>("choose an [green]Id[/]");
            return flashcardId;


        }
        public string GetUpdateInput()
        {
            var updateOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What [green]part[/] do you want to update?")
                    .AddChoices(new[] {"StackId","Question", "Answer"})
                );
            return updateOption;
        }
    }
}