using Flashcards.Arashi256.Classes;
using Flashcards.Arashi256.Controllers;
using Flashcards.Arashi256.Models;
using Spectre.Console;

namespace Flashcards.Arashi256.Views
{
    internal class FlashcardView
    {
        public FlashcardController FlashcardController { get; private set; }
        private StackView _stackView;

        public FlashcardView(StackView stackView)
        {
            _stackView = stackView;
            FlashcardController = new FlashcardController(_stackView.StackController);
        }

        public void AddNewFlashcard()
        {
            bool error = false;
            int stackId = 0;
            string flashcardFront = string.Empty;
            string flashcardBack = string.Empty;
            StackDto? selectedStack = null;
            FlashcardDto newFlashcard = null;
            List<StackDto> stacks = _stackView.ViewStacks();
            if (stacks != null && stacks.Count > 0)
            {
                stackId = CommonUI.GetNumberInput("Please select a Stack ID to add a flashcard to: ", 0, stacks.Count);
                if (stackId == -1)
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                }
                else
                {
                    selectedStack = stacks[stackId - 1];
                    do
                    {
                        AnsiConsole.MarkupLine("Enter 'Q' to cancel operation");
                        flashcardFront = AnsiConsole.Ask<string>("What is the flashcard [orange1]front[/] information? ");
                        if (flashcardFront.ToLower() == "q")
                        {
                            AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                            break;
                        }
                        flashcardBack = AnsiConsole.Ask<string>("What is the flashcard [orange1]back[/] information? ");
                        if (flashcardFront.ToLower() == "q")
                        {
                            AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                            break;
                        }
                        newFlashcard = new FlashcardDto();
                        newFlashcard.StackId = (int)selectedStack.Id;
                        newFlashcard.Subject = selectedStack.Subject;
                        newFlashcard.Front = flashcardFront;
                        newFlashcard.Back = flashcardBack;
                        bool result = FlashcardController.AddFlashcard(newFlashcard);
                        if (result)
                        {
                            AnsiConsole.MarkupLine($"[yellow]Flashcard for {newFlashcard.Subject}' stack successfully added.[/]");
                            ViewFlashcard(newFlashcard);
                        }
                        else
                        {
                            AnsiConsole.MarkupLine($"[red]There was a problem adding the new flashcard for stack '{newFlashcard.Subject}' Please try again.[/]");
                            error = true;
                        }
                    } while (error);
                }
            }
        }

        private void ViewFlashcard(FlashcardDto flashcard)
        {
            Table tblFlashcard = new Table();
            tblFlashcard.AddColumn(new TableColumn($"[yellow]Subject[/]").LeftAligned());
            tblFlashcard.AddColumn(new TableColumn($"[white]{flashcard.Subject}[/]").RightAligned());
            tblFlashcard.AddRow($"[yellow]Front[/]", $"[white]{flashcard.Front}[/]");
            tblFlashcard.AddRow($"[yellow]Back[/]", $"[white]{flashcard.Back}[/]");
            tblFlashcard.Alignment(Justify.Center);
            AnsiConsole.Write(tblFlashcard);
        }

