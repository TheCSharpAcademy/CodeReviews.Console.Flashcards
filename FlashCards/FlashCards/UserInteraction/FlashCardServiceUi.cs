using Spectre.Console;

namespace FlashCards
{
    /// <summary>
    /// Represents a user interface for FlashCardService.
    /// Inherits from UserInterace
    /// Implements IFlashCardServiceUi
    /// </summary>
    internal class FlashCardServiceUi : UserInterface, IFlashCardServiceUi
    {
        /// <inheritdoc/>
        public void PrintCards(List<FlashCardDto> cards)
        {
            var table = new Table();
            table.AddColumns("Card ID", "Front Text", "Back Text");
            table.ShowRowSeparators();
            foreach (var card in cards)
            {
                table.AddRow(card.CardID.ToString(), card.FrontText, card.BackText);
            }
            AnsiConsole.Write(table);
        }

        /// <inheritdoc/>
        public FlashCard GetNewCard()
        {
            FlashCard card = new FlashCard();
            card.FrontText = GetStringFromUser("Please enter front text value (question): ");
            card.BackText = GetStringFromUser("Please enter back text value (answer): ");

            return card;
        }

        /// <inheritdoc/>
        public int GetCardID(List<FlashCardDto> cards)
        {
            PrintCards(cards);

            var cardId = AnsiConsole.Prompt(
                new TextPrompt<int>("Please select card: ")
                .AddChoices(cards.Select(x => x.CardID))
                .HideChoices()
                );
            return cardId;
        }
    }
}