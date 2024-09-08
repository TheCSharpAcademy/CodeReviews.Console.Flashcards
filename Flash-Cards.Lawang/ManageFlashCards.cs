using System;
using Flash_Cards.Lawang.Controller;
using Flash_Cards.Lawang.Models;
using Flash_Cards.Lawang.Views;

namespace Flash_Cards.Lawang;

public class ManageFlashCards
{

    private Validation _validation;
    private Visualize _visual;
    private FlashCardController _flashCardController;
    private List<Stack> _stacks;
    public ManageFlashCards(Validation validation, Visualize visual, FlashCardController flashCardController, List<Stack> stackList)
    {
        _validation = validation;
        _visual = visual;
        _flashCardController = flashCardController;
        _stacks = stackList;
    }

    public void OperationMenu()
    {
        if (_stacks.Count == 0)
        {
            _visual.RenderStackTable(_stacks);
            return;
        }

        List<Option> listOfStack = new List<Option>();

        foreach (var stack in _stacks)
        {
            listOfStack.Add(new Option(stack.Name, stack.Id));
        }

        listOfStack.Add(new Option("Go back to menu", 0));

        var chosenStack = _validation.ChooseOption(listOfStack, "STACK NAMES", "[green bold underline]Choose a stack of [cyan1]flashcards[/] to interact with: [/]");
        if (chosenStack.Value == 0)
        {
            return;
        }

        Console.Clear();
        var listOfOption = new List<Option>()
        {
            new Option("Return to Main menu.", 0),
            new Option("Change current stack.", 1),
            new Option("View all flash-card in stack.", 2),
            new Option("Create a flash-card in current stack.", 3),
            new Option("Edit a flash-card", 4),
            new Option("Delete a flash-card", 5)
        };

        bool exitOption = false;
        do
        {
            var option = _validation.ChooseOption(listOfOption, "CHOOSE FLASH-CARD OPERATION", $"[green bold underline]Current Working stack: [cyan]{chosenStack.Display}[/][/]");
            Console.Clear();

            switch (option.Value)
            {
                case 1:
                    chosenStack = _validation.ChooseOption(listOfStack, "STACK NAMES", "[green bold underline]Choose a stack of [cyan1]flashcards[/] to interact with: [/]");
                    if (chosenStack.Value == 0)
                    {
                        return;
                    }
                    break;
                case 2:
                    ViewAllFlashCards(chosenStack);
                    Console.ReadLine();
                    break;
                case 3:
                    CreateFlashCard(chosenStack);
                    break;
                case 4:
                    ViewAllFlashCards(chosenStack);
                    EditFlashCard(chosenStack);
                    break;
                case 5:
                    ViewAllFlashCards(chosenStack);
                    DeleteFlashCard(chosenStack);
                    break;
                case 0:
                    exitOption = true;
                    break;
            }
            Console.Clear();
        } while (!exitOption);
    }

    private void ViewAllFlashCards(Option chosenStack)
    {
        var flashCards = _flashCardController.GetAllFlashCard(chosenStack);
        var flashCardDTOs = ConvertToDTO(flashCards);
        _visual.RenderFlashCardTable(flashCardDTOs);
        
    }
    private List<FlashCardDTO> ConvertToDTO(List<FlashCard> flashCards)
    {
        var flashCardsDTO = new List<FlashCardDTO>();
        foreach (var flashCard in flashCards)
        {
            flashCardsDTO.Add(new FlashCardDTO()
            {
                Id = flashCard.Id,
                Front = flashCard.Front,
                Back = flashCard.Back
            });
        }

        return flashCardsDTO;
    }

    private void CreateFlashCard(Option chosenStack)
    {

        string? front = _validation.ValidateFlashCard( "Front", chosenStack);
        if (front == null)
        {
            return;
        }

        string? back = _validation.ValidateFlashCard("Back", chosenStack);

        if (back == null)
        {
            return;
        }

        var flashCardDTO = new FlashCardDTO()
        {
            Front = front,
            Back = back
        };

        int rowAffected = _flashCardController.CreateFlashCard(flashCardDTO, chosenStack.Value);
        _visual.RenderResult(rowAffected);
    }

    private void EditFlashCard(Option chosenStack)
    {
        var flashCards = _flashCardController.GetAllFlashCard(chosenStack);

        if (flashCards.Count() == 0)
        {
            Console.ReadLine();
            return;
        }

        var flashCardsDTO = ConvertToDTO(flashCards);
        var editFlashCard = _validation.ValidateEditOrDelete(flashCardsDTO, "edit");
        if (editFlashCard == null)
        {
            return;
        }

        string? front = _validation.ValidateFlashCard("Front");
        if(front == null) return;

        string? back = _validation.ValidateFlashCard("Back");
        if(back == null) return;

        editFlashCard.Front = front;
        editFlashCard.Back = back;

        int affectedRow = _flashCardController.UpdateFlashCard(editFlashCard);

        _visual.RenderResult(affectedRow);
    }

    private void DeleteFlashCard(Option chosenStack)
    {
        var flashCards = _flashCardController.GetAllFlashCard(chosenStack);
        if(flashCards.Count() == 0)
        {
            Console.ReadLine();
            return;
        } 

        var flashCardsDTO = ConvertToDTO(flashCards);
        var deleteFlashCard = _validation.ValidateEditOrDelete(flashCardsDTO, "delete"); 
        if(deleteFlashCard == null)
        {
            return;
        }

        int affectedRow = _flashCardController.DeleteFlashCard(deleteFlashCard);
        _visual.RenderResult(affectedRow);
    }

}
