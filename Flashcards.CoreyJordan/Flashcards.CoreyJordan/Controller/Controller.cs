using Flashcards.CoreyJordan.Display;

namespace Flashcards.CoreyJordan.Controller;
internal abstract class Controller
{
    public ConsoleUI UIConsole { get; } = new();
    public PackUI UIPack { get; } = new();
    public InputModel UserInput { get; } = new();
}
