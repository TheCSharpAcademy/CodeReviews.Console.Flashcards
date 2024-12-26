using Spectre.Console;
using FlashCards.FlashCardsManager.Controllers;
using FlashCards.FlashCardsManager.Models;
using FlashCards.Database;
namespace FlashCards.FlashCardsManager.Views
{
    internal class FlashCardsView
    {
        public FlashCardsView(DataTools dataTools,StacksController stacksController,FlashCardsController flashCardsController)
        {
            bool run = true;
            bool validation;
            do
            {
                AnsiConsole.Clear();
                Stacks stack = stacksController.GetStack(dataTools);
                if (stack.Name != "[red]Cancel[/]")
                {
                    try
                    {
                        AnsiConsole.Clear();
                        FlashCard? selectedCard = flashCardsController.GetFlashCard(stack.Name,dataTools);
                
                        string? option = selectedCard.Question == "[red]Cancel[/]" ? "[red]Cancel[/]" : AnsiConsole.Prompt(
                            new SelectionPrompt<string>().AddChoices("[red]Cancel[/]", "Update", "Delete"));
                
                        if (option == "Delete")
                        {
                            AnsiConsole.Clear();
                            validation = UserInputs.Validation($"Are you sure you want to delete this card?");
                            if (validation) dataTools.DeleteCard(selectedCard,stack.Name);
                        }
                        else if (option == "Update")
                        {
                            AnsiConsole.Clear();
                            string? value;
                            option = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("[red]Cancel[/]", "Question", "Answer", "Stack"));
                            if (option != "[red]Cancel[/]")
                            {
                                if (option == "Stack")
                                {
                                    value = stacksController.GetStack(dataTools).Name;
                                    if (value != "[red]Cancel[/]")
                                    {
                                        validation = UserInputs.Validation($"Are you sure you want to update this card?");
                                        if (validation)
                                        {
                                            dataTools.UpdateCard(selectedCard, option, value, stack.Name);
                                            dataTools.UpdateStack(value);
                                        }
                                    }
                                }
                                else
                                {
                                    value = UserInputs.GetInputString($"Enter a New {option}:");
                                    validation = UserInputs.Validation($"Are you sure you want to update this card?");
                                    if (validation)
                                    {
                                        dataTools.UpdateCard(selectedCard, option, value,stack.Name);

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
                run = UserInputs.Validation("Do you want to keep managing cards?");
            } while (run);
        }
    }
}
