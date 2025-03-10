using FlashCards.Models.FlashCards;
using Spectre.Console;

namespace FlashCards.Views
{
    public static class CardView
    {
        public static void ShowCollection(IEnumerable<FlashCardDTO> cardsList)
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Front");
            table.AddColumn("Back");
            int id = 1;
            foreach (var card in cardsList)
            {
                table.AddRow($"{id++}", card.Name1, card.Name2);
            }
            AnsiConsole.Write(table);
        }
    }
}