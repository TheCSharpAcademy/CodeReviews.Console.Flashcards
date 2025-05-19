using Flashcards.glaxxie.Controllers;
using Flashcards.glaxxie.Display;
using Flashcards.glaxxie.DTO;
using static Flashcards.glaxxie.Utilities.StylesHelper;
using Spectre.Console;

namespace Flashcards.glaxxie.Viewer;

internal class ReviewUI
{
    internal static void Review(StackViewer stack)
    {
        Console.CursorVisible = false;
        var cards = CardController.GetCardsFromStack(stack.StackId);
        var totalCard = cards.Count;

        if (totalCard == 0)
        {
            Menu.ClearDisplay($"There is [red]ZERO[/] cards in this stack to review");
            return;
        }

        int cardIndex = 0;
        while (true)
        {
            CardDisplay card = new(Seq: cardIndex + 1, StackName: stack.Name, Front: cards[cardIndex].Front, Back: cards[cardIndex].Back);
            ShowCard(card, totalCard);
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    cardIndex = (cardIndex - 1 + totalCard) % totalCard;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    cardIndex = (cardIndex + 1) % totalCard;
                    break;
                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    return;
            }
            Console.Clear();
        }
    }

    internal static void ShowCard(CardDisplay card, int count)
    {
        var layout = new Layout("Root")
            .SplitRows(
                new Layout("Card").Size(13)
                    .SplitColumns(
                        new Layout("Front").Size(45),
                        new Layout("Back").Size(45)
                    ),
                new Layout("Row1").Size(6)
                    .SplitColumns(
                            new Layout("Info").Size(90),
                            new Layout("Clear").Size(45)
                    ),
                new Layout("Row2").Size(6)
                    .SplitColumns(
                            new Layout("Ins").Size(90),
                            new Layout("Clear2").Size(45)
                    ));

        layout["Clear"].Invisible();
        layout["Clear2"].Invisible();

        layout["Front"].Update(InnerPanel(card.Front, "FRONT"));
        layout["Back"].Update(InnerPanel(card.Back, "BACK"));

        var info = $"Stack: {card.StackName}  [[{card.Seq} / {count}]]";
        layout["Info"].Update(InnerPanel(info, "STACK INFO"));

        var ins = $"Press [[[green]<- | A[/]]] to move left, [[[green]D | ->[/]]] to move right. Or [[[red]Q | Esc[/]]] to quit";
        layout["Ins"].Update(InnerPanel(ins, "INSTRUCTION"));

        AnsiConsole.Write(layout);
    }

    private static Panel InnerPanel(string info, string header)
    {
        var p = new Panel(Align.Center(new Markup(info), VerticalAlignment.Middle))
        {
            Header = new PanelHeader(Styled(header, "bold cyan")).Centered(),
            Expand = true
        };
        return p;
    }
}