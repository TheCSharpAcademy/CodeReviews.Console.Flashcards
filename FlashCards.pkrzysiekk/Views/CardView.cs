using FlashCards.Models.FlashCards;
using Spectre.Console;

namespace FlashCards.Views
{
    public static class CardView
    {
        public static void ShowCollection(IEnumerable<FlashCardDTO> cardsList)
        {
            foreach (var card in cardsList)
            {
                var table = new Table();
                table.AddColumn(card.Name1);
                table.AddRow(card.Name2);
                AnsiConsole.Write(table);
            }

        }
        
    }
}
