using Flashcards.Application.Helpers;
using Flashcards.DAO;
using Flashcards.Database;
using Flashcards.DTO;
using Flashcards.Enums;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.Application;

public class AppManageFlashCards
{
    private readonly StackDao _stackDao;
    private readonly FlashCardDao _flashCardDao;
    private readonly InputHandler _inputHandler;
    private readonly ManageStacksHelper _manageStacksHelper;
    private readonly string _pageHeader = "Manage FlashCards";
    private bool _running;


    public AppManageFlashCards(DatabaseContext databaseContext, InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
        _stackDao = new StackDao(databaseContext);
        _flashCardDao = new FlashCardDao(databaseContext);
        _manageStacksHelper = new ManageStacksHelper(_stackDao, _flashCardDao, _inputHandler);

        _running = true;
    }

    public void Run()
    {
        while (_running)
        {
            AnsiConsole.Clear();
            Utilities.DisplayPageHeader(_pageHeader);
            PromptForSessionAction();
        }
    }

    private void PromptForSessionAction()
    {
        FlashCardMenuOption selectedOption = _inputHandler.PromptMenuSelection<FlashCardMenuOption>();
        ExecuteSelectedOption(selectedOption);
    }

    private void ExecuteSelectedOption(FlashCardMenuOption option)
    {
        switch (option)
        {
            case FlashCardMenuOption.Cancel:
                CloseSession();
                break;
            case FlashCardMenuOption.EditFlashCard:
                HandleEditFlashCardSelection();
                break;
            case FlashCardMenuOption.DeleteFlashCard:
                HandleDeleteFlashCardSelection();
                break;
        }
    }

    private void CloseSession()
    {
        _running = false;
    }

    private void HandleDeleteFlashCardSelection()
    {
        IEnumerable<FlashCardDto>? flashCards;

        try 
        {
            flashCards = _flashCardDao.GetAllFlashCards();
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error retrieving flashcards.", ex.Message);
            return;
        }

        if (flashCards == null)
        {
            _manageStacksHelper.HandleNoFlashCardsFound();
            return;
        }

        FlashCardDto flashCard = _inputHandler.PromptListSelectionFlashCard(flashCards, "Select a flash card to delete:");

        if (flashCard.CardID == 0)
        {
            Utilities.DisplayInformationConsoleMessage("No flash card selected.");
            _inputHandler.PauseForContinueInput();
            return;
        }

        try
        {
            bool result = _flashCardDao.DeleteFlashCard(flashCard);
            if (result)
            {
                Utilities.DisplaySuccessMessage("Flash card deleted successfully.");
            }
            else
            {
                Utilities.DisplayWarningMessage("Flash card not deleted.");
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error deleting flash card.", ex.Message);
        }
        finally
        {
            _inputHandler.PauseForContinueInput();
        }
    }

    private void HandleEditFlashCardSelection()
    {
        IEnumerable<FlashCardDto>? flashCards;

        try
        {
            flashCards = _flashCardDao.GetAllFlashCards();
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error retrieving flashcards.", ex.Message);
            return;
        }

        if (flashCards == null)
        {
            _manageStacksHelper.HandleNoFlashCardsFound();
            return;
        }

        FlashCardDto flashCard = _inputHandler.PromptListSelectionFlashCard(flashCards, "Select a flash card to edit:");

        if (flashCard.CardID == 0)
        {
            Utilities.DisplayInformationConsoleMessage("No flash card selected.");
            _inputHandler.PauseForContinueInput();
            return;
        }

        IEnumerable<EditablePropertyFlashCard> selectedProperties = _inputHandler.PromptForEditFlashCardPropertiesSelection();

        if (!selectedProperties.Any())
        {
            Utilities.DisplayCancellationMessage("Operation cancelled.");
            _inputHandler.PauseForContinueInput();
            return;
        }

        foreach (var property in selectedProperties)
        {
            switch (property)
            {
                case EditablePropertyFlashCard.Front:
                    string newFront = _inputHandler.PromptForNonEmptyString("Enter a new front for the flash card:    ");
                    flashCard.Front = newFront;
                    break;

                case EditablePropertyFlashCard.Back:
                    string newBack = _inputHandler.PromptForNonEmptyString("Enter a new back for the flash card:    ");
                    flashCard.Back = newBack;
                    break;
            }
        }

        try
        {
            _flashCardDao.UpdateFlashCard(flashCard);
            Utilities.DisplaySuccessMessage("Flash card updated successfully.");
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error updating flash card.", ex.Message);
        }
        finally
        {
            _inputHandler.PauseForContinueInput();
        }
    }
}
