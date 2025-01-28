using Spectre.Console;

namespace FunRun.Flashcards;

public class FlashcardApp
{
    public async Task RunApp()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Flashcards").Centered().Color(Color.Blue));

            AnsiConsole.MarkupLine("[blue] Inpired by the [link=https://thecsharpacademy.com/project/14/flashcards]C#Acadamy [/][/]");
            AnsiConsole.MarkupLine("");


            //var habits = _crud.ReturnAllHabits();
            Console.ReadKey(true);
        }
    }
}
