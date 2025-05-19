using Spectre.Console;
using static Flashcards.glaxxie.Utilities.StylesHelper;
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
        AnsiConsole.MarkupLine(Styled(title, "bold slowblink red"));
        AnsiConsole.MarkupLine(Styled("Press any key to start", "bold grey").PadLeft(65, ' '));
        Console.ReadKey();
        Console.Clear();
        AnsiConsole.MarkupLine(Styled(reversed, "bold blue"));

        Thread.Sleep( 1200 );
        Console.Clear();
    }
}