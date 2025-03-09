using FlashCards.Controllers;
using FlashCards.Models;
using FlashCards.Models.FlashCards;
using FlashCards.Models.MenuEnums;
using FlashCards.Models.Stack;
using Spectre.Console;

namespace FlashCards.Views.Menus
{
    public class StackMenu : IMenu
    {
        private StackController _stackController = new();
        private FlashCardsController _cardController = new();


        private StackMenuEnum GetUserChoice()
        {
            var menuChoice = AnsiConsole.Prompt(new SelectionPrompt<StackMenuEnum>()
                .Title("[Purple]What would you want to do? [/]")
                .AddChoices(Enum.GetValues<StackMenuEnum>())
                .UseConverter(x=>x switch
                {
                    StackMenuEnum.AddStack => "Add Stack",
                    StackMenuEnum.SeeStack => "See Stack",
                    StackMenuEnum.DeleteStack => "Delete Stack",
                    StackMenuEnum.UpdateStack => "Update Stack",
                    StackMenuEnum.AddCardToStack => "Add Card to stack",
                    StackMenuEnum.SeeCardsInStack => "See cards in stack",
                    StackMenuEnum.EditCardFromStack => "Edit card from stack",
                    StackMenuEnum.DeleteCardFromStack => "Delete card from stack",
                    StackMenuEnum.Exit => "Exit",
                    _ => throw new NotImplementedException()
                }
                )
                );
            return menuChoice;
        }
        public void Show()
        {
            StackBO stackBO = new StackBO();
            StackDTO stackDTO = new StackDTO();
            IEnumerable<StackBO> stackListBO = new List<StackBO>();
            IEnumerable<StackDTO> stackListDTO = new List<StackDTO>();
            while (true)
            {
                StackMenuEnum choice = GetUserChoice();
                AnsiConsole.Clear();
                switch (choice)
                {
                    case StackMenuEnum.AddStack:
                        stackBO = UserInput.GetModelToAdd(_ = new StackBO());
                        _stackController.Insert(stackBO);
                        break;
                    case StackMenuEnum.SeeStack:
                        stackListDTO = _stackController.GetAllDTO();
                        stackListBO = _stackController.GetAllBO();
                        if (stackListDTO.Count() == 0)
                        {
                            AnsiConsole.MarkupLine("[White] No stacks, add one first[/]");
                            continue;
                        }
                        StackView.ShowStacks(stackListDTO);
                        break;
                    case StackMenuEnum.DeleteStack:
                        stackListBO = _stackController.GetAllBO();
                        if (stackListBO.Count() == 0)
                        {
                            AnsiConsole.MarkupLine("[White] No stacks, add one first[/]");
                            continue;
                        }
                        stackBO = _stackController.GetUserSelection(stackListBO);
                        _stackController.Remove(stackBO);
                        break;
                    case StackMenuEnum.UpdateStack:
                        stackListBO = _stackController.GetAllBO();
                        if (stackListBO.Count() == 0)
                        {
                            AnsiConsole.MarkupLine("[White] No stacks, add one first[/]");
                            continue;
                        }
                        stackBO = _stackController.GetUserSelection(stackListBO);
                        var newStack = UserInput.GetModelToAdd(_ = new StackBO());
                        _stackController.Update(stackBO, newStack);
                        break;
                    case StackMenuEnum.AddCardToStack:
                        stackListBO = _stackController.GetAllBO();
                        if (stackListBO.Count() == 0)
                        {
                            AnsiConsole.MarkupLine("[White] No stacks, add one first[/]");
                            continue;
                        }
                        stackBO = _stackController.GetUserSelection(stackListBO);
                        AnsiConsole.MarkupLine($"[Purple]Current Stack {stackBO.Name}[/]");
                        var cardToAdd = UserInput.GetModelToAdd(_ = new FlashCardBO());
                        _cardController.Insert(stackBO, cardToAdd);
                        break;
                    case StackMenuEnum.SeeCardsInStack:
                        stackListBO = _stackController.GetAllBO();
                        if (stackListBO.Count() == 0)
                        {
                            AnsiConsole.MarkupLine("[White] No stacks, add one first[/]");
                            continue;
                        }
                        stackBO = _stackController.GetUserSelection(stackListBO);
                        var cardsDTO = _cardController.GetAllCardsDTO(stackBO);
                        CardView.ShowCollection(cardsDTO);

                        break;
                    case StackMenuEnum.EditCardFromStack:
                        stackListBO = _stackController.GetAllBO();
                        if (stackListBO.Count() == 0)
                        {
                            AnsiConsole.MarkupLine("[White] No stacks, add one first[/]");
                            continue;
                        }
                        stackBO = _stackController.GetUserSelection(stackListBO);
                        AnsiConsole.MarkupLine($"[Purple]Current Stack {stackBO.Name}[/]");
                        var currStackCards = _cardController.GetAllCardsFromStack(stackBO);
                        var cardToEdit = _cardController.GetUserCardSelection(currStackCards);
                        AnsiConsole.MarkupLine($"[Purple]Currently editing: {cardToEdit.Name1} || {cardToEdit.Name2}[/]");
                        var newCard = UserInput.GetModelToAdd(_ = new FlashCardBO());
                        _cardController.Update(cardToEdit, newCard);
                        return;
                    case StackMenuEnum.DeleteCardFromStack:
                        stackListBO = _stackController.GetAllBO();
                        if (stackListBO.Count() == 0)
                        {
                            AnsiConsole.MarkupLine("[White] No stacks, add one first[/]");
                            continue;
                        }
                        var stackToDeleteFrom = _stackController.GetUserSelection(stackListBO);
                        AnsiConsole.MarkupLine($"[Purple]Current Stack {stackToDeleteFrom.Name}[/]");
                        var currStackCardsToDelete = _cardController.GetAllCardsFromStack(stackToDeleteFrom);
                        var cardToDelete = _cardController.GetUserCardSelection(currStackCardsToDelete);
                        _cardController.Delete(cardToDelete);
                        break;
                    case StackMenuEnum.Exit:
                        return;
                }
            }

        }
    }
}
