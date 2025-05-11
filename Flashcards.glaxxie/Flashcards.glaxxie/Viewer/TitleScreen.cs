using Spectre.Console;

namespace Flashcards.glaxxie.Display;

internal class TitleScreen
{
    internal static void Show()
    {
        var title =
            @$"
        > > >  >      > >    > > >  >   >    > > >    > >  > >    > >
        >      >      >  >   >      >   >    >       >  >  >   >  >  >
        > >    >      > > >  > > >  > > >    >      > > >  > >    >   >
        >      >      >   >      >  >   >    >      >   >  >  >   >  >
        >      > > >  >   >  > > >  >   >    > > >  >   >  >   >  > >
            ";

        var reversed = title.Replace('>', '<');
        AnsiConsole.MarkupLine($"[bold slowblink red]{title}[/]");
        AnsiConsole.MarkupLine($"[bold grey] Press any key to start[/]".PadLeft(65,' '));
        Console.ReadKey();
        Console.Clear();
        AnsiConsole.MarkupLine($"[bold blue]{reversed}[/]");

        Thread.Sleep( 1200 );
    }
}
