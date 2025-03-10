using FlashCards.Controllers;
using FlashCards.Models;
using FlashCards.Models.FlashCards;
using FlashCards.Models.MenuEnums;
using Spectre.Console;

namespace FlashCards.Views.Menus
{
    public class FlashCardMenu : IMenu
    {
        private FlashCardsController _flashCardController = new();

        private FlashCardEnum GetUserChoice()
        {
            var menuChoice = AnsiConsole.Prompt(new SelectionPrompt<FlashCardEnum>()
              .Title("[Green]What would you want to do? [/]")
              .AddChoices(Enum.GetValues<FlashCardEnum>())
             .UseConverter(x => x switch
                {
                    FlashCardEnum.See_Flashcards => "See FlashCards",
                    FlashCardEnum.Edit_FlashCard => "Edit FlashCard",
                    FlashCardEnum.Delete_FlashCard => "Delete FlashCard",
                    FlashCardEnum.Exit => "Exit",
                    _ => throw new NotImplementedException()
                }
                )
              );
            return menuChoice;
        }

        public void Show()
        {
            while (true)
            {
                FlashCardEnum choice = GetUserChoice();
                IEnumerable<FlashCardBO> cardsList = new List<FlashCardBO>();
                IEnumerable<FlashCardDTO> cardsListDTO = new List<FlashCardDTO>();
                AnsiConsole.Clear();
                switch (choice)
                {
                    case FlashCardEnum.See_Flashcards:
                        cardsListDTO = _flashCardController.GetAllCardsDTO();
                        if (cardsListDTO.Count() == 0)
                        {
                            AnsiConsole.MarkupLine($"[White]No cards,create one first[/]");
                            continue;
                        }
                        CardView.ShowCollection(cardsListDTO);
                        break;

                    case FlashCardEnum.Delete_FlashCard:
                        cardsList = _flashCardController.GetAllCardsBO();
                        if (cardsList.Count() == 0)
                        {
                            AnsiConsole.MarkupLine($"[White]No cards,create one first[/]");
                            continue;
                        }
                        var cardToDelete = _flashCardController.GetUserCardSelection(cardsList);
                        _flashCardController.Delete(cardToDelete);
                        break;

                    case FlashCardEnum.Edit_FlashCard:
                        cardsList = _flashCardController.GetAllCardsBO();
                        if (cardsList.Count() == 0)
                        {
                            AnsiConsole.MarkupLine($"[White]No cards,create one first[/]");
                            continue;
                        }
                        var cardToEdit = _flashCardController.GetUserCardSelection(cardsList);
                        AnsiConsole.MarkupLine($"[Blue] Currently edditing: {cardToEdit.Name1} || {cardToEdit.Name2} [/]");
                        var newCard = UserInput.GetModelToAdd(_ = new FlashCardBO());
                        _flashCardController.Update(cardToEdit, newCard);
                        break;

                    case FlashCardEnum.Exit:
                        return;
                }
            }
        }
    }
}