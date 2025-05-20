using Flashcards.glaxxie.Controllers;
using Flashcards.glaxxie.Display;
using Flashcards.glaxxie.DTO;
using Flashcards.glaxxie.Utilities;
using static Flashcards.glaxxie.Utilities.StylesHelper;
using Spectre.Console;

namespace Flashcards.glaxxie.Viewer;

internal class PracticeUI
{
    internal static void Practice(StackViewer stack)
    {
        Console.CursorVisible = false;
        CardDrawer drawer = new();

        var cards = CardController.GetCardsFromStack(stack.StackId);
        if (cards.Count == 0)
        {
            Menu.ClearDisplay("This stack is empty. Press any key to continue");
            return;
        }
        int count = 0;
        int score = 0;
        while (count < cards.Count)
        {
            Console.Clear();
            CardDisplay card = new(Seq: count + 1, StackName: stack.Name, Front: cards[count].Front, Back: cards[count].Back);
            drawer.Draw(card);
            var prompt = AnsiConsole.Prompt(new TextPrompt<string>(">> Answer").PromptStyle("green").DefaultValue("quit"));
            if (prompt.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
            {
                Menu.ClearDisplay("Quitting practice mode...");
                break;
            }

            Console.Clear();
            drawer.Draw(card, true);

            if (CheckAnswer(card.Back, prompt))
            {
                score++;
                Console.WriteLine("Correct! Press any key to continue");
            } else
            {
                Console.WriteLine("Wrong asnwer. Press any key to continue");
            }
            count++;
            Console.ReadKey();
        }
        Console.WriteLine($"FINAL SCORE: {score} / {count}");

        SessionCreation ses = new(StackId: stack.StackId, Date: DateTime.Now, Cards: cards.Count, Score: score);
        SessionController.Insert(ses);
        Menu.ClearDisplay();
    }

    internal static bool CheckAnswer(string answer, string input) => answer.Contains(input, StringComparison.OrdinalIgnoreCase);
}

internal class CardDrawer
{
    private CardStyleDto CardStyle { get; }

    internal CardDrawer()
    {
        var layout = AppSettings.CardLayout;
        CardStyle = AppSettings.StyleGetter(layout);
    }

    internal void Draw(CardDisplay card, bool showBack = false)
    {
        var content = showBack ? card.Back : card.Front;
        var style = showBack ? "bold cyan" : "bold cyan";

        var panel = new Panel(Align.Center(new Markup(content), VerticalAlignment.Middle)).Expand();
        panel.Header = new PanelHeader(Styled(card.StackName, style)).Centered();
        panel.Padding = CardStyle.Padding;
        var grid = new Grid();
        grid.AddColumn(new GridColumn().Width(35));
        grid.AddRow(panel);

        AnsiConsole.Write(grid);
    }
}