        public void ViewFlashcardsInStack()
        {
            int stackId = 0;
            StackDto? selectedStack = null;
            List<StackDto> stacks = _stackView.ViewStacks();
            List<FlashcardDto> flashcards;
            if (stacks != null && stacks.Count > 0)
            {
                stackId = CommonUI.GetNumberInput("Please select a Stack ID to get flashcards: ", 0, stacks.Count);
                if (stackId == -1)
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                }
                else
                {
                    selectedStack = stacks[stackId - 1];
                    flashcards = FlashcardController.GetAllFlashcardsForStack((int)selectedStack.Id);
                    ViewFlashcards(flashcards);
                }
            }
        }

        public void ViewFlashcards(List<FlashcardDto> flashcards)
        {
            Table tblFlashcardList = new Table();
            tblFlashcardList.AddColumn(new TableColumn("[yellow]ID[/]").LeftAligned());
            tblFlashcardList.AddColumn(new TableColumn("[yellow]Subject[/]").RightAligned());
            tblFlashcardList.AddColumn(new TableColumn("[yellow]Front[/]").LeftAligned());
            tblFlashcardList.AddColumn(new TableColumn("[yellow]Back[/]").LeftAligned());
            if (flashcards.Count > 0)
            {
                for (int i = 0; i < flashcards.Count; i++)
                {
                    tblFlashcardList.AddRow($"[white]{flashcards[i].DisplayId}[/]", $"[darkorange]{flashcards[i].Subject}[/]", $"[white]{flashcards[i].Front}[/]", $"[white]{flashcards[i].Back}[/]");
                }
            }
            else
            {
                tblFlashcardList.AddRow("", "[red]No flashcards for stack found[/]");
            }
            tblFlashcardList.Alignment(Justify.Center);
            AnsiConsole.Write(tblFlashcardList);
        }

        public void UpdateFlashcard()
        {
            bool error = false;
            int stackId = 0;
            int flashcardId = 0;
            string flashcardFront = string.Empty;
            string flashcardBack = string.Empty;
            StackDto? selectedStack = null;
            FlashcardDto updatedFlashcard = null;
            List<FlashcardDto> flashcards;
            List<StackDto> stacks = _stackView.ViewStacks();
            if (stacks != null && stacks.Count > 0)
            {
                stackId = CommonUI.GetNumberInput("Please select a Stack ID to get flashcards from: ", 0, stacks.Count);
                if (stackId == -1)
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                }
                else
                {
                    selectedStack = stacks[stackId - 1];
                    flashcards = FlashcardController.GetAllFlashcardsForStack((int)selectedStack.Id);
                    ViewFlashcards(flashcards);
                    if (flashcards.Count > 0)
                    {
                        flashcardId = CommonUI.GetNumberInput("Please select a flashcard ID to update: ", 0, flashcards.Count);
                        if (flashcardId != -1)
                        {
                            updatedFlashcard = flashcards[flashcardId - 1];
                            AnsiConsole.MarkupLine("[white]Enter new flashcard details[/]");
                            // Enter new details.
                            do
                            {
                                AnsiConsole.MarkupLine("Enter 'Q' to cancel operation");
                                flashcardFront = AnsiConsole.Ask<string>("What is the flashcard [orange1]front[/] information? ");
                                if (flashcardFront.ToLower() == "q")
                                {
                                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                                    break;
                                }
                                flashcardBack = AnsiConsole.Ask<string>("What is the flashcard [orange1]back[/] information? ");
                                if (flashcardFront.ToLower() == "q")
                                {
                                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                                    break;
                                }
                                updatedFlashcard.Front = flashcardFront;
                                updatedFlashcard.Back = flashcardBack;
                                bool result = FlashcardController.UpdateFlashcard(updatedFlashcard);
                                if (result)
                                {
                                    AnsiConsole.MarkupLine($"[yellow]Flashcard for '{updatedFlashcard.Subject}' stack successfully updated.[/]");
                                    ViewFlashcard(updatedFlashcard);
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine($"[red]There was a problem updating the flashcard for stack '{updatedFlashcard.Subject}' Please try again.[/]");
                                    error = true;
                                }
                            } while (error);
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                        }
                    }
                }
            }
        }

        public void DeleteFlashcard()
        {
            int stackId = 0;
            StackDto? selectedStack = null;
            FlashcardDto deleteFlashcard = null;
            List<FlashcardDto> flashcards;
            int flashcardId = 0;
            List<StackDto> stacks = _stackView.ViewStacks();
            if (stacks != null && stacks.Count > 0)
            {
                stackId = CommonUI.GetNumberInput("Please select a Stack ID to get flashcards from: ", 0, stacks.Count);
                if (stackId == -1)
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                }
                else
                {
                    selectedStack = stacks[stackId - 1];
                    flashcards = FlashcardController.GetAllFlashcardsForStack((int)selectedStack.Id);
                    ViewFlashcards(flashcards);
                    flashcardId = CommonUI.GetNumberInput("Please select a flashcard ID to delete: ", 0, flashcards.Count);
                    if (flashcardId != 0)
                    {
                        deleteFlashcard = flashcards[flashcardId - 1];
                        ViewFlashcard(deleteFlashcard);
                        if (AnsiConsole.Confirm($"Are you sure you want to delete this flashcard for '{deleteFlashcard.Subject}'? "))
                        {
                            bool result = FlashcardController.DeleteFlashcard(deleteFlashcard);
                            if (result)
                            {
                                AnsiConsole.MarkupLine($"[yellow]Flashcard for '{deleteFlashcard.Subject}' successfully deleted.[/]");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine($"[red]There was a problem deleting the flashcard for '{deleteFlashcard.Subject}' Please try again.[/]");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                    }
                }
            }
        }
    }
}
